using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [Header("Healing")]
    public float m_healAmount;

    [Header("References")]
    public GameObject m_particleEffect;
    public HealthUI m_healthUI;

    private Vector3 m_startPosition;

    private void Start()
    {
        m_healthUI.SetUpMedkit(this);
        m_startPosition = transform.position;
        m_particleEffect = Instantiate(m_particleEffect, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (!GameManager.isPaused)
        {
            float y = Mathf.PingPong(Time.time, 1f);
            Vector3 axis = new Vector3(0, y, 0);
            transform.Rotate(axis, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerManager>();
            if (player.GetHealthSystem().IsHealthFull()) return;
            AudioManager.Instance.PlaySound(SoundType.HEALTH_PICKUP);
            player.GetHealthSystem().Heal(m_healAmount);
            Destroy(m_particleEffect);
            Destroy(gameObject);
        }
    }
}
