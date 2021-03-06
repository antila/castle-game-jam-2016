﻿using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject item;                // The enemy prefab to be spawned.
    public float spawnTime = 1f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public Vector3 spawnForce = new Vector3(0, 0, 20);
    public Vector3 spawnRandomJitter = new Vector3(6, 0, 10);

    public bool addRandom90Rotation = false;
    Quaternion nextRotation;

    public bool isActive = true;

    //maximum allowed number of objects - set in the editor
    public int maxObjects = 15;

    //number of objects currently spawned
    public int spawnCount = 0;

    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        nextRotation = transform.rotation;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        if (item != null && isActive && spawnCount < maxObjects)
        {
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            if (addRandom90Rotation) {
                nextRotation = Quaternion.Euler(Random.Range(0,4) * 90f, Random.Range(0, 4) * 90f, Random.Range(0, 4) * 90f);
            }
            GameObject spawn = (GameObject)Instantiate(item, transform.position, nextRotation);
            spawn.layer = 8; // Spawned
            var force = spawnForce + (spawnRandomJitter * Random.Range(0f, 1f));
            spawn.transform.parent = transform;

            Rigidbody r = spawn.GetComponent<Rigidbody>();
            r.AddRelativeForce(force, ForceMode.VelocityChange);

            spawn.AddComponent<SpawnedObject>();
            spawn.GetComponent<SpawnedObject>().homeSpawn = this;
            spawnCount++;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}