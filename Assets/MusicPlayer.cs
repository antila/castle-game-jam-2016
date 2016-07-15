using UnityEngine;
using System;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instance;

    private AudioSource aSource;
    public MusicTrack[] musicTracks;
    public string currentTrackId = "None";
    public bool playOnStart = false;

    [Serializable]
    public class MusicTrack {
        public string trackId;
        public AudioClip audioClip;
    }

    void Awake()
    {
        if (instance == null){
            instance = this;
        }
    }

    void Start() {
        aSource = GetComponent<AudioSource>();
        if (!aSource) {
            aSource = gameObject.AddComponent<AudioSource>();
        }
        aSource.loop = true;
        aSource.volume = 1;

        if (playOnStart)
        {
            MusicTrack track = Array.Find(musicTracks, item => item.trackId == currentTrackId);
            if (track != null)
            {
                aSource.clip = track.audioClip;
                aSource.Play();
            }
            else
            {
                Debug.LogError("MusicPlayer couldn't find any track with the ID: " + currentTrackId);
            }
        }
    }

    public void PlayTrack(string trackId)
    {
        Debug.Log("MusicPlayer loading track with the ID: " + trackId);
        if (trackId == "None") {
            // Stop playing
            aSource.Stop();
        } else if (trackId != currentTrackId) {
            // Find Track
            MusicTrack track = Array.Find(musicTracks, item => item.trackId == trackId);
            if (track != null) {
                currentTrackId = track.trackId;
                aSource.clip = track.audioClip;
                aSource.Play();
            } else {
                Debug.LogError("MusicPlayer couldn't find any track with the ID: " + trackId);
            }
        }
    }
}
