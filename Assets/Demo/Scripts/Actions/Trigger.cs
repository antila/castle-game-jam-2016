using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {

    public Color triggerColor = new Color(0, 0, 1);
    public string triggerTag = "Player";
    public UnityEvent triggerEnter;
    public UnityEvent triggerExit;
    public UnityEvent triggerStay;

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
