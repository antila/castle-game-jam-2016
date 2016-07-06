using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputNew;
using UnityEngine.Serialization;

public class GlobalControl : MonoBehaviour {
    public static GlobalControl Instance;

    MenuActions m_Actions;

    public MultiplayerManager globalMultiplayerManager;
    public ScreenManager screenManager;
    public WinScreen winScreen;
    public PlayerInput[] playerInputs;
    public MultiplayerManager.PlayerInfo[] players;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RoundFinished() {
        var players = globalMultiplayerManager.players;
        for (int i = 0; i < players.Count; i++) {
            if (players[i].winner) { 
                winScreen.ShowWinner(players[i]);
                break;
            }
        }

        playerInputs = FindObjectsOfType<PlayerInput>();
        for (int i = 0; i < playerInputs.Length; i++) {
            playerInputs[i].GetActions<MenuActions>().active = true;
        }

        screenManager.ChangeScreen(winScreen.GetComponent<UIScreen>());

    }
}