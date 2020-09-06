using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_DeadState : DeadState
{
    private Enemy3 m_enemy;

    public E3_DeadState(Entity entity, FiniteStateMachine stateMachine, D_DeadState stateData, Enemy3 enemy) : base(entity, stateMachine, stateData)
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
