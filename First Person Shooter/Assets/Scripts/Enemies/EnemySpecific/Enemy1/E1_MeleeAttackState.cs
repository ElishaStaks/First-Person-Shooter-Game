using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MeleeAttackState : MeleeAttackState
{
    private Enemy1 m_enemy;

    public E1_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

        if (isAnimFinished)
        {
            m_stateMachine.ChangeState(m_enemy.m_moveState);
        }
    }
}
