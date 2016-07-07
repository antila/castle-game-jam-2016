using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rotator : MonoBehaviour
{
    Vector3 originalRotation;
    public Vector3 rotation;
    public Ease m_easing = Ease.InOutQuint;
    public float transitionTime = 2f;

    [Header("LOOPS (Set to -1 for infinite)")]
    public int loops = 1;
    public LoopType loopType = LoopType.Incremental;

    bool rotationExist = false;

    void Reset() {
        originalRotation = transform.localScale;
        rotation = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    public void Pause() {
        transform.DOPause();
    }

    public void Play() {
        if (!rotationExist) {
            Rotate();
        } else {
            transform.DOPlay();
        }
    }
    public void Rotate() {
        rotationExist = true;
        transform.DORotate(rotation, transitionTime, RotateMode.FastBeyond360).SetEase(m_easing).SetLoops(loops, loopType);
    }

    public void RotateToOrigin() {
        transform.DOKill();
        transform.DORotate(originalRotation, transitionTime, RotateMode.Fast).SetEase(m_easing).SetLoops(1, LoopType.Restart);
        rotationExist = false;
    }
}
