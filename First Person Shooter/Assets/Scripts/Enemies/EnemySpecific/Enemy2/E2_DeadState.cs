using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DeadState : DeadState
{
    private Enemy2 m_enemy;

    public E2_DeadState(Entity entity, FiniteStateMachine stateMachine, D_DeadState stateData, Enemy2 enemy) : base(entity, stateMachine, stateData)
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
    }
}
