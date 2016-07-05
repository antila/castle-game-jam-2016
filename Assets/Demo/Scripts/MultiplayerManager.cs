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

	[Space]

	public ButtonAction joinAction;
	public ButtonAction leaveAction;

	[Space]

	public List<Transform> spawnPositions = new List<Transform>();

	[Space]

	public StartGameEvent onStartGame;
	
	public class PlayerInfo
	{
		public PlayerHandle playerHandle;

        public bool ready = false;
		public ButtonInputControl joinControl;
		public ButtonInputControl leaveControl;

		public PlayerInfo(PlayerHandle playerHandle, ButtonAction joinAction, ButtonAction leaveAction)
		{
			this.playerHandle = playerHandle;
			joinControl = playerHandle.GetActions(joinAction.action.actionMap)[joinAction.action.actionIndex] as ButtonInputControl;
			leaveControl = playerHandle.GetActions(leaveAction.action.actionMap)[leaveAction.action.actionIndex] as ButtonInputControl;
		}
	}

    public List<PlayerInfo> players = new List<PlayerInfo>();

    PlayerHandle globalHandle;
    ConnectScreen connectScreen = null;

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

		joinAction.Bind(globalHandle);
		leaveAction.Bind(globalHandle);

    }

    public void Update()
	{
        connectScreen = (ConnectScreen)FindObjectOfType(typeof(ConnectScreen));
        // Listen to if the join button was pressed on a yet unassigned device.
        if ((joinAction.control.wasJustPressed && !isStartScene) || (joinAction.control.wasJustPressed && isStartScene && connectScreen && connectScreen.isActiveAndEnabled))
		{
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

			players.Add(new PlayerInfo(handle, joinAction, leaveAction));
		}

		int readyCount = 0;
		for (int i = players.Count - 1; i >= 0; i--)
		{
			var player = players[i];
			if (!player.ready)
			{
				if (player.joinControl.wasJustPressed)
					player.ready = true;
				if (player.leaveControl.wasJustPressed)
				{
					player.playerHandle.Destroy();
					players.Remove(player);
					continue;
				}
			}
			else
			{
				if (player.joinControl.wasJustPressed || player.leaveControl.wasJustPressed)
					player.ready = false;
			}
			if (player.ready)
				readyCount++;
		}

        if (isStartScene && connectScreen && readyCount >= 1 && (players.Count - readyCount) == 0) {
            connectScreen.ShowGameScreen();
        } else if (!isStartScene && readyCount >= 1 && (players.Count - readyCount) == 0) {
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
			    if (!player.ready)
			    {
				    GUILayout.Label(string.Format("Press {0} when ready\n\n(Press {1} to leave)",
					    player.joinControl.GetPrimarySourceName(),
					    player.leaveControl.GetPrimarySourceName()));
			    }
			    else
			    {
				    GUILayout.Label(string.Format("READY\n\n(Press {0} to cancel)",
					    player.leaveControl.GetPrimarySourceName()));
			    }
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
				map.active = actionMapSlot.active;
			}

			Transform spawnTransform = spawnPositions[i % spawnPositions.Count];
			var player = (PlayerInput)Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
			player.handle = playerInfo.playerHandle;

            var camera = (GameObject)Instantiate(cameraPrefab, spawnTransform.position, spawnTransform.rotation);
            camera.GetComponent<LookAtCamera>().targetPlayer = player;
            camera.GetComponent<FollowCamera>().targetPlayer = player;
        }
		
		if (onStartGame != null)
			onStartGame.Invoke();
		
		gameObject.SetActive(false);
	}
}
