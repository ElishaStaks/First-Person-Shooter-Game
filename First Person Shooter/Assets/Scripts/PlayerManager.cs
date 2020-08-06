using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool isControllerEnabled;
    private HealthSystem m_health;

    private void Awake()
    {
        m_health = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_health.IsDead() && isControllerEnabled)
        {
            if (!PauseController.isPaused)
            {
                //TODO: Manage stuff
            }
        }
    }
}
