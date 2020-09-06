using UnityEngine;

public class FiniteStateMachine
{
    public State m_currentState { get; private set; }

    // Initialising the new state
    public void Initialise(State state)
    {
        m_currentState = state;
        m_currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        // exits the current state
        m_currentState.Exit();
        // sets the new state
        m_currentState = newState;
        // enters the new state
        m_currentState.Enter();
    }
}
