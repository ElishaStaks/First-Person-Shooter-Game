using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public D_IdleState m_stateData;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        m_entity.SetVelocity(0);
        m_entity.m_agent.isStopped = true;
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
