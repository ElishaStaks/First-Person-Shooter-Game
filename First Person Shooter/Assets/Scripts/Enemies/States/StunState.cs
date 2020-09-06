using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState m_stateData;
    protected bool isStunOver;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        isStunOver = false;
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
        if (Time.time >= m_startTime + m_stateData.m_stunTime)
        {
            isStunOver = true;
        }
    }
}
