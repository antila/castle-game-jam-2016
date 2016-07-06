using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    // Use this for initialization
    void OnEnable() {
        GetComponentInChildren<Button>().Select();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ShowWinner(MultiplayerManager.PlayerInfo winner) {
        Debug.Log(winner.playerHandle.index);
    }
}
