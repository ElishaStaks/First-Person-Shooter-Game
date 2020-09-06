using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAttackState : AttackState
{
    protected D_SuicideAttackState m_stateData;

    public SuicideAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_SuicideAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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
        m_entity.m_agent.isStopped = true;
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
        GameObject.Instantiate(m_stateData.m_explosionPrefab, m_attackPosition.position, m_attackPosition.rotation);
        GameObject.Destroy(m_entity.gameObject);
    }

    public override void Update()
    {
        base.Update();
    }
}
