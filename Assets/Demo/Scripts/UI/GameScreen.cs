using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour {

    public Button selectedGame;

	void OnEnable() {
        selectedGame.Select();	    
	}

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
