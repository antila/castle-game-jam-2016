using UnityEngine;
using System.Collections;

public class SpawnDestroyer : MonoBehaviour {

    public void DestroyItem(Collider other)
    {

        Debug.Log("OnCollisionEnter");
        /*if (collision.gameObject.tag == "Pickup")
        {
            Debug.Log("Destroy");
            Destroy(collision.gameObject);
        }*/
        
    }

    public void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.layer == 8) // Spawned
        {
           Debug.Log("Destroy");
           Destroy(other.gameObject);
       }
    }
}
