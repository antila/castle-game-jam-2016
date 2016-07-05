using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class FollowCamera : MonoBehaviour {

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
            var target = targetPlayer.gameObject;

            Vector3 velocity = Vector3.zero;
            Vector3 forward = target.transform.forward * 5.0f;
            Vector3 up = -target.transform.up * 2.0f;
            Vector3 needPos = target.transform.position - forward - up;
            transform.position = Vector3.SmoothDamp(transform.position, needPos,
                                                    ref velocity, 0.05f);

            //Look at and dampen the rotation
            //Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position);
            //_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
        }
    }
}
