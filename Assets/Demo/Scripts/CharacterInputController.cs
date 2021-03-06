﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputNew;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

public class CharacterInputController
	: MonoBehaviour
{
    [HideInInspector]
    public FirstPersonControls m_MapInput;

    [HideInInspector]
    public Rigidbody m_Rigid;

    [HideInInspector]
    public Vector2 m_Rotation = Vector2.zero;

	float m_TimeOfLastShot;

    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform head;


	public float timeBetweenShots = 0.5f;

    [HideInInspector]
    public DragRigidbody drag;

    [Space(10)]

    public Transform lastCheckpoint;

    /// <summary>
    /// 
    /// 
    /// </summary>


    [HideInInspector]
    public float DistanceToTarget;

    [HideInInspector]
    public Vector3 currentSpeed;
    //

    //setup
    [HideInInspector]
    public Transform mainCam;
    [HideInInspector]
    public Transform floorChecks;      //main camera, and floorChecks object. FloorChecks are raycasted down from to check the player is grounded.
    [HideInInspector]
    public Animator animator;                   //object with animation controller on, which you want to animate

    public AudioClip landSound;                 //play when landing on ground

    //movement
    public float accel = 70f;                   //acceleration/deceleration in air or on the ground
    public float airAccel = 18f;
    public float decel = 7.6f;
    public float airDecel = 1.1f;
    [Range(0f, 5f)]
    public float rotateSpeed = 0.7f, airRotateSpeed = 0.4f; //how fast to rotate on the ground, how fast to rotate in the air
    public float maxSpeed = 9;                              //maximum speed of movement in X/Z axis
    public float slopeLimit = 40, slideAmount = 35;         //maximum angle of slopes you can walk on, how fast to slide down slopes you can't
    public float movingPlatformFriction = 7.7f;             //you'll need to tweak this to get the player to stay on moving platforms properly

    //jumping
    public Vector3 jumpForce = new Vector3(0, 3, 0);       //normal jump force
    public float jumpLeniancy = 0.1f;                      //how early before hitting the ground you can press jump, and still have it work
    [HideInInspector]
    public int onEnemyBounce;

    private int onJump;
    private bool grounded;
    private Transform[] floorCheckers;

    [HideInInspector]
    private float curAccel, curDecel, curRotateSpeed, slope;
    private Vector3 direction, moveDirection, movingObjSpeed;

    //private CharacterMotor characterMotor;
    private Rigidbody rigid;

    public void Start()
	{
		m_MapInput = playerInput.GetActions<FirstPersonControls>();

		m_Rigid = GetComponent<Rigidbody>();

        drag = GetComponent<DragRigidbody>();

        LockCursor(true);

        mainCam = GetComponent<PlayerInput>().transform;

        animator = GetComponentInChildren<Animator>();
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        //set up rigidbody constraints
        rigid.interpolation = RigidbodyInterpolation.Interpolate;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        //add frictionless physics material
        if (GetComponent<Collider>().material.name == "Default (Instance)")
        {
            PhysicMaterial pMat = new PhysicMaterial();
            pMat.name = "Frictionless";
            pMat.frictionCombine = PhysicMaterialCombine.Multiply;
            pMat.bounceCombine = PhysicMaterialCombine.Multiply;
            pMat.dynamicFriction = 0f;
            pMat.staticFriction = 0f;
            GetComponent<Collider>().material = pMat;
        }

        //create single floorcheck in centre of object, if none are assigned
        if (!floorChecks)
        {
            floorChecks = new GameObject().transform;
            floorChecks.name = "FloorChecks";
            floorChecks.parent = transform;
            floorChecks.position = transform.position;
            GameObject check = new GameObject();
            check.name = "Check1";
            check.transform.parent = floorChecks;
            check.transform.position = transform.position;
        }
        //assign player tag if not already
        if (tag != "Player")
        {
            tag = "Player";
        }
        //usual setup
        rigid = GetComponent<Rigidbody>();

        //gets child objects of floorcheckers, and puts them in an array
        //later these are used to raycast downward and see if we are on the ground
        floorCheckers = new Transform[floorChecks.childCount];
        for (int i = 0; i < floorCheckers.Length; i++)
            floorCheckers[i] = floorChecks.GetChild(i);
    }
    
	void LockCursor(bool value)
	{
		var mouse = Mouse.current;
		if (mouse != null)
			mouse.cursor.isLocked = value;
	}

    //move rigidbody to a target and return the bool "have we arrived?"
    public bool MoveTo(Vector3 destination, float acceleration, float stopDistance, bool ignoreY)
    {
        Vector3 relativePos = (destination - transform.position);
        if (ignoreY)
            relativePos.y = 0;

        //Debug.DrawRay(destination, relativePos, Color.red);

        DistanceToTarget = relativePos.magnitude;
        if (DistanceToTarget <= stopDistance)
            return true;
        else
            rigid.AddForce(relativePos.normalized * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        return false;
    }

    //rotates rigidbody to a specific direction
    public void RotateToDirection(Vector3 lookDir, float turnSpeed)
    {
        Vector3 characterPos = transform.position;

        characterPos.y = 0;
        lookDir.y = 0;

        //Debug.DrawRay(m_Rigid.position, m_Rigid.velocity, Color.blue);
        //Debug.DrawRay(transform.position, rigid.velocity, Color.green);

        Vector3 newDir = lookDir - characterPos;
        Quaternion dirQ = Quaternion.LookRotation(newDir);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, turnSpeed * Time.deltaTime);

        rigid.MoveRotation(slerp);

        //Debug.DrawRay(rigid.position, dirQ, Color.blue);
    }

    // apply friction to rigidbody, and make sure it doesn't exceed its max speed
    public void ManageSpeed(float deceleration, float maxSpeed, bool ignoreY)
    {
        currentSpeed = rigid.velocity;
        if (ignoreY)
            currentSpeed.y = 0;

        if (currentSpeed.magnitude > 0)
        {
            rigid.AddForce((currentSpeed * -1) * deceleration * Time.deltaTime, ForceMode.VelocityChange);
            if (rigid.velocity.magnitude > maxSpeed)
                rigid.AddForce((currentSpeed * -1) * deceleration * Time.deltaTime, ForceMode.VelocityChange);
        }
    }


    //get state of player, values and input
    void Update()
    {
        // hack
        if (m_MapInput == null)
        {
            return;
        }

        //set animation values
        if (animator)
        {
            //animator.SetFloat("DistanceToTarget", characterMotor.DistanceToTarget);
            float speed = Mathf.Round(rigid.velocity.magnitude);
            //Debug.Log(grounded);
            
            animator.SetBool("Grounded", grounded);
            animator.SetFloat("YVelocity", speed);

            //Debug.Log(animator.GetFloat("YVelocity"));
        }

        //stops rigidbody "sleeping" if we don't move, which would stop collision detection
        rigid.WakeUp();
        //handle jumping
        JumpCalculations();
        //adjust movement values if we're in the air or on the ground
        curAccel = (grounded) ? accel : airAccel;
        curDecel = (grounded) ? decel : airDecel;
        curRotateSpeed = (grounded) ? rotateSpeed : airRotateSpeed;

        var move = m_MapInput.move.vector2;
        //get movement input, set direction to move in
        float v = move.y;
        float h = move.x;

        direction = playerInput.cameraHandle.transform.TransformDirection(new Vector3(h, 0, v)) ; 
        moveDirection = transform.position + direction; // new Vector3(velocity.x, m_Rigid.velocity.y, velocity.z);

        if (m_MapInput.menu.wasJustPressed) {
            FindObjectOfType<ScreenManager>().IngameMenu();
        }        
    }

    //apply correct player movement (fixedUpdate for physics calculations)
    void FixedUpdate()
    {
        //are we grounded
        grounded = IsGrounded();
        //move, rotate, manage speed
        MoveTo(moveDirection, curAccel, 0.7f, true);
        if (rotateSpeed != 0 && direction.magnitude != 0)
        {
            RotateToDirection(moveDirection, curRotateSpeed * 10);
        }

        ManageSpeed(curDecel, maxSpeed + movingObjSpeed.magnitude, true);
    }

    //prevents rigidbody from sliding down slight slopes (read notes in characterMotor class for more info on friction)
    void OnCollisionStay(Collision other)
    {
        //only stop movement on slight slopes if we aren't being touched by anything else
        if (other.collider.tag != "Untagged" || grounded == false)
            return;
        //if no movement should be happening, stop player moving in Z/X axis
        if (direction.magnitude == 0 && slope < slopeLimit && rigid.velocity.magnitude < 2)
        {
            //it's usually not a good idea to alter a rigidbodies velocity every frame
            //but this is the cleanest way i could think of, and we have a lot of checks beforehand, so it should be ok
            rigid.velocity = Vector3.zero;
        }
    }

    //returns whether we are on the ground or not
    //also: bouncing on enemies, keeping player on moving platforms and slope checking

    private bool IsGrounded()
    {
        //get distance to ground, from centre of collider (where floorcheckers should be)
        float dist = GetComponent<Collider>().bounds.extents.y;
        //check whats at players feet, at each floorcheckers position
        foreach (Transform check in floorCheckers)
        {
            RaycastHit hit;

            if (Physics.Raycast(check.position, Vector3.down, out hit, dist + 0.05f))
            {
                if (hit.transform.GetComponent<Collider>().isTrigger == false)
                {
                    //slope control
                    slope = Vector3.Angle(hit.normal, Vector3.up);
                    //slide down slopes
                    if (slope > slopeLimit && hit.transform.tag != "Pushable")
                    {
                        Vector3 slide = new Vector3(0f, -slideAmount, 0f);
                        rigid.AddForce(slide, ForceMode.Force);
                    }
                    //enemy bouncing
                    if (hit.transform.tag == "Enemy" && rigid.velocity.y < 0)
                    {
                        //enemyAI = hit.transform.GetComponent<EnemyAI>();
                        //enemyAI.BouncedOn();
                        //onEnemyBounce++;
                        //dealDamage.Attack(hit.transform.gameObject, 1, 0f, 0f);
                        
                    }
                    else
                        onEnemyBounce = 0;
                    //moving platforms
                    if (hit.transform.tag == "MovingPlatform" || hit.transform.tag == "Pushable")
                    {
                        movingObjSpeed = hit.transform.GetComponent<Rigidbody>().velocity;
                        movingObjSpeed.y = 0f;
                        //9.5f is a magic number, if youre not moving properly on platforms, experiment with this number
                        rigid.AddForce(movingObjSpeed * movingPlatformFriction * Time.fixedDeltaTime, ForceMode.VelocityChange);
                    }
                    else
                    {
                        movingObjSpeed = Vector3.zero;
                    }
                    //yes our feet are on something
                    return true;
                }
            }
        }
        movingObjSpeed = Vector3.zero;
        //no none of the floorchecks hit anything, we must be in the air (or water)
        return false;
    }

    //jumping
    private void JumpCalculations()
    {
        //if were on ground within slope limit
        if (grounded && slope < slopeLimit)
        {
            //and we press jump, or we pressed jump justt before hitting the ground
            if (m_MapInput.jump.isHeld) { //|| airPressTime + jumpLeniancy > Time.time) {
                Jump(jumpForce);
            }
        }
    }

    //push player at jump force
    public void Jump(Vector3 jumpVelocity)
    {
        if (GetComponent<Pickup>().heldObject != null)
        {
            return;
        } else
        {
            rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
            rigid.AddRelativeForce(jumpVelocity, ForceMode.Impulse);

            grounded = false;
        }
    }
}
