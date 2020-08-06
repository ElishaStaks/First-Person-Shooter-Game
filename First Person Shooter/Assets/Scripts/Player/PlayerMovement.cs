using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_gravity = -9.81f;
    public float m_walkSpeed = 5f;
    public float m_sprintSpeed = 10f;
    public float m_jumpHeight = 2f;
    public float m_jumpMultiplier = 5f;
    public float m_jumpRememberTime = .2f;
    public float m_groundedRememberTime = .2f;
    public float m_groundDistance = .4f;
    public float m_speedSmoothTime = .1f;
    public Transform m_groundedCheck;
    public LayerMask m_groundMask;

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

    [HideInInspector] public Vector3 m_velocity;

    private void Awake()
    {
        m_character = GetComponent<CharacterController>();
        m_mainCamera = Camera.main;
        m_initialWalkSpeed = m_walkSpeed;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!PauseController.isPaused)
        {
            m_jumRememberTimer -= Time.deltaTime;

            if (IsGrounded())
            {
                m_groundedRememberTimer = m_groundedRememberTime;
            }
            else
            {
                m_groundedRememberTimer -= Time.deltaTime;
            }

            Movement();
        }
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

        m_movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

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

        Sprint();

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
            }
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_targetSpeed = m_sprintSpeed * m_movementInput.magnitude;
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
