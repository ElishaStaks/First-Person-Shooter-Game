using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using TMPro;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        PISTOL,
        LMG,
        UZI,
    }

    public WeaponType m_weaponType;

    [Header("Throw")]
    public float m_throwForce;

    [Header("Drop")]
    public float m_dropForce;
    public float m_dropExtraForce;
    public float m_rotationForce;

    [Header("Pickup")]
    public float m_animTime;

    [Header("Shooting")]
    public int m_maxAmmo;
    public int m_shotPerSecond;
    public int m_bulletsPerTap;
    public float m_reloadSpeed;
    public float m_hitForce;
    public float m_range;
    public bool isFullyAuto;
    public float m_kickBackForce;
    public float m_resetSmooth;

    [Header("Damage")]
    public float m_damage;

    [Header("Data")]
    public int m_weaponGFXLayer;
    public GameObject m_weaponGFX;
    public GameObject m_weaponFloatFX;
    public GameObject m_environmentHoleFX;
    public GameObject m_bloodFX;
    public Collider[] m_gfxCollider;

    private int m_bulletsShot;
    private float m_time;
    private float m_rotationTime;
    private bool isHeld;
    private Rigidbody m_rb;
    private Transform m_playerCamera;
    private TextMeshProUGUI m_ammoText;
    private GameObject m_reticle;
    private bool isReloading;
    private bool isShooting;
    private int m_ammo;
    private bool hasPickedUp;
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    private void Awake()
    {
        m_rb = gameObject.AddComponent<Rigidbody>();
        m_rb.mass = 0;
        m_rb.useGravity = false;
        m_rb.isKinematic = true;
        hasPickedUp = false;
        m_ammo = m_maxAmmo;
        m_weaponFloatFX = Instantiate(m_weaponFloatFX, transform.position, Quaternion.identity);
    }

    private void Update()
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

            if ((isFullyAuto ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && !isShooting && !isReloading)
            {
                Shoot();
                switch (m_weaponType)
                {
                    case WeaponType.PISTOL:
                        AudioManager.Instance.PlaySound(SoundType.PISTOL_SHOT);
                        break;
                    case WeaponType.LMG:
                        AudioManager.Instance.PlaySound(SoundType.LMG_SHOT);
                        break;
                    case WeaponType.UZI:
                        AudioManager.Instance.PlaySound(SoundType.UZI_SHOT);
                        break;
                    default:
                        break;
                }
                m_ammo--;
                m_ammoText.text = m_ammo + " | " + m_maxAmmo;
                StartCoroutine(m_ammo <= 0 ? Reload() : CoolDown());
                CameraShake.instance.Shake(0.1f, 0.03f);
            }
        }
    }

    public virtual void Shoot()
    {
        transform.localPosition += new Vector3(0, 0, m_kickBackForce);

        RaycastHit hit;

        if (Physics.Raycast(m_playerCamera.position, m_playerCamera.forward, out hit, m_range))
        {
            var enemy = hit.transform.GetComponent<Monster>();
            if (enemy != null)
            {
                //ImpactReciever reciever = hit.transform.gameObject.GetComponent<ImpactReciever>();
                //if (reciever)
                //{
                //    reciever.AddForce(m_playerCamera.forward, m_hitForce);
                //}
                enemy.GetEnemyHealthSystem().Damage(m_damage);
                Instantiate(m_bloodFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

            if (hit.collider && !enemy)
            {
                Instantiate(m_environmentHoleFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }
    }

    private IEnumerator CoolDown()
    {
        isShooting = true;
        yield return new WaitForSeconds(1f / m_shotPerSecond);
        isShooting = false;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        m_ammoText.text = "RELOADING MAG";
        m_rotationTime = 0f;
        AudioManager.Instance.PlaySound(SoundType.RELOAD);
        yield return new WaitForSeconds(m_reloadSpeed);
        m_ammo = m_maxAmmo;
        m_ammoText.text = m_ammo + " | " + m_maxAmmo;
        isReloading = false;
    }

    public void PickUp(Transform weaponHolder, Transform playerCamera, TextMeshProUGUI ammoText, GameObject reticle)
    {
        if (isHeld) return;
        AudioManager.Instance.PlaySound(SoundType.WEAPON_PICKUP);
        Destroy(m_rb);
        Destroy(m_weaponFloatFX);
        transform.parent = weaponHolder;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        foreach (Collider collider in m_gfxCollider)
        {
            collider.enabled = false;
        }

        hasPickedUp = true;
        isHeld = true;
        m_playerCamera = playerCamera;
        m_ammoText = ammoText;
        m_ammoText.text = m_ammo + " | " + m_maxAmmo;
        m_reticle = reticle;
        m_reticle.gameObject.SetActive(true);
        m_ammoText.gameObject.SetActive(true);
    }

    public void Drop(Transform playerCamera)
    {
        if (!isHeld) return;
        m_rb = gameObject.AddComponent<Rigidbody>();
        m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        m_rb.mass = .1f;
        foreach (Collider collider in m_gfxCollider)
        {
            collider.enabled = true;
        }
        m_ammoText.gameObject.SetActive(false);
        m_reticle.gameObject.SetActive(false);
        
        transform.parent = null;
        var forward = -transform.forward;
        forward.y = 0;
        m_rb.angularVelocity = Random.onUnitSphere * m_rotationForce;

        m_rb.velocity = forward * m_dropForce;
        m_rb.velocity += Vector3.up * m_dropExtraForce;
        isHeld = false;
    }

    public void ThrowWeapon()
    {
        if (!isHeld) return;
        m_rb = gameObject.AddComponent<Rigidbody>();
        m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        m_rb.mass = .1f;

        foreach (Collider collider in m_gfxCollider)
        {
            collider.enabled = true;
        }

        m_ammoText.gameObject.SetActive(false);
        m_reticle.gameObject.SetActive(false);

        transform.parent = null;
        var forward = -transform.forward;
        forward.y = 0;

        m_rb.velocity = forward * m_throwForce;
        isHeld = false;
    }

    public int GetPlayerAmmo()
    {
        return m_maxAmmo;
    }
}
