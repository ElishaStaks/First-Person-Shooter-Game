using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour
{
    [Header("Attack")]
    public float m_attackRange;
    public float m_attackRate;

    [Header("Movement")]
    public float m_gravity = -9.81f;
    public float m_movementSpeed;
    public float m_turnSpeed;

    [Header("Damage")]
    public float m_damage;

    [Header("Jump Pad")]
    public float m_jumpHeight;

    [Header("References")]
    public GameObject m_spawnFX;

    private Transform m_player;
    private Vector3 m_velocity;
    private NavMeshAgent m_agent;
    private HealthSystem m_health;
    private CharacterController m_character;
    private Animator m_anim;
    private float m_nextAttackTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        m_player = FindObjectOfType<PlayerManager>().transform;
        m_character = GetComponent<CharacterController>();
        m_health = GetComponent<HealthSystem>();
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();

        m_agent.stoppingDistance = m_attackRange;
        m_agent.speed = m_movementSpeed;
        m_agent.updatePosition = false;
        m_agent.updateRotation = false;
    }

    private void Start()
    {
        Instantiate(m_spawnFX, transform.position + Vector3.up, Quaternion.identity);
    }

    private void Update()
    {
        if (!m_health.IsDead())
        {
            m_anim.SetBool("IsChasing", true);
            Move();

            if (m_agent.remainingDistance - m_attackRange < 0.1f)
            {
                m_anim.SetBool("IsChasing", false);
                Attack();
            }
            else
            {
                m_anim.SetBool("IsAttack", false);
                m_anim.SetBool("IsChasing", true);
            }
        }
        else
        {
            m_health.Die();
            Destroy(gameObject);
        }
    }

    public void Move()
    {
        Vector3 lookPos;
        Vector3 desiredDirection;
        Quaternion targetRot;

        m_agent.destination = m_player.transform.position;
        desiredDirection = m_agent.desiredVelocity.normalized;

        lookPos = m_player.transform.position - transform.position;
        lookPos.y = 0;
        targetRot = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_movementSpeed * Time.deltaTime);

        m_character.Move(desiredDirection * m_movementSpeed * Time.deltaTime);
        m_agent.velocity = m_character.velocity;
   
        m_velocity.y += m_gravity * Time.deltaTime;
        m_character.Move(m_velocity * Time.deltaTime);
    }

    public void Attack()
    {
        if (Time.time > m_nextAttackTime)
        {
            m_nextAttackTime = Time.time + m_attackRate;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, m_attackRange))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    var player = hit.transform.gameObject.GetComponent<PlayerManager>();
                    m_anim.SetBool("IsAttack", true);
                    Debug.DrawLine(transform.position, transform.position + transform.forward * m_attackRange, Color.red);
                    player.GetHealthSystem().Damage(m_damage);
                    CameraShake.instance.Shake(0.1f, 0.3f);
                    //AudioManager.Instance.PlaySound(SoundType.HURT);
                }
            } 
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "JumpPad":
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                break;
            default:
                break;
        }
    }

    public HealthSystem GetEnemyHealthSystem()
    {
        return m_health;
    }
}