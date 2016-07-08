using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputNew;
using UnityEngine.Serialization;

public class MultiplayerManager : MonoBehaviour
{
    [Serializable]
    public class StartGameEvent : UnityEvent { }

    public PlayerInput playerPrefab;
    public GameObject cameraPrefab;
    public bool isStartScene = false;

    public List<GameObject> playerPrefabs = new List<GameObject>();

    [Space]

    public ButtonAction startAction;
    public ButtonAction joinAction;
    public ButtonAction leaveAction;

    [Space]

    public AudioSource clickSound;

    [HideInInspector]
	public List<Transform> spawnPositions = new List<Transform>();

	[Space]

	public StartGameEvent onStartGame;

    public class PlayerInfo
	{
		public PlayerHandle playerHandle;

        public Color color = new Color(1f,1f,1f);

        public ButtonInputControl startControl;
        public ButtonInputControl joinControl;
		public ButtonInputControl leaveControl;

		public PlayerInfo(PlayerHandle playerHandle, ButtonAction joinAction, ButtonAction leaveAction, ButtonAction startAction)
		{
			this.playerHandle = playerHandle;
            startControl = playerHandle.GetActions(startAction.action.actionMap)[startAction.action.actionIndex] as ButtonInputControl;
            joinControl = playerHandle.GetActions(joinAction.action.actionMap)[joinAction.action.actionIndex] as ButtonInputControl;
			leaveControl = playerHandle.GetActions(leaveAction.action.actionMap)[leaveAction.action.actionIndex] as ButtonInputControl;
		}
	}

    public List<PlayerInfo> players = new List<PlayerInfo>();

    PlayerHandle globalHandle;
    ConnectScreen connectScreen = null;
    bool gameStarted = false;

    public void Start()
	{
		// Create a global player handle that listen to all relevant devices not already used
		// by other player handles.
		globalHandle = PlayerHandleManager.GetNewPlayerHandle();
		globalHandle.global = true;
		List<ActionMapSlot> actionMaps = playerPrefab.GetComponent<PlayerInput>().actionMaps;
		foreach (ActionMapSlot actionMapSlot in actionMaps)
		{
			ActionMapInput actionMapInput = ActionMapInput.Create(actionMapSlot.actionMap);
			actionMapInput.TryInitializeWithDevices(globalHandle.GetApplicableDevices());
			actionMapInput.active = actionMapSlot.active;
			actionMapInput.blockSubsequent = actionMapSlot.blockSubsequent;
			globalHandle.maps.Add(actionMapInput);
		}

        startAction.Bind(globalHandle);
        joinAction.Bind(globalHandle);
		leaveAction.Bind(globalHandle);

        playerPrefabs.Add((GameObject)Resources.Load("Green_Wizard"));
        playerPrefabs.Add((GameObject)Resources.Load("Red_Wizard"));
        playerPrefabs.Add((GameObject)Resources.Load("Yellow_Wizard"));
        playerPrefabs.Add((GameObject)Resources.Load("Blue_Wizard"));
    }

