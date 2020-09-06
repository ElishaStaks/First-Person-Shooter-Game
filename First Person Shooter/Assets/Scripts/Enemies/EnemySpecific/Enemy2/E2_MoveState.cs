using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MoveState : MoveState
{
    private Enemy2 m_enemy;

    public E2_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        m_enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_entity.m_agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (m_entity.Distance(m_entity.m_entityData.m_closeRangeDistance))
        {
            m_stateMachine.ChangeState(m_enemy.m_rangedAttackState);
        }
    }
}
