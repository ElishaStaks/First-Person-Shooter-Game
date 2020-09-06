using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState m_moveState { get; private set; }
    public E2_RangedProjectileAttackState m_rangedAttackState { get; private set; }
    public E2_DeadState m_deadState { get; private set; }

    [Header("State Data")]
    [SerializeField] private D_MoveState m_moveStateData;
    [SerializeField] private D_RangedProjectileAttackState m_rangedProjectileAttackStateData;
    [SerializeField] private D_DeadState m_deadStateData;

    [Header("Enemy Specific Stats")]
    [SerializeField] private Transform m_rangedAttackPosition;

    public override void Start()
    {
        base.Start();
        //SetCharacterControllerState(true);
        //SetColliderState(false);

        m_moveState = new E2_MoveState(this, m_stateMachine, "move", m_moveStateData, this);
        m_rangedAttackState = new E2_RangedProjectileAttackState(this, m_stateMachine, "throw", m_rangedAttackPosition, m_rangedProjectileAttackStateData, this);
        m_deadState = new E2_DeadState(this, m_stateMachine, m_deadStateData, this);

        m_stateMachine.Initialise(m_moveState);
    }

    public override void Update()
    {
        base.Update();
        TargetLockOn(m_moveStateData.m_turnSpeed);
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        if (m_healthSystem.IsDead())
        {
            m_stateMachine.ChangeState(m_deadState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
