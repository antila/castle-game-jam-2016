using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputNew;

public class RespawnPlayer : MonoBehaviour {
    public AudioClip respawnSound;
    private AudioSource aSource;

    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            var lastCheckpoint = other.GetComponent<CharacterInputController>().lastCheckpoint;

            if (lastCheckpoint == null)
            {
                Debug.Log(FindObjectOfType<GameModeLoaded>());
                List<Transform> spawnPositions = FindObjectOfType<GameModeLoaded>().spawnPositions;
                lastCheckpoint = spawnPositions[0];
            }

            other.gameObject.transform.position = lastCheckpoint.position;
            
            // Reset camera after position
            other.gameObject.GetComponent<PlayerInput>().cameraHandle.GetComponent<FollowCamera>().ResetPosition();

            if (respawnSound)
            {
                aSource.volume = 1;
                aSource.clip = respawnSound;
                aSource.Play();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
