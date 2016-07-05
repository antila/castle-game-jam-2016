using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectScreen : MonoBehaviour {
    public MultiplayerManager Multiplayer;
    public GameObject NextScreen;

    public List<Image> statusImages = new List<Image>();

    // Use this for initialization
    void Start () {
	    
	}
    public void ShowGameScreen () {
        NextScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void OnGUI () {
        for (int i = 0; i < statusImages.Count; i++) {

            if (i <= Multiplayer.players.Count - 1) {

                MultiplayerManager.PlayerInfo player = Multiplayer.players[i];

                if (player.ready) {
                    statusImages[i].color = Color.green;
                } else {
                    statusImages[i].color = Color.yellow;
                }

            } else {
                statusImages[i].color = Color.black;
            }
        }
	}
}
