using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// base script for all our enemies

public class Entity : MonoBehaviour
{
    public D_Entity m_entityData;
    public FiniteStateMachine m_stateMachine;
    public CharacterController m_characterController { get; private set; }
    public NavMeshAgent m_agent { get; private set; }
    public Animator m_anim { get; private set; }
    public Transform m_player { get; private set; }
    public AnimationToStateMachine m_animToStateMachine { get; private set; }
    public HealthSystem m_healthSystem { get; set; }
    [SerializeField] private GameObject m_spawnFX;
    private Vector3 m_velocity;

    public virtual void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_characterController = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
        m_player = FindObjectOfType<PlayerController>().transform;
        m_animToStateMachine = GetComponent<AnimationToStateMachine>();
        m_healthSystem = GetComponent<HealthSystem>();
        Instantiate(m_spawnFX, transform.position + Vector3.up, Quaternion.identity);

        m_stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        if (!m_healthSystem.IsDead())
        {
            m_stateMachine.m_currentState.Update();
        }
    }

    public virtual void FixedUpdate()
    {
        m_stateMachine.m_currentState.FixedUpdate();
    }

    public virtual void Damage(float damage)
    {
        m_healthSystem.Damage(damage);
    }

    public virtual bool Distance(float distance)
    {
        return (Vector3.Distance(transform.position, m_player.position) < distance);
    }

    public virtual void SetVelocity(float movementSpeed)
    {
        Vector3 desiredDirection;

        m_agent.destination = m_player.transform.position;
        desiredDirection = m_agent.desiredVelocity.normalized;

        m_characterController.Move(desiredDirection * movementSpeed * Time.fixedDeltaTime);
        m_agent.velocity = m_characterController.velocity;

        m_velocity.y += m_entityData.m_gravity * Time.fixedDeltaTime;
        m_characterController.Move(m_velocity * Time.fixedDeltaTime);
    }

    // Knock back
    public virtual void SetVelocity(float velocity, Vector3 angle, int direction)
    {
        angle.Normalize();
        m_velocity.Set(angle.x * velocity * direction, angle.y * velocity, angle.z * velocity * direction);
        m_agent.velocity = m_velocity;
    }

    public virtual void TargetLockOn(float speed)
    {
        Vector3 look;
        Quaternion targetRot;

        look = m_player.transform.position - transform.position;
        targetRot = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.deltaTime);
    }

    public virtual void Spawn()
    {
        Instantiate(this, transform.position, Quaternion.identity);
    }

    public virtual void OnDrawGizmos()
    {

    }
}
