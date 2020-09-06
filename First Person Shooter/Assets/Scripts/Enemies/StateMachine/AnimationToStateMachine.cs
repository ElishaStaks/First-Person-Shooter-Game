using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState m_attackState;

    private void TriggerAttack()
    {
        m_attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        m_attackState.FinishAttack();
    }
}
