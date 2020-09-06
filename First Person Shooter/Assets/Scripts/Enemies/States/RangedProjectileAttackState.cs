using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileAttackState : AttackState
{
    protected D_RangedProjectileAttackState m_stateData;
    protected GameObject m_projectile;
    protected Grenade m_projectileData;

    public RangedProjectileAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedProjectileAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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
        m_projectile = GameObject.Instantiate(m_stateData.m_projectile, m_attackPosition.position, m_attackPosition.rotation);
        m_projectileData = m_projectile.GetComponent<Grenade>();
        m_projectileData.ThrowGrenade(m_stateData.m_projectile, m_entity.m_entityData.m_attackDamage);
    }

    public override void Update()
    {
        base.Update();
    }
}
