using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class GameScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
