using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    private float CanAnim = 0;

    [SerializeField] public float DistanceDownButton;
    [SerializeField] public GameObject button;
    [SerializeField] public bool thereIsAnim = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player" || other.tag == "CompanionCube")
        {
            if (CanAnim == 0)
            {
                if (thereIsAnim)
                {
                    myDoor.Play("DoorAnim", 0, 0.0f);
                }
                
                button.transform.position += new Vector3(0, DistanceDownButton, 0);
            }
            
            CanAnim += 1;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Player" || other.tag == "CompanionCube")
        {
            if (CanAnim == 1)
            {
                if (thereIsAnim)
                {
                    myDoor.Play("DoorCloseAnim", 0, 0.0f);
                }
                
                button.transform.position += new Vector3(0, -DistanceDownButton, 0);
            }

            CanAnim -= 1;
        }     
    }



}
