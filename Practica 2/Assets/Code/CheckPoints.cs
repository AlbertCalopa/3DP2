using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public List<float> checkpoints;
    // Start is called before the first frame update
    void Start()
    {
        checkpoints = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(checkpoints.Count);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach(var check in checkpoints)
            {
                
            }
            checkpoints.Add(1);
        }
    }
}
