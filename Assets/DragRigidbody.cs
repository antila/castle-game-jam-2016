using UnityEngine;

/// <summary>
/// Drag a rigidbody with the mouse using a spring joint.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;

    Transform jointTrans;
    float dragDepth;

   /* void OnMouseDown()
    {
        HandleInputBegin(Input.mousePosition);
    }

    void OnMouseUp()
    {
        HandleInputEnd(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        HandleInput(Input.mousePosition);
    }*/

    public void HandleInputBegin(Transform playerTransform)
    {
        if (jointTrans == null)
        {
            Vector3 rayFromPosition = playerTransform.position;
            rayFromPosition = rayFromPosition + (playerTransform.up * 1);

            Debug.DrawRay(rayFromPosition, transform.forward, Color.red);

            //Debug.DrawRay(playerPosition, ray, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(new Ray(playerTransform.position, transform.forward), out hit, 5f))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactive"))
                {
                    Debug.Log("Drag start");
                    //dragDepth = CameraPlane.CameraToPointDepth(Camera.main, hit.point);
                    jointTrans = AttachJoint(hit.rigidbody, hit.point);

                }
            }
        }
    
    }

    public void HandleInput(Transform playerTransform)
    {
        if (jointTrans == null)
        {
            return;
        }

        //var worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        //jointTrans.position = CameraPlane.ScreenToWorldPlanePoint(Camera.main, dragDepth, screenPosition);
        jointTrans.position = playerTransform.position;
        jointTrans.transform.Translate(playerTransform.forward * 2);
        jointTrans.transform.Translate(playerTransform.up * 2);
    }

    public void HandleInputEnd(Transform playerTransform)
    {
        if (jointTrans != null)
        {
            Debug.Log("Drag end");
            Destroy(jointTrans.gameObject);
        }
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        //Debug.Log("hit");
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }
}