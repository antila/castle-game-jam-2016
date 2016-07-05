using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameModeLoaded : MonoBehaviour {

    public GameObject quickStart;
    public List<Transform> spawnPositions = new List<Transform>();


    // Use this for initialization
    void Awake () {
        GlobalControl globalController = (GlobalControl)FindObjectOfType(typeof(GlobalControl));
        if (globalController) {
            quickStart.SetActive(false);
            globalController.GetComponentInChildren<MultiplayerManager>().spawnPositions = spawnPositions;
            globalController.globalMultiplayerManager.StartGame();
        }
	}
    void OnLevelWasLoaded(int level) {
        if (level == SceneManager.GetActiveScene().buildIndex)
            Debug.Log("Woohoo");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
