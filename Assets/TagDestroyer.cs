using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TagDestroyer : MonoBehaviour
{

    public string tagToDestroy = "Player";
    public UnityEvent callback;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagToDestroy) // Spawned
        {
            Destroy (other.gameObject);
            StartCoroutine(CallBack());
        }
    }

    IEnumerator CallBack() {
        yield return null;
        callback.Invoke();
    }
}
