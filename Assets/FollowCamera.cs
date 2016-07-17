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

    public bool isMouseControls = false;

    void Start()
    {
        m_MapInput = targetPlayer.GetComponent<CharacterInputController>().m_MapInput;
        if (m_MapInput.controlScheme.name == "KeyboardMouse")
        {
            isMouseControls = true;
        }
    }

    public void ResetPosition()
    {
        Transform target = targetPlayer.gameObject.transform;
        
        transform.position = target.position;
        transform.position += Vector3.up * 2;
        transform.position += Vector3.forward * 15;
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

        var wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        wantedHeight += (m_MapInput.look.vector2.y * 3);

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Vector3 lookAt = new Vector3(target.position.x, currentHeight, target.position.z);

        float lookVector = (m_MapInput.look.vector2.x * 200);
        if (isMouseControls)
        {
            lookVector = (lookVector * 0.2f);
        }

        transform.RotateAround(lookAt, Vector3.up, lookVector * Time.deltaTime);

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < 10f && distance > 6f)
        {
            lookAt.x = transform.position.x;
            lookAt.z = transform.position.z;
        }

        float speed  = 7f;
        var step = speed * Time.deltaTime;

        if (distance < 6f)
        {
            step = 0-step;
        }

        lookAt.y = wantedHeight;
        transform.position = Vector3.MoveTowards(transform.position, lookAt, step);

        

    }
    
}