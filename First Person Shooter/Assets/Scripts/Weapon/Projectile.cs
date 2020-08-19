using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    STANDARD,
}

public enum DamageType
{
    DIRECT,
}

public class Projectile : MonoBehaviour
{
    public ProjectileType m_projectileType = ProjectileType.STANDARD;
    public DamageType m_damageType = DamageType.DIRECT;

    public float m_damage = 100.0f;
    public float m_blastRadius;
    public float m_initialForce = 1000.0f;
    public float m_explosiveForce;
    public float m_speed = 10f;
    public float m_lifeTime = 20f;

    public GameObject m_explosion;
    private float m_lifeTimer;

    private void Awake()
    {
        // Add the initial force to rigidbody
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, m_initialForce);
    }

    private void Update()
    {
        m_lifeTimer += Time.deltaTime;

        if (m_lifeTimer >= m_lifeTime)
        {
            Explode(transform.position);
        }

        if (m_initialForce == 0)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * m_speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_damageType == DamageType.DIRECT)
        {
            OnImpact();
            Explode(collision.contacts[0].point);
        }
    }

    private void OnImpact()
    {
        Vector3 impactPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(impactPos, m_blastRadius);
        foreach (Collider collider in colliders)
        {
            var enemy = collider.transform.GetComponent<Monster>();
            var player = collider.transform.GetComponent<PlayerManager>();

            if (enemy != null)
            {
                var reciever = enemy.transform.GetComponent<ImpactReciever>();
                if (reciever != null)
                {
                    var direction = collider.transform.position - impactPos;
                    var force = Mathf.Clamp(m_explosiveForce, 0, 50);
                    reciever.AddForce(direction, force);
                    enemy.GetEnemyHealthSystem().Damage(m_damage);
                }
            }

            if (player != null)
            {
                var playerReciever = player.transform.GetComponent<ImpactReciever>();
                if (playerReciever != null)
                {
                    var direction = collider.transform.position - impactPos;
                    var force = Mathf.Clamp(m_explosiveForce, 0, 50);
                    playerReciever.AddForce(direction, force);
                    player.GetHealthSystem().Damage(10);
                }
            }
        }
    }

    private void Explode(Vector3 position)
    {
        if (m_explosion != null)
        {
            Instantiate(m_explosion, position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // Modify the damage that this projectile can cause
    public void MultiplyDamage(float amount)
    {
        m_damage *= amount;
    }

    // Modify the inital force
    public void MultiplyForce(float amount)
    {
        m_initialForce *= amount;
    }
}
