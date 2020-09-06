using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRaycastAttackState : AttackState
{
    protected D_RangedRaycastAttackState m_stateData;
    private Vector3 m_direction;

    public RangedRaycastAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedRaycastAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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
        GameObject.Instantiate(m_stateData.m_muzzleFlash, m_attackPosition.position, m_attackPosition.rotation);

        RaycastHit hit;

        Vector3 direction = (m_entity.m_player.position - m_attackPosition.position).normalized;
        if (Physics.Raycast(m_attackPosition.position, direction, out hit, m_stateData.m_range, m_stateData.whatIsPlayer))
        {
            var target = hit.transform.gameObject.GetComponent<PlayerManager>();

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
