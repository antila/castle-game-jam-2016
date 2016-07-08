using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
    public Spawner homeSpawn;

    void OnDestroy()
    {
        homeSpawn.spawnCount--;
    }
}