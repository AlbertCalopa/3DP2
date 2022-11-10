using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public LineRenderer m_Laser;
    public LayerMask m_LaserLayerMask;
    public float m_MaxLaserDistance = 250.0f;
    public float m_AlifeAngleInDegrees = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool l_LaserAlife = Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_AlifeAngleInDegrees * Mathf.Deg2Rad);
        m_Laser.gameObject.SetActive(l_LaserAlife);
        if (l_LaserAlife)
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
            }
            m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_LaserDistance));
        }        
    }
}
