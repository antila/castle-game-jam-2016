using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour {

    public Button selectedGame;
    private AudioSource aSource;
    public AudioClip levelMusic;

	void OnEnable() {
        selectedGame.Select();	    
	}

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Invoke("PlayLevelMusic", 1);

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
