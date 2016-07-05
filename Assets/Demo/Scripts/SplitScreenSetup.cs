using UnityEngine;
using System.Collections;

public class SplitScreenSetup : MonoBehaviour
{
	public void Setup () {
		Camera[] cameras = FindObjectsOfType<Camera>();

        switch (4) {
            case 1:
                cameras[0].rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                cameras[0].rect = new Rect(0, 0.5f, 1, 0.5f);
                cameras[1].rect = new Rect(0, 0, 1, 0.5f);
                break;
            case 3:
                cameras[0].rect = new Rect(0, 0.5f, 1, 0.5f);
                cameras[1].rect = new Rect(0, 0, 0.5f, 0.5f);
                cameras[2].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
            case 4:
                cameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                cameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
        }
	}
}
