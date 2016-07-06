using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectScreen : MonoBehaviour {
    public MultiplayerManager Multiplayer;
    public GameObject NextScreen;

    [Serializable]
    public class StatusImages {
        public Image image;
        public Color color;
        public Sprite inactive;
        public Sprite active;
        public Sprite ready;
    }

    public List<StatusImages> statusImages = new List<StatusImages>();

    public void ShowGameScreen () {
        NextScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void OnGUI () {
        for (int i = 0; i < statusImages.Count; i++) {

            if (i <= Multiplayer.players.Count - 1) {

                var player = Multiplayer.players[i];

                if (player.ready) {
                    statusImages[i].image.sprite = statusImages[i].ready;
                    statusImages[i].image.color = Color.green;
                } else {
                    statusImages[i].image.sprite = statusImages[i].active;
                    statusImages[i].image.color = statusImages[i].color;
                }

            } else {
                statusImages[i].image.sprite = statusImages[i].inactive;
                statusImages[i].image.color = Color.black;
            }
        }
    }
}
