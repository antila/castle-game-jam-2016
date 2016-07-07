using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {

    Collider triggerCollider;
    public Color triggerColor = new Color(0, 0, 1);
    public string triggerTag = "Player";
    public UnityEvent triggerEnter;
    public UnityEvent triggerExit;
    public UnityEvent triggerStay;

    int count = 0;

    public void Test() { Debug.Log("Debug"); }

    void OnTriggerEnter(Collider other) {
        if (other.tag == triggerTag) {
            triggerEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == triggerTag) {
            triggerExit.Invoke();
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == triggerTag) {
            triggerStay.Invoke();
        }
    }

    void Start () {
        triggerCollider = gameObject.GetComponent<Collider>();

	}

    void OnDrawGizmos() {
        var tempColor = triggerColor;

        tempColor.a = 0.25f;
        Gizmos.color = tempColor;
        Gizmos.DrawWireCube(transform.localPosition, transform.localScale);
        tempColor.a = 0.15f;
        Gizmos.color = tempColor;
        Gizmos.DrawCube(transform.localPosition, transform.localScale);
    }
}
