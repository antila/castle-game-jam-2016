using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject item;                // The enemy prefab to be spawned.
    public float spawnTime = 1f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    public Vector3 spawnForce = new Vector3(0, 0, 20);
    public Vector3 spawnRandomJitter = new Vector3(6, 0, 10);

    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        if (item != null)
        {
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            GameObject spawn = (GameObject)Instantiate(item, transform.position, transform.rotation);
            spawn.layer = 8; // Spawned
            var force = spawnForce + (spawnRandomJitter * Random.Range(0f, 1f));

            Rigidbody r = spawn.GetComponent<Rigidbody>();
            r.AddRelativeForce(force, ForceMode.VelocityChange);
        }
        
    }
}