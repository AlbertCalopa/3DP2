using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public int CheckpointID;
    public Transform CheckpointTransform;
    public FPPlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        
    }
    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CheckpointTransform = this.transform;
            PlayerPrefs.SetInt("CheckPoint", CheckpointID);
            PlayerPrefs.SetFloat("x", transform.position.x);
            PlayerPrefs.SetFloat("y", transform.position.y);
            PlayerPrefs.SetFloat("z", transform.position.z);
            other.gameObject.GetComponent<FPPlayerController>().checkpoints = this;
            Debug.Log(PlayerPrefs.GetInt("CheckPoint"));
        }
    }

    public void Save()
    {
        Vector3 playerPos = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));
        player.transform.position = playerPos;
        Debug.Log(playerPos);
    }
}
