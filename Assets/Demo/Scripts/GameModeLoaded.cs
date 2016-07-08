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

    public GameObject winner = null;

    public float gameTime = 300;
    bool isRunning = false;

    public bool gameOver = false;

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
            if (winner == null) {
                winner = GameObject.FindGameObjectsWithTag("Player")[0];
            }
            globalController.RoundFinished(winner);
            globalController.globalMultiplayerManager.gameObject.SetActive(true);
        } else {
            if (winner == null)
            {
                winner = GameObject.FindGameObjectsWithTag("Player")[0];
            }
            Debug.Log("Round Ended, playerHandle " + winner + " won.");
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (isRunning) {
            gameTime -= Time.deltaTime;
            if (gameOver || gameTime < 0) {
                Debug.LogError("GAME OVER!!!");
                isRunning = false;
                gameOver = false;
                RoundEnd();
            }
        }
	}
}