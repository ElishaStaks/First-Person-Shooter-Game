using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_RangedRaycastAttackState : RangedRaycastAttackState
{
    private Enemy3 m_enemy;

    public E3_RangedRaycastAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedRaycastAttackState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
            m_stateMachine.ChangeState(m_enemy.m_idleState);
        }
    }
}