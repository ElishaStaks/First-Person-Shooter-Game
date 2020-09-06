using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float m_blastRadius = 1f;
    [SerializeField] private GameObject m_explosion;

    private float m_damage;
    private GameObject m_projectile;
    private Transform m_target;
    private Collider[] m_colliders;

    private void Awake()
    {
        m_target = FindObjectOfType<PlayerController>().transform;
    }

    private void Start()
    {
        Projectile();
    }

    public void Projectile()
    {
        Vector3 vo = CalculateVelocity(m_target.position, transform.position, 1f);
        GetComponent<Rigidbody>().velocity = vo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact();
        Explode(collision.contacts[0].point);
    }

    public void Impact()
    {
        m_colliders = Physics.OverlapSphere(transform.position, m_blastRadius);

        foreach (Collider collider in m_colliders)
        {
            var target = collider.gameObject.GetComponent<PlayerManager>();

            if (target != null)
            {
                target.GetHealthSystem().Damage(m_damage);
            }
        }
    }

    public void Explode(Vector3 position)
    {
        if (m_explosion != null)
        {
            Instantiate(m_explosion, position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void ThrowGrenade(GameObject projectile, float damage)
    {
        m_projectile = projectile;
        m_damage = damage;
    }

    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // define distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        // create float to represent our distance
        float sy = distance.y;
        float sxz = distanceXZ.magnitude;

        float vxz = sxz / time;
        float vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= vxz;
        result.y = vy;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_blastRadius);
    }
}
