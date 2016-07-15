using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class ScreenManager : MonoBehaviour {

    public UIScreen previousScreen;
    public UIScreen activeScreen;
    public UIScreen ingameMenuScreen;

    PlayerHandle globalHandle;

    private bool HasGamepadMoved()
    {
        //I feel dirty doing this but it works
        return (Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0);
    }

    private bool HasMouseMoved()
    {
        //I feel dirty doing this but it works
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    public void ChangeScreen(UIScreen nextScreen)
    {
        previousScreen = activeScreen;
        activeScreen = nextScreen;

        previousScreen.Disable();
        activeScreen.gameObject.SetActive(true);
        activeScreen.Enable();
    }

    public void IngameMenu() {
        if (!ingameMenuScreen.isActiveAndEnabled) {
            ChangeScreen(ingameMenuScreen);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    void Update() {
        if (HasGamepadMoved())
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        if (HasMouseMoved())
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}
