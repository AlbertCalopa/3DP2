using UnityEngine;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_MirrorPortal;
    public FPPlayerController m_Player;
    public float m_OffsetNearPlane;
    public List<Transform> m_ValidPoints;
    public float m_MinValidDistance;
    public float m_MaxValidDistance;
    public float m_MinDotValidAngle;
    public Laser m_Laser;
    public GameObject LaserTransform;
    bool m_RefractionEnabled = false;

    
    private void Update()
    {
        //m_Laser.gameObject.SetActive(m_RefractionEnabled);
        m_RefractionEnabled = false;
    }
    private void LateUpdate()
    {
        
        Vector3 l_WorldPosition = m_Player.m_Camera.transform.position;
        Vector3 l_LocalPosition = m_OtherPortalTransform.InverseTransformPoint(l_WorldPosition);
        m_MirrorPortal.m_Camera.transform.position = m_MirrorPortal.transform.TransformPoint(l_LocalPosition);
        //LaserTransform.transform.position = m_Laser.transform.TransformPoint(l_LocalPosition);

        Vector3 l_WorldDirection = m_Player.m_Camera.transform.forward;
        Vector3 l_LocalDirection = m_OtherPortalTransform.InverseTransformDirection(l_WorldDirection);
        m_MirrorPortal.m_Camera.transform.forward = m_MirrorPortal.transform.TransformDirection(l_LocalDirection);
        //LaserTransform.transform.forward = m_Laser.transform.TransformDirection(l_LocalDirection);

        float l_Distance = Vector3.Distance(m_MirrorPortal.m_Camera.transform.position, m_MirrorPortal.transform.position);
        m_MirrorPortal.m_Camera.nearClipPlane = l_Distance + m_OffsetNearPlane;
    }

    public bool IsValidPosition(Vector3 StartPosition, Vector3 forward, float MaxDistance, LayerMask PortalLayerMask, out Vector3 Position, out Vector3 Normal)
    {
        Ray l_Ray = new Ray(StartPosition, forward);
        RaycastHit l_RaycastHit;
        bool l_Valid = false;
        Position = Vector3.zero;
        Normal = Vector3.forward;

        if (Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
        {
            if (l_RaycastHit.collider.tag == "DrawableWall")
            {
                l_Valid = true;
                Normal = l_RaycastHit.normal;
                Position = l_RaycastHit.point;
                transform.position = Position;
                transform.rotation = Quaternion.LookRotation(Normal);

                for (int i = 0; i < m_ValidPoints.Count; i++)
                {
                    Vector3 l_Direction = m_ValidPoints[i].position - StartPosition;
                    l_Direction.Normalize();
                    l_Ray = new Ray(StartPosition, l_Direction);
                    if (Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
                    {
                        if (l_RaycastHit.collider.tag == "DrawableWall")
                        {
                            float l_Distance = Vector3.Distance(Position, l_RaycastHit.point);
                            float l_DotAngle = Vector3.Dot(Normal, l_RaycastHit.normal);
                            //Debug.Log("Dist: " + l_Distance+" -´"+l_DotAngle);
                            if (!(l_Distance >= m_MinValidDistance && l_Distance <= m_MaxValidDistance && l_DotAngle > m_MinDotValidAngle))
                            {
                                l_Valid = false;
                            }
                        }
                        else
                        {
                            l_Valid = false;
                        }
                    }
                    else
                    {
                        l_Valid = false;
                    }
                }

            }
            
        }
        return l_Valid;
    }

    public void CreateRefraction()
    {
        
        /*if (m_RefractionEnabled)
        {            
            return;
        }        
        m_RefractionEnabled = true;*/
        if (m_RefractionEnabled)
        {            
            m_Laser.Shoot();
        }

    }
}
