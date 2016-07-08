using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour {

    public Trap trapPrefab;

    [Range(2, 10)]
    public int gridSize = 5;
    [Range(8.75f, 15f)]
    public float trapSpace = 12f;

    List<Trap> traps;

    public float roundTime = 5;
    float timer = 0;

    public int minTrapsActivated = 3;
    public int maxTrapsActivated = 5;
    int trapActivationCount = 1;

    public float minShakeTime = 3;
    public float maxShakeTime = 10;

    public float minDropTime = 3;
    public float maxDropTime = 10;

    void Awake() {
        var spawnpositions = GetComponent<GameModeLoaded>().spawnPositions;
        spawnpositions[0].position = new Vector3(trapSpace, 20f, trapSpace);
        spawnpositions[1].position = new Vector3(trapSpace * gridSize, 20f, trapSpace * gridSize);
        spawnpositions[2].position = new Vector3(trapSpace, 20f, trapSpace * gridSize);
        spawnpositions[3].position = new Vector3(trapSpace * gridSize, 20f, trapSpace);
        spawnpositions[0].rotation = Quaternion.Euler(0, 45, 0);
        spawnpositions[1].rotation = Quaternion.Euler(0, 225, 0);
        spawnpositions[2].rotation = Quaternion.Euler(0, 135, 0);
        spawnpositions[3].rotation = Quaternion.Euler(0, 315, 0);
    }

    // Use this for initialization
    void Start () {
        var x = 0f;
        var z = 0f;
        var r = 0f;
        for (var i = 0; i < gridSize; i++) {
            z = z + trapSpace;
            for (var n = 0; n < gridSize; n++) {
                x = x + trapSpace;
                r += 90f;
                Instantiate(trapPrefab, new Vector3(x, 15f, z), Quaternion.Euler(0, r, 0));
            }
            x = 0;
        }

        traps = new List<Trap>(FindObjectsOfType<Trap>());
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < roundTime) { 
            timer += Time.deltaTime;
        } else if (traps.Count > 0) {
            timer = 0;
            trapActivationCount = Mathf.Min(Random.Range(minTrapsActivated, maxTrapsActivated + 1), traps.Count);
            traps.Shuffle();
            for (var i = 0; i < trapActivationCount; i++) {
                var trap = traps[i];
                traps.RemoveAt(i);
                if (trap.trapState == Trap.TrapState.Closed) {
                    trap.Shake();
                }
            }
        }
    }

    public void PlayerFell() {
        var gameMode = GetComponent<GameModeLoaded>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 2) {
            gameMode.winner = players[0];
            gameMode.gameOver = true;
        }
    }

}
