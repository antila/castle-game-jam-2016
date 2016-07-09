using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour {

    public Button selectedGame;
    private AudioSource aSource;
    public AudioClip levelMusic;
    bool allowLevelLoad = true;
	void OnEnable() {
        selectedGame.Select();
        allowLevelLoad = true;	    
	}

    public void LoadScene(string sceneName)
    {
        if (allowLevelLoad) {
            allowLevelLoad = false;
            SceneManager.LoadScene(sceneName);
            Invoke("PlayLevelMusic", 1);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    void PlayLevelMusic()
    {
        if (levelMusic)
        {
            aSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
            Debug.Log("Play Level music");
            aSource.volume = 1;
            aSource.clip = levelMusic;
            aSource.Play();
        }
    }
}
