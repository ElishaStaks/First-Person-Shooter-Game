using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
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

            if (Input.GetMouseButtonDown(0) && !isShooting && !isReloading)
            {
                Shoot();
                AudioManager.Instance.PlaySound(SoundType.PISTOL_SHOT);
                m_ammo--;
                StartCoroutine(m_ammo <= 0 ? Reload() : CoolDown());
                CameraShake.instance.Shake(0.1f, 0.03f);
            }
        }
    }
}
