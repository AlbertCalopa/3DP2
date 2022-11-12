using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private bool stayTrigger = false;

    [SerializeField] private float CanAnim = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player" || other.tag == "CompanionCube")
        {
            if (CanAnim == 0)
            {
                myDoor.Play("DoorAnim", 0, 0.0f);
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
                myDoor.Play("DoorCloseAnim", 0, 0.0f);
            }

            CanAnim -= 1;
        }     
    }



}
