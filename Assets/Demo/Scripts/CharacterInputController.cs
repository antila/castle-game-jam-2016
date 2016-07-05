using System;
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
	FirstPersonControls m_MapInput;
	Rigidbody m_Rigid;
	Vector2 m_Rotation = Vector2.zero;

	float m_TimeOfLastShot;

	public PlayerInput playerInput;
	public Transform head;
	public float moveSpeed = 5;
	public GameObject projectile;
	public float timeBetweenShots = 0.5f;

    public float jumpPower = 600;
	private float distToGround;

    public DragRigidbody drag;

    [Space(10)]

	public CubeSizer sizer;
	public Text controlsText;
	public RuntimeRebinding rebinder;

	public void Start()
	{
		m_MapInput = playerInput.GetActions<FirstPersonControls>();

		if (rebinder != null)
			rebinder.Initialize(m_MapInput);

		m_Rigid = GetComponent<Rigidbody>();

        drag = GetComponent<DragRigidbody>();

        LockCursor(true);

		/*if (!playerInput.handle.global) {
			transform.Find("Canvas/Virtual Joystick").gameObject.SetActive(false);
		}*/

		distToGround = GetComponent<Collider>().bounds.extents.y;
	}

	public void Update()
	{
        // hack
        if (m_MapInput == null)
        {
            return;
        }

		// Move
		var move = m_MapInput.move.vector2;

		Vector3 velocity = transform.TransformDirection(new Vector3(move.x, 0, move.y)) * moveSpeed;
		m_Rigid.velocity = new Vector3(velocity.x, m_Rigid.velocity.y, velocity.z);

		// Look
		var look = m_MapInput.look.vector2 * 3;

		m_Rotation.y += look.x;
		m_Rotation.x = Mathf.Clamp(m_Rotation.x - look.y, -89, 89);

		transform.localEulerAngles = new Vector3(0, m_Rotation.y, 0);
		head.localEulerAngles = new Vector3(m_Rotation.x, 0, 0);

		// Fire
		var fire = m_MapInput.fire.isHeld;
		if (fire)
		{
            /*
			var currentTime = Time.time;
			var timeElapsedSinceLastShot = currentTime - m_TimeOfLastShot;
			if (timeElapsedSinceLastShot > timeBetweenShots)
			{
				m_TimeOfLastShot = currentTime;
				Fire();
			}*/
            drag.HandleInputBegin(transform);
        } else {
            drag.HandleInputEnd(transform);
        }

        // Drag
        drag.HandleInput(transform);

        // Jump
        var jump = m_MapInput.jump.isHeld;
		if (jump && IsGrounded())
		{
			Jump();
		}

		if (m_MapInput.lockCursor.wasJustPressed)
			LockCursor(true);

		if (m_MapInput.unlockCursor.wasJustPressed)
			LockCursor(false);

		if (m_MapInput.menu.wasJustPressed)
			sizer.OpenMenu();

		if (rebinder != null)
		{
			if (m_MapInput.reconfigure.wasJustPressed)
				rebinder.enabled = !rebinder.enabled;

			if (rebinder.enabled == m_MapInput.active)
			{
				LockCursor(!rebinder.enabled);
				m_MapInput.active = !rebinder.enabled;
				controlsText.enabled = !rebinder.enabled;
			}
		}

		//HandleControlsText();
		controlsText.enabled = false;
	}

	void HandleControlsText()
	{
		string help = string.Empty;

		help += GetControlHelp(m_MapInput.moveX) + "\n";
		help += GetControlHelp(m_MapInput.moveY) + "\n";
		help += GetControlHelp(m_MapInput.lookX) + "\n";
		help += GetControlHelp(m_MapInput.lookY) + "\n";
		help += GetControlHelp(m_MapInput.fire) + "\n";
		help += GetControlHelp(m_MapInput.menu);
		controlsText.text = help;
	}

	private string GetControlHelp(InputControl control)
	{
		return string.Format("Use {0} to {1}!", control.GetPrimarySourceName(), control.name);
	}

	void Fire()
	{
        //ag.HandleInputBegin(transform);
        /*
		var newProjectile = Instantiate(projectile);
		newProjectile.transform.position = head.position + head.forward * 0.6f;
		newProjectile.transform.rotation = head.rotation;
		float size = (sizer == null ? 1 : sizer.size);
		newProjectile.transform.localScale *= size;
		newProjectile.GetComponent<Rigidbody>().mass = Mathf.Pow(size, 3);
		newProjectile.GetComponent<Rigidbody>().AddForce(head.forward * 20f, ForceMode.Impulse);
		newProjectile.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
        */
    }

    bool IsGrounded() {
  		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void Jump()
	{
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
        //transform.Translate(Vector3.up * jumpPower * Time.deltaTime, Space.World);
	}

	void LockCursor(bool value)
	{
		var mouse = Mouse.current;
		if (mouse != null)
			mouse.cursor.isLocked = value;
	}
}
