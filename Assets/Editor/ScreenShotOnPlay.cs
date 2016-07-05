using UnityEngine;
using UnityEditor;
using System.Collections;

/**
 * Put me in a folder named "Editor".
 * Change SCREENSHOT_PATH to modify where screenshots are saved.
 */
[InitializeOnLoad]
public class ScreenshotOnPlay
{
    const string SCREENSHOT_PATH = "Assets/Screenshots/Screenshot_";
    static bool isPlaying = false;

    static ScreenshotOnPlay()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
    }

    ~ScreenshotOnPlay()
    {
        EditorApplication.playmodeStateChanged -= OnPlaymodeStateChange;
    }

    static void OnPlaymodeStateChange()
    {
        if (EditorApplication.isPlaying)
        {
            isPlaying = !isPlaying;

            if (isPlaying)
            {
                string path = "";
                int i = 0;

                if (EditorPrefs.HasKey("CurrentScreenshot"))
                    i = EditorPrefs.GetInt("CurrentScreenshot");

                path = AssetDatabase.GenerateUniqueAssetPath(SCREENSHOT_PATH + i + ".png");

                EditorPrefs.SetInt("CurrentScreenshot", i);

                // Instantiate a new gameobject that will capture a screenshot then delete itself.
                // This works around a lack of coroutines in the editor.
                GameObject go = new GameObject();
                go.AddComponent<ScreenshotOnDelay>().SetTimer(2f, path);
            }
        }
    }
}