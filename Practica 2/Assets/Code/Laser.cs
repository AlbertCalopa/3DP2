using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer m_Laser;
    public LayerMask m_LaserLayerMask;
    public float m_MaxLaserDistance = 250.0f;
    [SerializeField] private Animator myDoor = null;
    private bool IsOpen = false;
    // Start is called before the first frame update


    public void Shoot()
    {
        Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);
        float l_LaserDistance = m_MaxLaserDistance;
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxLaserDistance, m_LaserLayerMask.value))
        {
            l_LaserDistance = Vector3.Distance(m_Laser.transform.position, l_RaycastHit.point);
            if (l_RaycastHit.collider.tag == "RefractionCube")
            {
                l_RaycastHit.collider.GetComponent<RefractionCube>().CreateRefraction();
                
            }
            if(l_RaycastHit.collider.tag == "Portal")
            {
                l_RaycastHit.collider.GetComponent<Portal>().CreateRefraction();
            }
            
            if (l_RaycastHit.collider.gameObject.tag == "Turret") 
            {
                Destroy(l_RaycastHit.collider.gameObject);
            }

            if (l_RaycastHit.collider.gameObject.tag == "ButtonLasse")
            {
                if (IsOpen == false)
                {
                    myDoor.Play("DoorAnim", 0, 0.0f);
                    IsOpen = true;
                }
                    
            }

            if (l_RaycastHit.collider.gameObject.tag == "Player")
            {
                l_RaycastHit.collider.GetComponent<FPPlayerController>().Die();
            }
        }
        m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_LaserDistance));

    }
}
