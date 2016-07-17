using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour {

    GlobalControl globalController;
    bool allowLevelLoad = true;

    void OnEnable()
    {
        allowLevelLoad = true;
    }

    // Use this for initialization
    public void ReloadActiveScene () {
        if (allowLevelLoad)
        {
            allowLevelLoad = false;


            Invoke("LoadScene", 0.33f);
        }
    }

    private void LoadScene()
    {
        globalController = (GlobalControl)FindObjectOfType(typeof(GlobalControl));
        globalController.globalMultiplayerManager.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
