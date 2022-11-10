using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    bool m_IsAttached = false;
    Rigidbody m_RigidBody;
    public Laser m_Laser;
    bool m_RefractionEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Laser.gameObject.SetActive(m_RefractionEnabled);
        m_RefractionEnabled = false;
    }

    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }

    public void CreateRefraction()
    {
        if (m_RefractionEnabled)
        {
            return;
        }
        m_RefractionEnabled = true;
        if (m_RefractionEnabled)
        {
            m_Laser.Shoot();
        }        
       
    }
}
