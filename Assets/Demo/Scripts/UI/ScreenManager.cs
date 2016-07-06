using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

    public UIScreen previousScreen;
    public UIScreen activeScreen;

    public void ChangeScreen(UIScreen nextScreen)
    {
        previousScreen = activeScreen;
        activeScreen = nextScreen;

        previousScreen.Disable();
        activeScreen.gameObject.SetActive(true);
        activeScreen.Enable();
    }
}
