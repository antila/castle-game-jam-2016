using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class LookAtCamera : MonoBehaviour {

    //public Transform target;        //an Object to lock on to
    public float damping = 6.0f;    //to control the rotation 
    public bool smooth = true;
    public float minDistance = 10.0f;   //How far the target is from the camera
    public string property = "";

    private Color color;
    private float alpha = 1.0f;
    private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public PlayerInput targetPlayer;

    void LateUpdate()
    {
        if (targetPlayer)
        {
            var target = targetPlayer.gameObject.transform;

            //Look at and dampen the rotation
            Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position + Vector3.up * 2);
            _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
            
        }
    }
}