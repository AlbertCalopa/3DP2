using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPPlayerController : MonoBehaviour
{

    float m_Yaw;
    float m_Pitch;
    public float m_YawRotationSpeed;
    public float m_PitchRotationSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_PitchController;

    public bool m_UseYawInverted;
    public bool m_UsePitchInverted;
    

    public CharacterController m_CharacterController;
    public float m_PlayerSpeed;
    public float m_FastSpeedMultiplier;
    public KeyCode m_LeftKeyCode;
    public KeyCode m_RightKeyCode;
    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_DebugLockAngleCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public KeyCode m_Reload = KeyCode.R; 
    
    bool m_AngleLocked = false;
    bool m_AimLocked = true;

    public Camera m_Camera;
    public float m_NormalMovementFOV = 60.0f;
    public float m_RunMovementFOV = 75.0f;
    public float m_FOVSpeed = 4f;
    public float m_FOVSpeedReleased = 10f;  

    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    public float m_JumpSpeed = 10.0f;

    float m_TimeOfGround;
    public float m_TimeGrounded = 0.2f;

    public float m_MaxShootDistance;

    public LayerMask m_ShootingLayerMask;

    
    

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ShootAnimationClip;    
    public AnimationClip m_RunAnimationClip;

    bool m_Shooting = false;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    public float m_Health; //el profe el te com life
    public float m_bullets = 10;
    public float m_MaxBullets = 100;
    public float m_ChargerBullets = 10;
    public float m_Points; 
    public float m_Shield;

    bool isReloading;
    bool isRunning;

    public bool isGaleryActive = false;
    public bool pointsActive = false;


    public GameObject GalleyTrigger;
    public GameObject Gallery;
    public GameObject Bridge;
    public GameObject m_HitDamageScreeen;



    [SerializeField]
    List<GameObject> Llista;


    public ParticleSystem shootParticle;


    



    void Start()
    {
        
        
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;

        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        //#if UNITY_EDITOR //para poder bloquear o desbloquear cosas cuando le das a play
        //#else
        //#endif

        SetIdleWeaponAnimation();
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;

       

    }


    void Update()
    {
        UpdateInputDebug();
        ShootingGalery();

        if(m_Health <= 0)
        {
            Die();
        }

        if (m_HitDamageScreeen != null)
        {
            if (m_HitDamageScreeen.GetComponent<Image>().color.a > 0) 
            {
                var color = m_HitDamageScreeen.GetComponent<Image>().color;

                color.a -= 0.01f;

                m_HitDamageScreeen.GetComponent<Image>().color = color;
            }
            
        }

        //Debug.Log("Health: " + m_Health);

        float l_FOV = m_NormalMovementFOV;

        m_TimeOfGround += Time.deltaTime;
        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        Vector3 l_Direction = Vector3.zero;

        float l_Speed = m_PlayerSpeed;

        if (Input.GetKey(m_UpKeyCode))
            l_Direction = l_ForwardDirection;
        else
        {
            l_FOV = Mathf.Lerp(m_Camera.fieldOfView, m_NormalMovementFOV, m_FOVSpeedReleased * Time.deltaTime);
        }
        if (Input.GetKey(m_DownKeyCode))
            l_Direction -= l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode))
            l_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode))
            l_Direction -= l_RightDirection;
        if(Input.GetKeyDown(m_JumpKeyCode) && m_OnGround)
        {
            m_VerticalSpeed = m_JumpSpeed;
        }
        if (Input.GetKeyDown(m_Reload) && isReloading == false && m_bullets != m_ChargerBullets)
        {
            StartCoroutine(Reload());            
        }

        
        if (Input.GetKey(m_RunKeyCode))
        {
            l_Speed = m_PlayerSpeed * m_FastSpeedMultiplier;
            if (l_Direction != Vector3.zero && !isReloading)
            {
                isRunning = true; 
                SetRunWeaponAnimation();
                l_FOV = Mathf.Lerp(m_Camera.fieldOfView, m_RunMovementFOV, m_FOVSpeed * Time.deltaTime);
                
            }
            
            //l_FOV = m_RunMovementFOV;
        } else
        {
            isRunning = false;

            l_FOV = Mathf.Lerp(m_Camera.fieldOfView, m_NormalMovementFOV, m_FOVSpeedReleased * Time.deltaTime);
        }

            m_Camera.fieldOfView = l_FOV;

        l_Direction.Normalize();

        Vector3 l_movement = l_Direction * l_Speed * Time.deltaTime;

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif

        m_Yaw = m_Yaw + l_MouseX * m_YawRotationSpeed * Time.deltaTime * (m_UseYawInverted ? -1.0f : 1.0f); //(m_UseYawInverted ? -1.0f : 1.0f) Si el bool es correcte fara lo primer, sinó lo segon.
        m_Pitch = m_Pitch + l_MouseY * m_PitchRotationSpeed * Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch); //Mathf.Clamp et clava el valor al minim o maxim que haguis donat o al de la meitat.

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_movement);
        if((l_CollisionFlags & CollisionFlags.Above)!= 0 && m_VerticalSpeed > 0.0f)
        {
            m_VerticalSpeed = 0.0f;
        }
        if((l_CollisionFlags & CollisionFlags.Below)!= 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
            m_TimeOfGround = 0;
        }
        else
        {
            m_OnGround = false;
        }
        if(m_TimeOfGround < m_TimeGrounded)
        {
            m_OnGround = true;
        }

        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            //Shoot();
        }

        if (m_Points > 600) 
        {
            Bridge.SetActive(true); 
        }


        void UpdateInputDebug()
        {
            if (Input.GetKeyDown(m_DebugLockAngleCode))
            {
                m_AngleLocked = !m_AngleLocked;
            }
            if (Input.GetKeyDown(m_DebugLockKeyCode))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
                }
            }
        }


        bool CanShoot()
        {
            return true;
        }
        
        /*void Shoot()
        {
            if (m_bullets > 0 && isReloading == false && isRunning == false) 
            {
                m_bullets--;
                Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit l_RaycastHit;
                if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxShootDistance, m_ShootingLayerMask.value))
                {
                    if (l_RaycastHit.collider.tag == "DroneCollider")
                    {
                        l_RaycastHit.collider.GetComponent<HitCollider>().Hit();
                    }
                    else if (l_RaycastHit.collider.tag == "ObjetivosGaleria")
                    {
                        m_Points += 100;
                        l_RaycastHit.collider.GetComponent<GaleriaDeTiro1>().Timer();
                    }
                    else if(l_RaycastHit.collider.tag == "Door")
                    {

                    }
                    else
                    {
                        CreateShootHitParticles(l_RaycastHit.collider, l_RaycastHit.point, l_RaycastHit.normal);
                    }
                }
                SetShootWeaponAnimation();
                m_Shooting = true;
                shootParticle.Play();
            }
            

        }*/
       

        
    }
    void SetIdleWeaponAnimation()
    {
        m_Animation.CrossFade(m_IdleAnimationClip.name);
    }
    void SetShootWeaponAnimation()
    {
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
        StartCoroutine(EndShoot());
    }
    
    void SetRunWeaponAnimation() 
    {
        m_Animation.CrossFade(m_RunAnimationClip.name, 0.1f); 
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
        StartCoroutine(EndShoot());
    }

    IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShootAnimationClip.length);
        m_Shooting = false;
    }
    
          
    

    public void OnTriggerEnter(Collider other) //si colisiona
    {
       
        
        if (other.tag == "DeadZone")
        {
            Die();
        }
        else if (other.tag == "GaleriaDeTiro")
        {
            isGaleryActive = true;
            
        }
        else if (other.tag == "NoGaleriaDeTiro")
        {
            m_Points = 0;
            isGaleryActive = false;
            Gallery.SetActive(false);            
            pointsActive = false;
        }
        else if(other.tag == "WallKill")
        {
            Die();
        }
    }

    void ShootingGalery()
    {
        if(isGaleryActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                 

                Gallery.SetActive(true);
                m_Animation.Play(); 
                pointsActive = true;
                
                foreach (GameObject g in Llista)
                {
                    g.GetComponent<MeshCollider>().enabled = true;
                    g.GetComponent<Renderer>().enabled = true;
                }
                    
            }
        }
    }

  



    public void Die()
    {
        m_Health = 0.0f;
        
        
        //GameController.GetGameController().RestartGame();

    }
    public void RestartGame() 
    {
        
        m_Health = 0.5f;
        m_Shield = 0.5f;
        m_Points = 0;
        m_bullets = 10;
        m_MaxBullets = 100;
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;
        Gallery.SetActive(false);
        isGaleryActive = false;
        pointsActive = false;
        



    }

    private IEnumerator Reload()
    {
        isReloading = true;
        
        yield return new WaitForSeconds(2);
        m_MaxBullets -= 10 - m_bullets;
        m_bullets = m_ChargerBullets;
        isReloading = false;

}
    
        
}
