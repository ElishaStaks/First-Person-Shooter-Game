using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    [SerializeField] private float m_maxHealth;
    private float m_currentHealth;

    private void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    public float GetHealth()
    {
        return m_currentHealth;
    }

    public float GetHealthMax()
    {
        return m_maxHealth;
    }

    public void Damage(float amount)
    {
        m_currentHealth -= amount;
        if (m_currentHealth < 0f)
        {
            m_currentHealth = 0f;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (m_currentHealth <= 0f)
        {
            Die(); 
        }
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
        IsDead();
    }

    public bool IsDead()
    {
        return m_currentHealth <= 0;
    }

    public void Heal(float amount)
    {
        m_currentHealth += amount;
        if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
            
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealComplete()
    {
        m_currentHealth = m_maxHealth;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealth(float health)
    {
        m_currentHealth = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealthMax(float healthMax, bool fullHealth)
    {
        m_maxHealth = healthMax;

        if (fullHealth)
        {
            m_currentHealth = healthMax;
        }

        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsHealthFull()
    {
        return m_currentHealth == m_maxHealth;
    }
}
