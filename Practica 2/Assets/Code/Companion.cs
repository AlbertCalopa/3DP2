using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{

    bool m_IsAttached = false;
    Rigidbody m_RigidBody;
    public float m_OffsetTeleportPortal = 1.5f;    
    Portal m_ExitPortal = null;
    BoxCollider m_Collider;
    public PhysicMaterial m_MaterialCubo;


    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();

    }

    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Portal" && !m_IsAttached)
        {
            Portal l_Portal = other.GetComponent<Portal>();
            if (l_Portal != m_ExitPortal)
            {
                Teleport(l_Portal);
            }
            
        }
        if(other.tag == "IceFloor")
        {
            m_Collider.material = m_MaterialCubo;
        }

        if(other.tag == "DestroyWall")
        {
            Destroy(this.gameObject);
        }

        if(other.tag == "Lasse")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Portal")
        {
            if(other.GetComponent<Portal>() == m_ExitPortal)
            {
                m_ExitPortal = null;
            }
        }
        if (other.tag == "IceFloor")
        {
            m_Collider.material = null;
        }
    }

    void Teleport(Portal _Portal)
    {
        Vector3 l_LocalPosotion = _Portal.m_OtherPortalTransform.InverseTransformPoint(transform.position);
        Vector3 l_LocalDirection = _Portal.m_OtherPortalTransform.transform.InverseTransformDirection(transform.forward);

        Vector3 l_LocalVelocity = _Portal.m_OtherPortalTransform.transform.InverseTransformDirection(m_RigidBody.velocity);
        Vector3 l_WorldVelocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalVelocity);

        

        m_RigidBody.isKinematic = true;
        

        transform.forward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalDirection);
        Vector3 l_WorldVelocityNormalized = l_WorldVelocity.normalized;
        transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_LocalPosotion) + l_WorldVelocityNormalized * m_OffsetTeleportPortal;
        transform.localScale *= (_Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x);
        m_RigidBody.isKinematic = false;
        m_RigidBody.velocity = l_WorldVelocity;
        m_ExitPortal = _Portal.m_MirrorPortal;

        //this.gameObject.transform.localScale = _Portal.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
