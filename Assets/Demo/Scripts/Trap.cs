using UnityEngine;
using System.Collections;
using DG.Tweening;


public class Trap : MonoBehaviour {

    public enum TrapState { Closed, Shaking, Open, Dropped};

    public TrapState trapState = TrapState.Closed;

    public Rigidbody leftDoor;
    public Rigidbody rightDoor;

    public float weight = 1;
    TrapManager trapManager;
    float shakeTime;
    float dropTime;

    // Use this for initialization
    void Start () {

        trapManager = FindObjectOfType<TrapManager>();

        switch (trapState)
        {
            case TrapState.Shaking:
                Shake();
                return;
            case TrapState.Open:
                Open();
                return;
        }
    }

    public void Shake() {
        StartCoroutine(Shaker());

    }

    IEnumerator Shaker()
    {
        shakeTime = Random.Range(trapManager.minShakeTime, trapManager.maxShakeTime + 1);
        trapState = TrapState.Shaking;
        transform.DOShakePosition(shakeTime, 0.15f, 15, 45, false, false);
        yield return new WaitForSeconds(shakeTime);
        Open();
    }

    public void Open () {
        trapState = TrapState.Open;
        transform.DOKill();
        leftDoor.isKinematic = false;
        rightDoor.isKinematic = false;
        Drop();
    }

    public void Drop() {
        StartCoroutine(Dropper());

    }

    IEnumerator Dropper()
    {
        dropTime = Random.Range(trapManager.minDropTime, trapManager.maxDropTime + 1);
        trapState = TrapState.Dropped;
        yield return new WaitForSeconds(dropTime);
        Destroy(leftDoor.GetComponent<HingeJoint>());
        Destroy(rightDoor.GetComponent<HingeJoint>());
    }


}
