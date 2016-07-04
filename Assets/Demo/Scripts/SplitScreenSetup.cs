using UnityEngine;
using System.Collections;

public class SplitScreenSetup : MonoBehaviour
{
	public void Setup () {
		Camera[] cameras = FindObjectsOfType<Camera>();

		float fraction = 1f / cameras.Length;
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].rect = new Rect(0, fraction * i, 1, fraction);
		}
	}
}
