using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TagDestroyer : MonoBehaviour
{

    public string tagToDestroy = "Player";
    public int minAliveAmount = 0;
    public UnityEvent callback;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagToDestroy) {
            if (GameObject.FindGameObjectsWithTag(tagToDestroy).Length > minAliveAmount)
                Destroy (other.gameObject);
            StartCoroutine(CallBack());
        }
    }

    IEnumerator CallBack() {
        yield return null;
        callback.Invoke();
    }
}
