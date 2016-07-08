using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
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
}
