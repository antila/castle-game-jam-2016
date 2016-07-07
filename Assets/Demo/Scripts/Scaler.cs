using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

    public Vector3 startScale;
    public Vector3 endScale;

    void Reset() {
        startScale = transform.localScale;
        endScale = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    public void Scale (bool forward) {
        Debug.LogError(forward);
        if (forward) {
            transform.localScale = endScale;
        } else {
            transform.localScale = startScale;
        }
    }
	
}
