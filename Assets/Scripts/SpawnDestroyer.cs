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
}
