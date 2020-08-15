using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReciever : MonoBehaviour
{
    // mass of the character
    public float m_mass = 1f;
    // set force to 0 because we dont want any force at first
    private Vector3 m_force = Vector3.zero;
    private CharacterController m_character;

    // Start is called before the first frame update
    void Awake()
    {
        m_character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the force of the character is more than .2
        if (m_force.magnitude > .2f)
        {
            // move the character based on the force
            m_character.Move(m_force * Time.deltaTime);
            // consumes the force energy each cycle
            m_force = Vector3.Lerp(m_force, Vector3.zero, 5 * Time.deltaTime);
        }
    }

    public void AddForce(Vector3 direction, float force)
    {
        direction.Normalize();
        if (direction.y < 0)
        {
            // adds down force on the ground
            direction.y = -direction.y;
            m_force += direction.normalized * force / m_mass;
        }
    }
}
