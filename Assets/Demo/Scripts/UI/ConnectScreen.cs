using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectScreen : MonoBehaviour {
    public MultiplayerManager Multiplayer;
    public ScreenManager screenManager;
    public UIScreen nextScreen;

    [Serializable]
    public class StatusImages {
        public Image image;
        public Color color;
        public Text message;
    }

    public List<StatusImages> statusImages = new List<StatusImages>();

    public void ShowGameScreen () {
        screenManager.ChangeScreen(nextScreen);
    }
    // Update is called once per frame
    public void OnGUI () {
        for (int i = 0; i < statusImages.Count; i++) {

            if (i <= Multiplayer.players.Count - 1) {
                Color playerColor = statusImages[i].color;

                var player = Multiplayer.players[i];

                if (player.ready) {
                    //statusImages[i].message.text = "READY!";
                    playerColor.a = 1f;
                    statusImages[i].image.color = playerColor;
                } else {
                    //statusImages[i].message.text = "READY?";
                    playerColor = playerColor * 0.8f;
                    playerColor.a = 1f;
                    statusImages[i].image.color = playerColor;
                }

            } else {
                statusImages[i].image.color = new Color(0.5f,0.5f, 0.5f, 1f);
            }
        }
    }
}
