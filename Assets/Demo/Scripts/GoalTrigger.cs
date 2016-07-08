using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GoalTrigger : MonoBehaviour
{
    public string tagToReachGoal = "Player";
    [HideInInspector]
    public float[] times = new float[4];
    float timer;
    [HideInInspector]
    public List<GameObject> placed;
    int reachedGoalCount = 0;

    HUD hud;

    public void Start() {
        timer = 0;
        hud = FindObjectOfType<HUD>();
        InvokeRepeating("UpdateTimerText", 0.1f, 0.1f);
    }

    void UpdateTimerText() {
        hud.topLeftText.text = "RUN TIME: " + Mathf.Floor(timer);
    }

    void Update() {
        timer += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == tagToReachGoal) {
            var players = GameObject.FindGameObjectsWithTag("Player");
 
            placed.Add(other.gameObject);
            times[reachedGoalCount] = timer;
            reachedGoalCount++;

            if (reachedGoalCount == players.Length) {
                var gameMode = FindObjectOfType<GameModeLoaded>();
                gameMode.winner = placed[0];
                gameMode.gameOver = true;
            }
        }
    }

}
