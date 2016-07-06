using UnityEngine;
using System.Collections;

public class UIScreen : MonoBehaviour {

    public float waitTime = 0.2f;
    public Vector3 originalScale = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 disabledScale = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 enabledScale = new Vector3(1.0f, 1.0f, 1.0f);

    Vector3 scaleStart;
    Vector3 scaleEnd;

    void Start() {
        foreach (Transform child in transform) {
            child.localScale = originalScale;
        }
    }

    IEnumerator Scale(bool activate)
    {

        float currentTime = 0.0f;

        if (activate) {
            gameObject.SetActive(true);
            scaleStart = disabledScale;
            scaleEnd = enabledScale;
        } else {
            scaleStart = enabledScale;
            scaleEnd = disabledScale;
        }

        do
        {

            foreach (Transform child in transform)
            {
                child.localScale = Vector3.Lerp(scaleStart, scaleEnd, currentTime / waitTime);
            }
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= waitTime);

        if (!activate) {
            gameObject.SetActive(false);
        }
    }
    // Use this for initialization
    public void Enable () {
        StartCoroutine(Scale(true));
    }

    // Update is called once per frame
    public void Disable () {
        StartCoroutine(Scale(false));
    }
}
