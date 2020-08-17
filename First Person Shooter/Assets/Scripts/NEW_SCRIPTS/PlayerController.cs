using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_gravity = -9.81f;
    [SerializeField] private float m_walkSpeed = 5f;
    [SerializeField] private float m_jumpHeight = 2f;
    [SerializeField] private float m_jumpMultiplier = 5f;
    [SerializeField] private float m_jumpRememberTime = .2f;
    [SerializeField] private float m_groundedRememberTime = .2f;
    [SerializeField] private float m_groundDistance = .4f;
    [SerializeField] private float m_speedSmoothTime = .1f;
    [SerializeField] private Transform m_groundedCheck;
    [SerializeField] private LayerMask m_groundMask;

    [SerializeField] private bool lockCursor = true;

    private float m_initialWalkSpeed;
    private float m_groundedRememberTimer;
    private float m_jumRememberTimer;
    private bool isGrounded = true;
    private float m_currentSpeed = 0f;
    private float m_targetSpeed = 0f;
    private float m_speedSmoothVelocity = 0f;
    private Vector2 m_movementInput;
    private Camera m_mainCamera;
    private CharacterController m_character;
    private bool hasJustLanded = true;
    private bool isMoving = false;
    private bool playStep = true;

    [HideInInspector] public Vector3 m_velocity;

    private void Awake()
    {
        m_character = GetComponent<CharacterController>();
        m_mainCamera = Camera.main;
        m_initialWalkSpeed = m_walkSpeed;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!GameManager.isPaused)
        {
            m_jumRememberTimer -= Time.deltaTime;

            if (IsGrounded())
            {
                if (!hasJustLanded)
                {
                    hasJustLanded = true;
                    //AudioManager.Instance.PlaySound(SoundType.GROUND_LAND);
                }

                m_groundedRememberTimer = m_groundedRememberTime;
            }
            else
            {
                hasJustLanded = false;
                m_groundedRememberTimer -= Time.deltaTime;
            }
        }
        Movement();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "JumpPad":
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * m_jumpMultiplier * -2f * m_gravity);
                break;

            case "Ground":
                m_walkSpeed = m_initialWalkSpeed;
                break;
        }
    }

    public void Movement()
    {
        if (IsGrounded() && m_velocity.y < 0)
        {
            m_velocity.y = -2f;
        }

        m_movementInput = new Vector2(Input.GetAxis("Horizontal"),
                                        Input.GetAxis("Vertical"));

        Vector3 forward = m_mainCamera.transform.forward;
        Vector3 right = m_mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredDirection = (forward * m_movementInput.y + right * m_movementInput.x).normalized;

        if (desiredDirection != Vector3.zero)
        {
            m_targetSpeed = m_walkSpeed * m_movementInput.magnitude;
        }

        m_currentSpeed = Mathf.SmoothDamp(m_currentSpeed, m_targetSpeed, ref m_speedSmoothVelocity, m_speedSmoothTime);
        m_character.Move(desiredDirection * m_currentSpeed * Time.deltaTime);

        Jump();

        m_velocity.y += m_gravity * Time.deltaTime;
        m_character.Move(m_velocity * Time.deltaTime);

    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_jumRememberTimer = m_jumpRememberTime;

            if (m_jumRememberTimer > 0f && m_groundedRememberTimer > 0f)
            {
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                //AudioManager.Instance.PlaySound(SoundType.JUMP);
            }
        }
    }

    public bool IsGrounded()
    {
        return isGrounded = Physics.CheckSphere(m_groundedCheck.position, m_groundDistance, m_groundMask);
    }

    public float GetGravity()
    {
        return m_gravity;
    }
}
