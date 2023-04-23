using System;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class BrackeyCharacterController : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationsHandler animationHandler;
    
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;

    [SerializeField] private PlayerAbilities m_PlayerAbilities;

    const float k_GroundedRadius = .2f;
    [SerializeField]
    private bool m_Grounded;
    const float k_CeilingRadius = .2f;
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
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
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
                    OnLandEvent.Invoke();
                    m_PlayerAbilities.RefreshAirJumps();
                }
                break;
            }
        }
    }

    public float gravityWhileJumpingMultiplier = 1f;
    public float gravityWhileFallingMultiplier = 2f;
    public float terminalVelocity = 10f;
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
                m_Rigidbody2D.AddForce(new Vector2(0,Physics2D.gravity.y * gravityWhileJumpingMultiplier));
                Debug.DrawLine(transform.position, transform.position + new Vector3(0, Physics2D.gravity.y * gravityWhileJumpingMultiplier), Color.green);
                break;
            //If falling, apply falling gravity
            case JumpState.Falling:
                m_Rigidbody2D.AddForce(new Vector2(0, Physics2D.gravity.y * gravityWhileFallingMultiplier));
                Debug.DrawLine(transform.position, transform.position + new Vector3(0, Physics2D.gravity.y * gravityWhileFallingMultiplier), Color.red);
                break;
        }
        
    
    }

    public void Move(float move, bool crouch, bool jump, bool jumpEnded)
    {
        animationHandler.SetLanded(m_Grounded);
        animationHandler.SetMove(move);

        if (!crouch && m_CeilingCheck)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        if (m_Grounded || m_AirControl)
        {
            if (crouch)
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
            else
            {
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
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
            m_Grounded = false;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            animationHandler.Jump();
            //Play jump sound
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
    }
    
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
