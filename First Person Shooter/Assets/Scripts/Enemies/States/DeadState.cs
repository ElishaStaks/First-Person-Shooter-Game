using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState m_stateData;

    public DeadState(Entity entity, FiniteStateMachine stateMachine, D_DeadState stateData) : base(entity, stateMachine)
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
        //m_entity.GetComponent<Animator>().enabled = false;
        //m_entity.SetCharacterControllerState(false);
        //m_entity.SetColliderState(true);
        GameObject.Destroy(m_entity.gameObject);
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
