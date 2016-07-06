using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class FollowCamera : MonoBehaviour
{

    // The target we are following
    public PlayerInput targetPlayer;
    // The distance in the x-z plane to the target
    public float distance = 4;
    // the height we want the camera to be above the target
    public float height = 6;
    // How much we 
    public float heightDamping = 2;
    public float rotationDamping = 16f;

    FirstPersonControls m_MapInput;

    void Start()
    {
        m_MapInput = targetPlayer.GetComponent<CharacterInputController>().m_MapInput;
        
    }

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!targetPlayer)
            return;

        if (m_MapInput == null)
        {
            return;
        }

        Transform target = targetPlayer.gameObject.transform;

        /*
        transform.position = target.position;
        transform.position += Vector3.forward * 5;
        transform.position += Vector3.up * 3;
        */


        //if (Vector3.Distance(target.position, transform.position) > 10f)
        //{
        // Calculate the current rotation angles
        var wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        //Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target

        Vector3 lookAt = new Vector3(target.position.x, currentHeight, target.position.z);

        transform.RotateAround(lookAt, Vector3.up, m_MapInput.look.vector2.x * 200 * Time.deltaTime);

        if (Vector3.Distance(target.position, transform.position) < 10f)
        {
            lookAt.x = transform.position.x;
            lookAt.z = transform.position.z;
        }

        float speed  = 7f;
        var step = speed * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, lookAt, step); // target.position;

        //transform.position -= currentRotation * Vector3.forward * distance;

        // Move our position a step closer to the target.
        //transform.position = 

        // Set the height of the camera
        //transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        // Always look at the target
        //transform.LookAt(transform.position + transform.forward * 10);
        //}

    }
    
}