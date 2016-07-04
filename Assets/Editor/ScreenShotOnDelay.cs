using UnityEngine;
using System.Collections;

public class ScreenshotOnDelay : MonoBehaviour
{

    public void SetTimer(float time, string path)
    {
        StartCoroutine(Capture(time, path));
    }

    IEnumerator Capture(float time, string path)
    {
        yield return new WaitForSeconds(time);

        Application.CaptureScreenshot(path);

        GameObject.Destroy(this.gameObject);
    }
}