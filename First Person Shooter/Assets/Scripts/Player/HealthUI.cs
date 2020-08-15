using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Timer")]
    public float m_bloodOverlayTime;
    [Header("References")]
    public Monster m_enemy;

    private float m_bloodOverlayTimer;
    private TextMeshProUGUI m_healthText;
    private Image m_bloodOverlay;
    private HealthSystem m_health;
    private bool isDamaged = false;
    private Color m_alpha;
    private Color m_initialAlpha;
    private Medkit m_medkit;

    private void Awake()
    {
        m_bloodOverlay = transform.Find("bloodOverlay").GetComponentInChildren<Image>();
        m_healthText = transform.Find("healthText").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        m_alpha = m_bloodOverlay.color;
        m_initialAlpha = m_alpha;
        m_bloodOverlayTimer = m_bloodOverlayTime;

        m_health.OnHealed += health_OnHealed;
        m_health.OnDamaged += health_OnDamaged;
    }

    private void Update()
    {
        if (isDamaged)
        {
            if (m_health.GetHealth() >= 50)
            {
                m_bloodOverlayTimer -= Time.deltaTime;
                if (m_bloodOverlayTimer <= 0)
                {
                    m_bloodOverlayTimer = m_bloodOverlayTime;
                    m_bloodOverlay.color = m_initialAlpha;
                    m_alpha.a -= m_enemy.m_damage / 100;
                    isDamaged = false;
                }
            }
        }
    }

    public void SetUpMedkit(Medkit medkit)
    {
        m_medkit = medkit;
    }

    public void SetHealthUI(HealthSystem health)
    {
        m_health = health;
        m_health.OnHealthChanged += HealthBar_OnHealthChanged;

        UpdateHealthAmount();
    }

    private void health_OnHealed(object sender, EventArgs e)
    {
        UpdateHealthAmount();
        UpdateOverlayHeal();
    }

    private void health_OnDamaged(object sender, EventArgs e)
    {
        UpdateBloodOverlay();
    }

    private void HealthBar_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthAmount();
    }

    public void UpdateOverlayHeal()
    {
        m_alpha.a -= m_medkit.m_healAmount / 100;
        m_bloodOverlay.color = m_alpha;
    }

    private void UpdateHealthAmount()
    {
        m_healthText.text = m_health.GetHealth().ToString();

        if (m_health.GetHealth() > 50 || m_health.GetHealth() == m_health.GetHealthMax())
        {
            m_healthText.color = Color.green;
        }
        
        if (m_health.GetHealth() <= 50)
        {
            m_healthText.color = Color.yellow;
        }

        if (m_health.GetHealth() <= 25)
        {
            m_healthText.color = Color.red;
        }
    }

    private void UpdateBloodOverlay()
    {
        isDamaged = true;
        m_alpha.a += m_enemy.m_damage / 100;
        m_bloodOverlay.color = m_alpha;
    }
}
