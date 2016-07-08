using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectScreen : MonoBehaviour {
    public MultiplayerManager Multiplayer;
    public ScreenManager screenManager;
    public UIScreen nextScreen;
    public Text instructions;

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

            if (Multiplayer.players.Count > 1) {
                instructions.text = "Press it once more when all players are ready to play.";
            } else {
                instructions.text = "Press Right Trigger or Left Mouse Button to join.";
            }

            if (i <= Multiplayer.players.Count - 1) {
                Color playerColor = statusImages[i].color;

                var player = Multiplayer.players[i];

                statusImages[i].message.text = "READY!";
                playerColor.a = 1f;
                statusImages[i].image.color = playerColor;

            } else {
                statusImages[i].message.text = "";
                statusImages[i].image.color = new Color(0.5f,0.5f, 0.5f, 1f);
            }
        }
    }
}
