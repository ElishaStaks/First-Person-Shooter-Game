using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{
    public E3_IdleState m_idleState { get; private set; }
    public E3_RangedRaycastAttackState m_rangedRaycastAttackState { get; private set; }
    public E3_DeadState m_deadState { get; private set; }
    public E3_StunState m_stunState { get; private set; }

    [Header("State Data")]
    [SerializeField] private D_IdleState m_idleStateData;
    [SerializeField] private D_RangedRaycastAttackState m_rangedRaycastAttackStateData;
    [SerializeField] private D_DeadState m_deadStateData;
    [SerializeField] private D_StunState m_stunStateData;

    [Header("Enemy Specific Stats")]
    [SerializeField] private Transform m_rangedAttackPosition;
    [SerializeField] private float m_targetLockSpeed = 10f;

    private float m_currentStunResistance;
    private float m_lastDamageTime;
    private bool isStunned;

    public override void Start()
    {
        base.Start();
        m_currentStunResistance = m_stunStateData.m_stunResistance;

        m_idleState = new E3_IdleState(this, m_stateMachine, "idle", m_idleStateData, this);
        m_rangedRaycastAttackState = new E3_RangedRaycastAttackState(this, m_stateMachine, "shoot", m_rangedAttackPosition, m_rangedRaycastAttackStateData, this);
        m_deadState = new E3_DeadState(this, m_stateMachine, m_deadStateData, this);
        m_stunState = new E3_StunState(this, m_stateMachine, "stunned", m_stunStateData, this);
        m_stateMachine.Initialise(m_idleState);
    }

    public override void Damage(float damage)
    {
        m_lastDamageTime = Time.time;

        base.Damage(damage);

        m_currentStunResistance -= 1;

        if (m_currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if (m_healthSystem.IsDead())
        {
            m_stateMachine.ChangeState(m_deadState);
        }

        if (isStunned && m_stateMachine.m_currentState != m_stunState)
        {
            m_stateMachine.ChangeState(m_stunState);
        }
    }

    public void ResetStunResistance()
    {
        isStunned = false;
        m_currentStunResistance = m_stunStateData.m_stunResistance;
    }

    public override void Update()
    {
        base.Update();
        TargetLockOn(m_targetLockSpeed);

        if (Time.time >= m_lastDamageTime + m_stunStateData.m_stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}