using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectScreen : MonoBehaviour {
    public MultiplayerManager Multiplayer;

    public List<Image> statusImages = new List<Image>();

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < statusImages.Count; i++) {

            if(i <= Multiplayer.players.Count - 1) {
                statusImages[i].color = Color.green;
            } else {
                statusImages[i].color = Color.black;
            }
        }
	}
}
