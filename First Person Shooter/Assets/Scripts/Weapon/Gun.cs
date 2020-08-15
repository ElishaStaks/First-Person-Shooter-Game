using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public abstract class Gun : MonoBehaviour
{
    public event EventHandler OnAmmoChanged;

    public AmmoTypes m_ammoType;

    [Header("Drop")]
    public float m_dropForce;
    public float m_dropExtraForce;
    public float m_rotationForce;

    [Header("Shooting")]
    public int m_maxAmmo;
    public int m_shotPerSecond;
    public float m_reloadSpeed;
    public float m_hitForce;
    public float m_range;
    public float m_kickBackForce;
    public float m_resetSmooth;

    [Header("Damage Inflict")]
    public float m_damage;

    [Header("Data")]
    public int m_weaponGFXLayer;
    public GameObject m_weaponGFX;
    public GameObject m_weaponFloatFX;
    public GameObject m_environmentHoleFX;
    public GameObject m_bloodFX;
    public Collider[] m_gfxCollider;

    public int m_ammo;
    protected float m_animTime = 0.1f;
    protected float m_timeOfLastShot;
    protected float m_time;
    protected float m_rotationTime;
    protected bool isHeld;
    protected bool isReloading;
    protected bool isShooting;
    protected bool hasPickedUp;
    protected Rigidbody m_rb;
    protected Transform m_playerCamera;
    private AmmoUI m_ammoUI;
    protected GameObject m_reticle;
    protected Vector3 m_startPosition;
    protected Quaternion m_startRotation;

    private void Awake()
    {
        m_ammoUI = FindObjectOfType<AmmoUI>();
        m_rb = gameObject.AddComponent<Rigidbody>();
        m_rb.mass = 0;
        m_rb.useGravity = false;
        m_rb.isKinematic = true;
        hasPickedUp = false;
        m_ammo = m_maxAmmo;
        m_weaponFloatFX = Instantiate(m_weaponFloatFX, transform.position, Quaternion.identity);
        m_ammoUI.SetUpAmmoUI(this);
    }

    protected abstract void Update();

    protected virtual void Shoot()
    {
        transform.localPosition += new Vector3(0, 0, m_kickBackForce);

        RaycastHit hit;

        if (Physics.Raycast(m_playerCamera.position, m_playerCamera.forward, out hit, m_range))
        {
            var enemy = hit.transform.GetComponent<Monster>();
            if (enemy != null)
            {
                enemy.GetEnemyHealthSystem().Damage(m_damage);
                Instantiate(m_bloodFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

            if (hit.collider && !enemy)
            {
                Instantiate(m_environmentHoleFX, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }
    }

    protected IEnumerator CoolDown()
    {
        isShooting = true;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(1f / m_shotPerSecond);
        isShooting = false;
    }

    protected IEnumerator Reload()
    {
        isReloading = true;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        m_ammoUI.m_ammoText.text = "RELOADING MAG";
        m_rotationTime = 0f;
        yield return new WaitForSeconds(m_reloadSpeed);
        m_ammo = m_maxAmmo;
        m_ammoUI.m_ammoText.text = m_ammo + " | " + m_maxAmmo;
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
        m_playerCamera = playerCamera;
        m_ammoUI.m_ammoText = ammoText;
        m_reticle = reticle;
        m_reticle.gameObject.SetActive(true);
        m_ammoUI.m_ammoText.gameObject.SetActive(true);
        isHeld = true;
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
            collider.isTrigger = false;
        }
        m_ammoUI.m_ammoText.gameObject.SetActive(false);
        m_reticle.gameObject.SetActive(false);

        transform.parent = null;
        var forward = -transform.forward;
        forward.y = 0;
        m_rb.angularVelocity = UnityEngine.Random.onUnitSphere * m_rotationForce;
        
        m_rb.velocity = forward * m_dropForce;
        m_rb.velocity += Vector3.up * m_dropExtraForce;
        isHeld = false;
    }
}
