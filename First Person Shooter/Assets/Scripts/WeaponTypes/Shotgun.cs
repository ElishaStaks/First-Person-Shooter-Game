using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Spread Range")]
    public float verticalSpread = 10f;
    public float horizontalSpread = 10f;

    [Header("Pellets Per Shot")]
    public int m_numberOfPellets;

    protected override void Update()
    {
        if (!GameManager.isPaused)
        {
            if (!hasPickedUp)
            {
                float y = Mathf.PingPong(Time.time, 1f);
                Vector3 axis = new Vector3(0f, y, 0f);
                transform.Rotate(axis, 1f);
            }

            if (!isHeld) return;

            if (m_time < m_animTime)
            {
                m_time += Time.deltaTime;
                m_time = Mathf.Clamp(m_time, 0, m_animTime);
                var delta = -(Mathf.Cos(Mathf.PI * (m_time / m_animTime)) - 1f) / 2f;
                transform.localPosition = Vector3.Lerp(m_startPosition, Vector3.zero, delta);
                transform.localRotation = Quaternion.Lerp(m_startRotation, Quaternion.identity, delta);
            }
            else
            {
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, m_resetSmooth * Time.deltaTime);
            }

            if (isReloading)
            {
                m_rotationTime += Time.deltaTime;
                var spin = (Mathf.Cos(Mathf.PI * (m_rotationTime / m_reloadSpeed)) - 1f) / 2f;
                transform.localRotation = Quaternion.Euler(new Vector3(spin * 360f, 0, 0));
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && m_ammo < m_maxAmmo)
            {
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButton(0) && !isShooting && !isReloading)
            {
                Shoot();
                m_ammo--;
                AudioManager.Instance.PlaySound(SoundType.UZI_SHOT);
                StartCoroutine(m_ammo <= 0 ? Reload() : CoolDown());
                CameraShake.instance.Shake(0.1f, 0.03f);
            }
        }
    }

    protected override void Shoot()
    {
        transform.localPosition += new Vector3(0, 0, m_kickBackForce);

        float x = 0;
        float y = 0f;
        float radians = 0f;

        for (int i = 0; i < m_numberOfPellets; i++)
        {
            radians = Random.Range(0f, 360f) * Mathf.Rad2Deg;
            x = Random.Range(0f, horizontalSpread / 2f) * Mathf.Cos(radians);
            y = Random.Range(0f, verticalSpread / 2f) * Mathf.Sin(radians);

            Vector3 deviation = new Vector3(x, y, 0);

            RaycastHit hit;

            if (Physics.Raycast(m_playerCamera.position, deviation + m_playerCamera.forward, out hit, m_range))
            {
                var enemy = hit.transform.GetComponent<Monster>();
                if (enemy != null)
                {
                    ImpactReciever reciever = hit.transform.gameObject.GetComponent<ImpactReciever>();
                    if (reciever)
                    {
                        reciever.AddForce(m_playerCamera.forward, m_hitForce);
                    }
                    enemy.GetEnemyHealthSystem().Damage(m_damage);
                    Instantiate(m_bloodFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }

                if (hit.collider && !enemy)
                {
                    Instantiate(m_environmentHoleFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
            }
        }
    }
}
