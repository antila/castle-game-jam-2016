using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.gameObject.transform.position = other.GetComponent<CharacterInputController>().lastCheckpoint.position;
        }
    }
}
