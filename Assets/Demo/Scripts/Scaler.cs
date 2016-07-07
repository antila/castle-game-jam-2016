using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Scaler : MonoBehaviour {

    public Vector3 startScale;
    public Vector3 endScale;
    public Ease scaleEasing = Ease.InOutQuint;
    public float scaleTime = 2f;


    void Reset() {
        startScale = transform.localScale;
        endScale = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    public void Scale (bool forward) {
        if (forward) {
            transform.DOScale(endScale, scaleTime).SetEase(scaleEasing);
        } else {
            transform.DOScale(startScale, scaleTime).SetEase(scaleEasing);
        }
    }
	
}