    public void Update()
	{
        if (globalHandle.GetActions<MenuActions>().move.vector2.x != 0)
        {
            Screen.lockCursor = true;
        }

        connectScreen = (ConnectScreen)FindObjectOfType(typeof(ConnectScreen));
        // Listen to if the join button was pressed on a yet unassigned device.
        if ((joinAction.control.wasJustPressed && !isStartScene) || (joinAction.control.wasJustPressed && isStartScene && connectScreen && connectScreen.isActiveAndEnabled))
		{
            if (clickSound)
                clickSound.Play();

            // These are the devices currently active in the global player handle.
            List<InputDevice> devices = globalHandle.GetActions(joinAction.action.actionMap).GetCurrentlyUsedDevices();
			PlayerHandle handle = PlayerHandleManager.GetNewPlayerHandle();
			foreach (var device in devices)
            {
                handle.AssignDevice(device, true);
            }
				
			
			foreach (ActionMapSlot actionMapSlot in playerPrefab.actionMaps)
			{
				ActionMapInput map = ActionMapInput.Create(actionMapSlot.actionMap);
				map.TryInitializeWithDevices(handle.GetApplicableDevices());
				map.blockSubsequent = actionMapSlot.blockSubsequent;
				
				// Activate the ActionMap that is used to join,
				// disregard active state from ActionMapSlots for now (wait until instantiating player).
				if (map.actionMap == joinAction.action.actionMap)
					map.active = true;
				
				handle.maps.Add(map);
			}
            
			players.Add(new PlayerInfo(handle, joinAction, leaveAction, startAction));

            if (connectScreen) {
                players[players.Count - 1].color = connectScreen.statusImages[players.Count - 1].color;
            }
        }

        bool startWasPressed = false;
		for (int i = players.Count - 1; i >= 0; i--) {
			var player = players[i];
			if (player.leaveControl.wasJustPressed) {
                if (clickSound)
                    clickSound.Play();
                player.playerHandle.Destroy();
				players.Remove(player);
				continue;
			}

            if (player.startControl.wasJustPressed) {
                startWasPressed = true;
                if (clickSound)
                    clickSound.Play();
            }
                
        }

        if (!gameStarted && isStartScene && connectScreen && startWasPressed && players.Count > 1) {
            gameStarted = true;
            connectScreen.ShowGameScreen();
        } else if (!isStartScene && startWasPressed && players.Count > 1) {
            StartGame();
        }
	}
	
	public void OnGUI()
	{
        if (!isStartScene) {
		    float width = 200;
		    float height = 300;
		    int playerNum = 0;
		    for (int i = 0; i < players.Count; i++)
		    {
			    PlayerInfo player = players[i];
			
			    Rect rect = new Rect(20 + (width + 20) * playerNum, (Screen.height - height) * 0.5f, width, height);
			    GUILayout.BeginArea(rect, "Player", "box");
			    GUILayout.Space(20);
			
			    GUILayout.BeginVertical();
			    GUILayout.Space(10);
				GUILayout.Label(string.Format("Press {0} when ready\n\n(Press {1} to leave)",
					player.startControl.GetPrimarySourceName(),
					player.leaveControl.GetPrimarySourceName()));
			    GUILayout.EndVertical();
			    GUILayout.EndArea();
			
			    playerNum++;
		    }
        }
    }

    public void StartGame()
	{
		for (int i = 0; i < players.Count; i++)
		{
			PlayerInfo playerInfo = players[i];

			// Activate ActionMaps according to settings in ActionMapSlots.
			foreach (ActionMapSlot actionMapSlot in playerPrefab.actionMaps)
			{
				var map = playerInfo.playerHandle.GetActions(actionMapSlot.actionMap);
                map.active = !actionMapSlot.active;
            }

			Transform spawnTransform = spawnPositions[i % spawnPositions.Count];

            if (spawnTransform == null)
            {
                Debug.LogError("Couldn't find spawn for player " + i);
            }

            if (spawnPositions.Count != 4)
            {
                Debug.LogError("You don't have 4 spawn positions set!");
            }

            var player = (PlayerInput) Instantiate(
                playerPrefab, 
                spawnTransform.position, 
                spawnTransform.rotation
            );

			player.handle = playerInfo.playerHandle;

            var camera = (GameObject)Instantiate(cameraPrefab, spawnTransform.position, spawnTransform.rotation);
            camera.GetComponent<LookAtCamera>().targetPlayer = player;
            //camera.GetComponent<LookatTarget>().target = player;
            camera.GetComponent<FollowCamera>().targetPlayer = player;
            camera.GetComponent<FollowCamera>().ResetPosition();

            camera.transform.position += Vector3.up * 2;
            camera.transform.position += Vector3.forward * 10;

            player.cameraHandle = camera;

            GameObject playerModel = (GameObject)Instantiate(playerPrefabs[i], spawnTransform.position, spawnTransform.rotation);
            playerModel.transform.parent = player.gameObject.transform;
        }
		
		if (onStartGame != null)
			onStartGame.Invoke();
		
		gameObject.SetActive(false);
	}

}

