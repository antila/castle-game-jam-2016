using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Mover : MonoBehaviour
{

    public Vector3 startPosition;
    public Vector3 endPosition;

    public List<Vector3> midPositions;

    public Ease m_easing = Ease.InOutQuint;
    public float transitionTime = 2f;

    void Reset()
    {
        startPosition = transform.localPosition;
        endPosition = transform.localPosition;
    }

    // Use this for initialization
    public void Move(bool forward)
    {
        if (forward) {
            transform.DOMove(endPosition, transitionTime).SetEase(m_easing);
        } else {
            transform.DOMove(startPosition, transitionTime).SetEase(m_easing);
        }
    }
}
