using UnityEngine;

//simple class to add to checkpoint triggers
[RequireComponent(typeof(AudioSource))]
public class Checkpoints : MonoBehaviour
{
    public Color activeColor = Color.green; //color when checkpoint is activated
    public float activeColorOpacity = 0.4f; //opacity when checkpoint is activated

    //private GameObject[] checkpoints;
    //private AudioSource aSource;

    //setup
    void Awake()
    {
        //aSource = GetComponent<AudioSource>();
        if (tag != "Respawn")
        {
            tag = "Respawn";
        }
        GetComponent<Collider>().isTrigger = true;
    }

    //more setup
    void Start()
    {
        //checkpoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    //set checkpoint
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<CharacterInputController>().lastCheckpoint = transform;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}