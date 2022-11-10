using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionCube : MonoBehaviour
{

    public Laser m_Laser;
    bool m_RefractionEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Laser.gameObject.SetActive(m_RefractionEnabled);
        m_RefractionEnabled = false;
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
