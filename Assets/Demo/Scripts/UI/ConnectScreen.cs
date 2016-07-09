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
        public Sprite spriteNotReady;
        public Sprite spriteReady;
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
                instructions.text = string.Format("Press {0} when ready\n\n(Press {1} to leave)",
                                    Multiplayer.startAction.control.GetPrimarySourceName(),
                                    Multiplayer.leaveAction.control.GetPrimarySourceName());
            } else {
                instructions.text = string.Format("Press {0} to join",
                                    Multiplayer.joinAction.control.GetPrimarySourceName());
            }

            if (i <= Multiplayer.players.Count - 1) {
                var player = Multiplayer.players[i];
                statusImages[i].image.sprite = statusImages[i].spriteReady;
            } else {
                statusImages[i].message.text = "";
                statusImages[i].image.sprite = statusImages[i].spriteNotReady;
            }
        }
    }
}
