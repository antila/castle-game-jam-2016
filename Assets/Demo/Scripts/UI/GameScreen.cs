using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour {

    bool allowLevelLoad = true;
    string nextLevel;

    void OnEnable() {
        allowLevelLoad = true;	    
	}

    public void LoadScene(string sceneName)
    {
        if (allowLevelLoad) {
            allowLevelLoad = false;
            nextLevel = sceneName;

            Invoke("LoadNextLevel", 0.33f);
       }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
