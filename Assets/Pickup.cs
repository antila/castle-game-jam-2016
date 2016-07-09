using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class Pickup : MonoBehaviour {
    [HideInInspector]
    public FirstPersonControls m_MapInput;
    [HideInInspector]
    public GameObject heldObject;
    private float timeOfPickup;

    public AudioClip pickUpSound;                               //sound when you pickup/grab an object
    public AudioClip throwSound;                                //sound when you throw an object
    public AudioClip dropSound;
    [HideInInspector]
    public GameObject grabBox;                                  //objects inside this trigger box can be picked up by the player (think of this as your reach)
    public Vector3 holdOffset = new Vector3(0, 0.2f, 1.1f);           //position offset from centre of player, to hold the box (used to be "gap" in old version)
    public Vector3 throwForce = new Vector3(0, 3, 80);           //the throw force of the player
    [HideInInspector]
    public float rotateToBlockSpeed = 3;                        //how fast to face the "Pushable" object you're holding/pulling
    [HideInInspector]
    public float checkRadius = 0.5f;                            //how big a radius to check above the players head, to see if anything is in the way of your pickup
    [Range(0.01f, 1f)]                                           //new weight of a carried object, 1 means no change, 0.1 means 10% of its original weight													
    public float weightChange = 0.1f;                           //this is so you can easily carry objects without effecting movement if you wish to
    [Range(10f, 1000f)]
    public float holdingBreakForce = 45, holdingBreakTorque = 45;//force and angularForce needed to break your grip on a "Pushable" object youre holding onto
    [HideInInspector]
    public Animator animator;                                   //object with animation controller on, which you want to animate (usually same as in PlayerMove)
    [HideInInspector]
    public int armsAnimationLayer;                              //index of the animation layer for "arms"

    [HideInInspector]
    private Vector3 holdPos;
    private FixedJoint joint;
    private float timeOfThrow, defRotateSpeed;
    private Color gizmoColor;
    private AudioSource aSource;

    //private PlayerMove playerMove;
    //private CharacterMotor characterMotor;	line rendererd unnecessary for now. (see line 85)
    private RigidbodyInterpolation objectDefInterpolation;

    // Use this for initialization
    void Start () {
        m_MapInput = GetComponent<PlayerInput>().GetActions<FirstPersonControls>();
        animator = GetComponentInChildren<Animator>();
        aSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_MapInput == null)
        {
            return;
        }

        var grabPrimary = m_MapInput.fire.isHeld;
        var grabAlt = m_MapInput.fireAlt.isHeld;
        bool grab = (grabPrimary || grabAlt);

        var jump = m_MapInput.jump.isHeld;

        if (!grab && heldObject && Time.time > timeOfPickup + 0.1f)
        {
            if (heldObject.tag == "Pickup")
            {
                ThrowPickup(false);
            }       
        }

        if (heldObject)
        {
            animator.SetBool("Holding", true);
        } else
        {
            animator.SetBool("Holding", false);
        }

        if (heldObject && jump)
        {
            ThrowPickup(true);
        }
        

        if (heldObject && heldObject.tag == "Pushable")
        {
            if (grab == false)
            {
                DropPushable();
            }

            if (!joint)
            {
                DropPushable();
                print("'Pushable' object dropped because the 'holdingBreakForce' or 'holdingBreakTorque' was exceeded");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        bool grab = false;
        if (m_MapInput != null && m_MapInput.fire != null)
        {
            var grabPrimary = m_MapInput.fire.isHeld;
            var grabAlt = m_MapInput.fireAlt.isHeld;
            grab = (grabPrimary || grabAlt);
        }
        
        //if grab is pressed and an object is inside the players "grabBox" trigger
        if (grab)
        {
            //pickup
            if (other.tag == "Pickup" && heldObject == null && timeOfThrow + 0.2f < Time.time)
                LiftPickup(other);
            //grab
            if (other.tag == "Pushable" && heldObject == null && timeOfThrow + 0.2f < Time.time)
                GrabPushable(other);
        }
    }

    private void GrabPushable(Collider other)
    {
        heldObject = other.gameObject;
        objectDefInterpolation = heldObject.GetComponent<Rigidbody>().interpolation;
        heldObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        AddJoint();
        //no breakForce limit for pushables anymore because Unity 5's new physics system broke this. Perhaps it'll be fixed in future
        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;
        //stop player rotating in direction of movement, so they can face the block theyre pulling
        //playerMove.rotateSpeed = 0;
    }

    private void LiftPickup(Collider other)
    {

        var existingJoint = other.gameObject.GetComponent<FixedJoint>();
        if (existingJoint != null)
        {
            other.gameObject.GetComponent<Rigidbody>().interpolation = objectDefInterpolation;
            Destroy(existingJoint);
        }
  
        //get where to move item once its picked up
        Mesh otherMesh = other.GetComponent<MeshFilter>().mesh;
        holdPos = transform.position + transform.forward * holdOffset.z + transform.right * holdOffset.x + transform.up * holdOffset.y;
        holdPos.y += (GetComponent<Collider>().bounds.extents.y) + (otherMesh.bounds.extents.y);
        //if there is space above our head, pick up item (this uses the defaul CheckSphere layers, you can add a layerMask parameter here if you need to though!)
        //if (!Physics.CheckSphere(holdPos, checkRadius))
        //{
            gizmoColor = Color.green;
            heldObject = other.gameObject;
            objectDefInterpolation = heldObject.GetComponent<Rigidbody>().interpolation;
            heldObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            heldObject.transform.position = holdPos;
            heldObject.transform.rotation = transform.rotation;
            AddJoint();
            //here we adjust the mass of the object, so it can seem heavy, but not effect player movement whilst were holding it
            heldObject.GetComponent<Rigidbody>().mass *= weightChange;
            //make sure we don't immediately throw object after picking it up
            timeOfPickup = Time.time;

            //Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>());
        //}
        //if not print to console (look in scene view for sphere gizmo to see whats stopping the pickup)
        /*else
        {
            gizmoColor = Color.red;
            print("Can't lift object here. If nothing is above the player, perhaps you need to add a layerMask parameter to line 136 of the code in this script," +
                "the CheckSphere function, in order to make sure it isn't detecting something above the players head that is invisible");
        }*/
    }

    private void DropPushable()
    {
        heldObject.GetComponent<Rigidbody>().interpolation = objectDefInterpolation;
        Destroy(joint);
        //playerMove.rotateSpeed = defRotateSpeed;
        heldObject = null;
        timeOfThrow = Time.time;

        if (dropSound)
        {
            aSource.volume = 1;
            aSource.clip = dropSound;
            aSource.Play();
        }
    }

    public void ThrowPickup(bool throwIt)
    {
        if (joint == null)
        {
            // Pickup have been stolen
            DropPushable();
            return;
        }
        
        if (throwSound)
        {
            aSource.volume = 1;
            aSource.clip = throwSound;
            aSource.Play();
        }
        
        Rigidbody r = heldObject.GetComponent<Rigidbody>();
        r.interpolation = objectDefInterpolation;
        r.mass /= weightChange;

        if (throwIt && joint != null)
        {
            r.AddRelativeForce(throwForce, ForceMode.VelocityChange);
        }

        Destroy(joint);

        heldObject = null;
        timeOfThrow = Time.time;
    }

    //connect player and pickup/pushable object via a physics joint
    private void AddJoint()
    {
        if (heldObject)
        {
            if (pickUpSound)
            {
                aSource.volume = 1;
                aSource.clip = pickUpSound;
                aSource.Play();
            }
            joint = heldObject.AddComponent<FixedJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
        }
    }

    //draws red sphere if something is in way of pickup (select player in scene view to see)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(holdPos, checkRadius);
    }
}
