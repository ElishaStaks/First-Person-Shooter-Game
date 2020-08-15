using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    private bool isControllerEnabled;
    private HealthSystem m_health;
    public HealthUI m_healthUI;
    [HideInInspector] public int m_ammo;

    private void Awake()
    {
        instance = this;
        m_health = GetComponent<HealthSystem>();
        isControllerEnabled = true;
    }

    private void Start()
    {
        m_healthUI.SetHealthUI(m_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_health.IsDead() && isControllerEnabled)
        {
            if (!GameManager.isPaused)
            {
                //TODO: Manage stuff
            }
        }
        else
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return m_health;
    }
}
