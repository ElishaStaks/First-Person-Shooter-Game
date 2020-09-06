using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_StunState : StunState
{
    private Enemy3 m_enemy;

    public E3_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        m_enemy.ResetStunResistance();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (isStunOver)
        {
            if (m_enemy.Distance(m_entity.m_entityData.m_closeRangeDistance))
            {
                m_stateMachine.ChangeState(m_enemy.m_rangedRaycastAttackState);
            }
            else
            {
                m_stateMachine.ChangeState(m_enemy.m_idleState);
            }
        }
    }
}
