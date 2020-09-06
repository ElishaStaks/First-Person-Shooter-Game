using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState m_stateData;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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

        Collider[] detectedObjects = Physics.OverlapSphere(m_attackPosition.position, m_stateData.m_attackRadius, m_stateData.whatIsPlayer);

        foreach (Collider collider in detectedObjects)
        {
            var target = collider.transform.gameObject.GetComponent<PlayerManager>();
            if (target != null)
            {
                target.GetHealthSystem().Damage(m_entity.m_entityData.m_attackDamage);
            }
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
