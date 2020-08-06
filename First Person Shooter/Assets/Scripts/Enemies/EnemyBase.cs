using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public Transform m_player;

    private NavMeshAgent m_agent;
    private HealthSystem m_health;
    private Rigidbody m_rb;
    private Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = gameObject.AddComponent<Rigidbody>();
        m_rb.useGravity = true;
        m_rb.isKinematic = true;
        m_health = GetComponent<HealthSystem>();
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
    }

    private void Update()
    { 
       
    }

    public HealthSystem GetEnemyHealthSystem()
    {
        return m_health;
    }
}