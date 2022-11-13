using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Laser m_Laser;
    public float m_AlifeAngleInDegrees = 30.0f;
    bool m_IsAttached = false;
    Rigidbody m_RigidBody;
    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool l_LaserAlife = Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_AlifeAngleInDegrees * Mathf.Deg2Rad);
        m_Laser.gameObject.SetActive(l_LaserAlife);
        if (l_LaserAlife)
        {
            m_Laser.Shoot();
        }
    }

    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }
}
