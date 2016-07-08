using UnityEngine;
using System.Collections;

public class SpawnDestroyer : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.layer == 8) // Spawned
        {
           Destroy(other.gameObject);
       }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
