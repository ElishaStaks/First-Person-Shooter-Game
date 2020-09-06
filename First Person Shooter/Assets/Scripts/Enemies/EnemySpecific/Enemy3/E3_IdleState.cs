 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_IdleState : IdleState
{
    private Enemy3 m_enemy;

    public E3_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
            m_stateMachine.ChangeState(m_enemy.m_rangedRaycastAttackState);
        }
    }
}
