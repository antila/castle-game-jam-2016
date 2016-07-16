using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour {

    bool allowLevelLoad = true;

	void OnEnable() {
        allowLevelLoad = true;	    
	}

    public void LoadScene(string sceneName)
    {
        if (allowLevelLoad) {
            allowLevelLoad = false;
            SceneManager.LoadScene(sceneName);
       }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
