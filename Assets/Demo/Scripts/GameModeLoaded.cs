using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameModeLoaded : MonoBehaviour {

    public GameObject quickStart;
    public GameObject content;
    public List<Transform> spawnPositions = new List<Transform>();
    public GlobalControl globalController;
    public float gameTime = 0;
    bool isRunning = false;

    // Use this for initialization
    void Awake () {
       globalController = (GlobalControl)FindObjectOfType(typeof(GlobalControl));
        if (gameTime > 0) {
            isRunning = true;
        }
        if (globalController) {
            quickStart.SetActive(false);
            globalController.GetComponentInChildren<MultiplayerManager>().spawnPositions = spawnPositions;
            globalController.globalMultiplayerManager.StartGame();
        } else {
            FindObjectOfType<MultiplayerManager>().spawnPositions = spawnPositions;
        }
	}

    public void RoundEnd() {
        if (globalController) {
            globalController.globalMultiplayerManager.players[0].winner = true;
            globalController.RoundFinished();
            globalController.globalMultiplayerManager.gameObject.SetActive(true);
        } else {
            Debug.Log("Round Ended");
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (isRunning) {
            gameTime -= Time.deltaTime;
            if (gameTime < 0) {
                isRunning = false;
                RoundEnd();
            }
        }
	}
}