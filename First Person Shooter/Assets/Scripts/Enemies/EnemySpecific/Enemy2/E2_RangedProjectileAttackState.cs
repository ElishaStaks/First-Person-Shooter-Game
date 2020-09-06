using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_RangedProjectileAttackState : RangedProjectileAttackState
{
    private Enemy2 m_enemy;

    public E2_RangedProjectileAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedProjectileAttackState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
        m_entity.m_agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }

    public override void Update()
    {
        base.Update();

        if (isAnimFinished && !m_entity.Distance(m_entity.m_entityData.m_closeRangeDistance))
        {
            m_stateMachine.ChangeState(m_enemy.m_moveState);
        }
    }
}
