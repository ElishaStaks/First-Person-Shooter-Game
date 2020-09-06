using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_MoveState m_moveState { get; private set; }
    public E1_SuicideAttackState m_suicideAttackState { get; private set; }

    [Header("State Data")]
    [SerializeField] private D_MoveState m_moveStateData;
    [SerializeField] private D_SuicideAttackState m_SuicideAttackStateData;

    [Header("Enemy Specific Stats")]
    [SerializeField] private Transform m_SuicideAttackPosition;

    public override void Start()
    {
        base.Start();
        m_moveState = new E1_MoveState(this, m_stateMachine, "move", m_moveStateData, this);
        m_suicideAttackState = new E1_SuicideAttackState(this, m_stateMachine, "explode", m_SuicideAttackPosition, m_SuicideAttackStateData, this);

        m_stateMachine.Initialise(m_moveState);
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        if (m_healthSystem.IsDead())
        {
            Collider[] detectedObjects = Physics.OverlapSphere(m_SuicideAttackPosition.position, m_SuicideAttackStateData.m_attackRadius, m_SuicideAttackStateData.whatIsPlayer);

            foreach (Collider collider in detectedObjects)
            {
                var target = collider.transform.gameObject.GetComponent<PlayerManager>();
                if (target != null)
                {
                    target.GetHealthSystem().Damage(m_entityData.m_attackDamage);
                }
            }

            Instantiate(m_SuicideAttackStateData.m_explosionPrefab, m_SuicideAttackPosition.position, m_SuicideAttackPosition.rotation);
            Destroy(gameObject);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(m_SuicideAttackPosition.position, m_SuicideAttackStateData.m_attackRadius);
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
