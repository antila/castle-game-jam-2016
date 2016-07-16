using UnityEngine;
using System.Collections;
using UnityEngine.InputNew;

public class ScreenManager : MonoBehaviour {

    public UIScreen previousScreen;
    public UIScreen activeScreen;

    [Space]

    public UIScreen quitScreen;
    public UIScreen ingameScreen;

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

        if(ingameScreen.isActiveAndEnabled) {
            var mouse = Mouse.current;
            if (mouse != null)
                mouse.cursor.isLocked = true;
        }

    }

    public void IngameMenu() {
        if (!quitScreen.isActiveAndEnabled) {
            ChangeScreen(quitScreen);
        }
    }


    public void Quit() {
        Application.Quit();
    }

    void Update() {
        if (!ingameScreen.isActiveAndEnabled && HasGamepadMoved())
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        if (!ingameScreen.isActiveAndEnabled && HasMouseMoved())
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}
