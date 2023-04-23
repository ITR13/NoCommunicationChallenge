using System;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BrackeyCharacterController : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationsHandler animationHandler;

    [SerializeField] private float m_JumpForce = 1200f;
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = 0;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;

    [SerializeField] private PlayerAbilities m_PlayerAbilities;
    [SerializeField] private PhysicsMaterial2D SlidingMaterial;
    [SerializeField] private PhysicsMaterial2D NormalMaterial;

    const float k_GroundedRadius = .4f;
    [SerializeField]
    private bool m_Grounded;
    [SerializeField]
    private bool m_Sliding;

    const float k_CeilingRadius = .2f;
    private Vector2 startingCapsuleSize;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    public AudioClip jumpSound;
    public AudioSource audioSource;

    
    private void Awake()
    {
        startingCapsuleSize = GetComponentInChildren<CapsuleCollider2D>().size;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    [SerializeField] public float gravityWhileJumpingMultiplier = 3f;
    [SerializeField] public float gravityWhileFallingMultiplier = 5f;
    [SerializeField] public float terminalVelocity = 10f;
    [SerializeField] private float speed;
    [SerializeField] public float m_SlideSpeedStartThreshold { get; private set; } = 5f;
    [SerializeField] public float m_SlideSpeedStopThreshold { get; private set; } = 2f;

    public enum JumpState { Grounded, Jumping, Falling };
    private void ApplyGravity(JumpState jumpState)
    {
        //Switch on jump state
        switch(jumpState)
        {
            //If grounded, reset gravity and set velocity to zero
            case JumpState.Grounded:
                //Do Nothing
                break;
            //If jumping, apply jumping gravity
            case JumpState.Jumping:
                //Add the normal gravity
                m_Rigidbody2D.AddForce(new Vector2(0, Physics2D.gravity.y * gravityWhileJumpingMultiplier));
                //Debug.DrawLine(transform.position, transform.position + new Vector3(0, Physics2D.gravity.y * gravityWhileJumpingMultiplier), Color.green);
                break;
            //If falling, apply falling gravity
            case JumpState.Falling:
                m_Rigidbody2D.AddForce(new Vector2(0, Physics2D.gravity.y * gravityWhileFallingMultiplier));
                //Debug.DrawLine(transform.position, transform.position + new Vector3(0, Physics2D.gravity.y * gravityWhileFallingMultiplier), Color.red);
                break;
        }
        
    
    }

    public void Move(float move, bool crouch, bool jump, bool jumpEnded, bool slidePressed)
    {
        Vector2 startVelocity = m_Rigidbody2D.velocity;
        animationHandler.SetLanded(m_Grounded);
        animationHandler.SetMove(move);
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    if (!jump && m_Rigidbody2D.velocity.y < 0)
                    {
                        // Snap to ground
                        RaycastHit2D[] downcasts = Physics2D.LinecastAll(m_Rigidbody2D.position + (Vector2.up * 0.5f), m_Rigidbody2D.position + (Vector2.down * 5));
                        foreach(RaycastHit2D hit in downcasts)
                        {
                            if (m_WhatIsGround == (m_WhatIsGround | (1 << hit.collider.gameObject.layer)))
                            {
                                transform.position = new Vector2(transform.position.x, hit.point.y + (transform.position.y - m_GroundCheck.position.y));
                                break;
                            }
                        }
                    }
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Math.Max(m_Rigidbody2D.velocity.y, 0));

                    OnLandEvent.Invoke();
                    m_PlayerAbilities.RefreshAirJumps();
                }
                break;
            }
        }

        if (!crouch && m_CeilingCheck)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        m_Sliding = (slidePressed && m_Grounded && m_Rigidbody2D.velocity.magnitude > m_SlideSpeedStartThreshold) || (m_Sliding && m_Rigidbody2D.velocity.magnitude > m_SlideSpeedStopThreshold);

        if ((m_Grounded || m_AirControl) && !m_Sliding)
        {
            if (crouch && !m_Sliding)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= m_CrouchSpeed;

                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            if (!crouch)
            {
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(move * speed, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
    
        // If the player should jump...
        if (jump && (m_Grounded || m_PlayerAbilities.ExpendAirJump()))
        {
            var vel = m_Rigidbody2D.velocity;
            vel.y = Mathf.Max(vel.y + m_JumpForce, m_JumpForce);
            m_Rigidbody2D.velocity = vel;
            animationHandler.Jump();
            //Play jump sound
            m_Grounded = false;
            audioSource.PlayOneShot(jumpSound);
        }
        //If player havent reach apoapsis and they are still holding jump
        // Apply jumping gravity
        if (!jumpEnded && m_Rigidbody2D.velocity.y > 0)
        {
            ApplyGravity(JumpState.Jumping);
        }
        //Else, the player want to fall or they have reached apoapsis
        else
        {
            ApplyGravity(JumpState.Falling);
        }
        if (m_Sliding)
        {
            //Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y);
            //m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            m_Rigidbody2D.AddForce(m_Rigidbody2D.velocity.normalized * .5f);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
        animationHandler.SetSliding(m_Sliding);
        m_Rigidbody2D.sharedMaterial = m_Sliding ? SlidingMaterial : NormalMaterial;
        GetComponentInChildren<CapsuleCollider2D>().size = m_Sliding ? Vector2.one * 0.5f: startingCapsuleSize;
        if (m_Sliding)
            m_PlayerAbilities.ApplySlideDamage(m_Rigidbody2D.velocity);
    }
    
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
