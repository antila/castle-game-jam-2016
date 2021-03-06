﻿using UnityEngine;
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
        if (hud)
            hud.topLeftText.text = "RUN TIME: " + Mathf.Floor(timer);
    }

    void Update() {
        timer += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == tagToReachGoal) {
            
            placed.Add(other.gameObject);
            times[reachedGoalCount] = timer;
            reachedGoalCount++;

            var gameMode = FindObjectOfType<GameModeLoaded>();
            gameMode.winner = placed[0];
            gameMode.gameOver = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}
