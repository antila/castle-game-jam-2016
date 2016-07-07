using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {

    Collider triggerCollider;
    public Color triggerColor = new Color(0, 0, 1);
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerLeave;
    public UnityEvent onTriggerStay;
    int count = 0;

    void OnTriggerEnter(Collider other) {
        Debug.Log("OnEnter");
    }

    void OnTriggerLeave(Collider other)
    {
        Debug.Log("OnLeave " + count);
    }

    void OnTriggerStay(Collider other)
    {
        count++;
    }

    // Use this for initialization
    void Start () {
        triggerCollider = gameObject.GetComponent<Collider>();

	}

    // Update is called once per frame
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
