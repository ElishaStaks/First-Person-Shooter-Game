using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all enemies states
public class State
{
    protected FiniteStateMachine m_stateMachine;
    protected Entity m_entity;
    protected float m_startTime;
    protected string m_animBoolName;

    // Constructor
    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        m_entity = entity;
        m_stateMachine = stateMachine;
        m_animBoolName = animBoolName;
    }

    public State(Entity entity, FiniteStateMachine stateMachine)
    {
        m_entity = entity;
        m_stateMachine = stateMachine;
    }

    // Enter the state
    public virtual void Enter()
    {
        // every call will store the start time 
        m_startTime = Time.time;
        m_entity.m_anim.SetBool(m_animBoolName, true);
        DoChecks();
    }

    // Exit the State
    public virtual void Exit()
    {
        m_entity.m_anim.SetBool(m_animBoolName, false);
    }

    // Update the state
    public virtual void Update()
    {

    }

    // Physics update state
    public virtual void FixedUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
