using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using WeaponBobbingV2;

public enum WeaponType
{
    PROJECTILE,
    RAYCAST,
}

public enum ShootingType
{
    FULL_AUTO,
    SEMI_AUTO,
}

public class Weapon : MonoBehaviour
{
    [Header("Name")]
    public string m_name = "";

    [Header("Weapon Type")]
    public WeaponType m_type = WeaponType.RAYCAST;

    [Header("Shooting Type")]
    public ShootingType m_fireType = ShootingType.FULL_AUTO;

    [Header("General")]
    public bool playerWeapon = false;
    public GameObject m_weaponModel;
    public float m_delayBeforeFire = 0.0f;

    [Header("Charge")]
    public bool chargeUp = false;							       
    public float m_maxChargeUp = 2.0f;
    public bool multiplyForce = true;
    public bool multiplyPower = false;
    public float m_powerMultiplier = 1.0f;
    public float m_initialForceMultiplier = 1.0f;
    public bool allowCancel = false;
    private float m_heat = 0.0f;

    [Header("Projectile")]
    public GameObject m_projectile;
    public Transform m_projectileSpawnSpot;

    [Header("Damage / Force")]
    public float m_damage = 10f;
    public float m_forceMultiplier;

    [Header("Range")]
    public float m_range = 9999.0f;

    [Header("Rate of Fire")]
    public float m_rateOfFire = 10f;
    private float m_actualROF;
    private float m_shootTimer;

    [Header("Ammo / Reload")]
    public bool m_infiniteAmmo = false;
    public bool showCurrentAmmo = true;
    public bool reloadAutomatically = true;
    public int m_ammoCapacity = 12;
    public int m_roundsPerShot = 1;
    public float m_reloadTime = 2.0f;
    private int m_currentAmmo;
    private float m_rotationTime = 0;
    private bool isReloading = false;

    [Header("Accuracy")]
    public float m_accuracy = 80.0f;
    private float m_currentAccuracy;
    public float m_accuracyDropPerShot = 1.0f;
    public float m_accuracyRecoverRate = 0.1f;

    [Header("Burst")]
    public int m_burstRate = 3;
    public float m_burstPause = 0.0f;
    private int m_burtCounter = 0;
    private float m_burstTimer = 0.0f;

    [Header("Recoil")]
    public bool recoil = true;
    public float m_recoilKickBackMin = .1f;
    public float m_recoilKickBackMax = .3f;
    public float recoilRotationMin = 0.1f;
    public float recoilRotationMax = 0.25f;
    public float m_recoilRecoveryRate = 0.01f;

    [Header("Shell FX")]
    public bool spitShells = false;
    public GameObject m_shell;
    public Transform m_shellSpitPosition;
    public float m_shellSpitForce = 1.0f;
    public float m_shellForceRandom = .5f;
    public float m_shellSpitTorqueX = 0.0f;               // The torque with which the shells will rotate on the x axis
    public float m_shellSpitTorqueY = 0.0f;               // The torque with which the shells will rotate on the y axis
    public float m_shellTorqueRandom = 1.0f;		      // The variant by which the spit torque can change + or - for each shot

    [Header("Muzzle FX")]
    public bool createMuzzleFlashEffect = true;
    //public GameObject m_muzzleFlashEffect;
    public Transform m_muzzleFlashEffectPosition;
    public GameObject[] m_muzzleFlashEffects = new GameObject[] { null };

    [Header("Hit FX")]
    public bool createHitEffects = true;
    public GameObject m_groundHitEffect;
    public GameObject m_bloodHitEffect;
    //public GameObject[] m_hitEffects = new GameObject[] { null };

    [Header("Cross Hairs")]
    private bool showCrosshair = false;
    public Texture2D m_crosshairTexture;
    public int m_crosshairLength = 10;
    public int m_crosshairWidth = 4;
    public float m_startingCrosshairSize = 10.0f;
    private float m_currentCrosshairSize;

    [Header("Pick Up")]
    //public GameObject m_weaponFloatingFX;
    private bool isHeld = false;
    [SerializeField] private bool hasPickedup = false;

    [Header("Audio")]
    public AudioMixerGroup m_audioGroup;
    public AudioClip m_shootSound;
    public AudioClip m_reloadSound;
    public AudioClip m_dryShotSound;

    private bool canFire = true;
    private Transform m_playerCamera;
    private CapsuleCollider m_capsuleCollider;
    public bool createCollider = true;
    private BobStyle1 m_weaponBobScript;

    private void Awake()
    {
        m_playerCamera = Camera.main.transform;
        m_weaponBobScript = GetComponent<BobStyle1>();
        //m_weaponFloatingFX = Instantiate(m_weaponFloatingFX, transform.position, Quaternion.identity);

        if (createCollider)
        {
            m_capsuleCollider = gameObject.AddComponent<CapsuleCollider>() as CapsuleCollider;
            m_capsuleCollider.isTrigger = true;
            m_capsuleCollider.radius = 1f;
            m_capsuleCollider.height = 3;
        }

        if (m_rateOfFire != 0)
        {
            m_actualROF = 1.0f / m_rateOfFire;
        }
        else
        {
            m_actualROF = 0.01f;
        }

        // Initializes the starting crosshair size 
        m_currentCrosshairSize = m_startingCrosshairSize;

        m_shootTimer = 0.0f;
        // Start of with a full magazine
        m_currentAmmo = m_ammoCapacity;

        if (m_muzzleFlashEffectPosition == null)
        {
            m_muzzleFlashEffectPosition = gameObject.transform;
        }

        if (m_projectileSpawnSpot == null)
        {
            m_projectileSpawnSpot = gameObject.transform;
        }

        if (m_weaponModel == null)
        {
            m_weaponModel = gameObject;
        }

        if (m_crosshairTexture == null)
        {
            m_crosshairTexture = new Texture2D(0, 0);
        }

        InitialiseAudioMixerGroup(gameObject.GetComponent<AudioSource>());
    }

    public void InitialiseAudioMixerGroup(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = m_audioGroup;
    }

    private void Update()
    {
        if (!GameManager.isPaused)
        {
            if (!hasPickedup)
            {
                float y = Mathf.PingPong(Time.time, 1);
                Vector3 axis = new Vector3(0, y, 0);
                transform.Rotate(axis, 1);

                m_weaponBobScript.enabled = false;
            }

            if (!showCrosshair)
            {
                showCrosshair = !showCrosshair;
            }

            // Calculates current accuracy
            m_currentAccuracy = Mathf.Lerp(m_currentAccuracy, m_accuracy, m_accuracyRecoverRate * Time.deltaTime);
            // Calculates the current cross hair size
            // We use this to grow and shrink the crosshair dynamically
            m_currentCrosshairSize = m_startingCrosshairSize + (m_accuracy - m_currentAccuracy) * .8f;
            // Updates fire timer
            m_shootTimer += Time.deltaTime;


            // Player input
            if (playerWeapon)
            {
                PlayerInput();
            }

            if (isReloading)
            {
                m_rotationTime += Time.deltaTime;
                var spin = (Mathf.Cos(Mathf.PI * (m_rotationTime / m_reloadTime)) - 1f) / 2f;
                transform.localRotation = Quaternion.Euler(new Vector3(-spin * 360f, 0, 0));

                if (m_shootTimer >= 0)
                {
                    isReloading = false;
                }
            }

            // Reload gun if out of ammo
            if (reloadAutomatically && m_currentAmmo <= 0)
            {
                Reload();
            }

            // Recoil Recovery
            if (playerWeapon && recoil)
            {
                m_weaponModel.transform.position = Vector3.Lerp(m_weaponModel.transform.position, transform.position, m_recoilRecoveryRate * Time.deltaTime);
                m_weaponModel.transform.rotation = Quaternion.Lerp(m_weaponModel.transform.rotation, transform.rotation, m_recoilRecoveryRate * Time.deltaTime);
            }
        }
        else if (showCrosshair)
        {
            showCrosshair = !showCrosshair;
        }
    }

    private void PlayerInput()
    {
        if (m_type == WeaponType.RAYCAST)
        {
            if (m_shootTimer >= m_actualROF && m_burtCounter < m_burstRate && canFire)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!chargeUp)
                    {
                        Shoot();
                    }
                    else if (m_heat < m_maxChargeUp)
                    {
                        // increase heat timer
                        // once heat timer is greater than max warm up it will be fully charged
                        m_heat += Time.deltaTime;
                    }
                }
                // Checks if charge up is allowed and if the player is holding down the shoot button
                if (chargeUp && Input.GetMouseButtonUp(0))
                {
                    // If user presses tab key it will cancel the warm up
                    if (allowCancel && Input.GetKeyDown(KeyCode.Tab))
                    {
                        // set heat back to 0 ready for the user to charge up again
                        m_heat = 0.0f;
                    }
                    else
                    {
                        Shoot();
                    }
                }
            }
        }

        if (m_type == WeaponType.PROJECTILE)
        {
            if (m_shootTimer >= m_actualROF && m_burtCounter < m_burstRate && canFire)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!chargeUp)
                    {
                        Launch();
                    }
                    else if (m_heat < m_maxChargeUp)
                    {
                        // increase heat timer
                        // once heat timer is greater than max warm up it will be fully charged
                        m_heat += Time.deltaTime;
                    }
                }
                // Checks if charge up is allowed and if the player is holding down the shoot button
                if (chargeUp && Input.GetMouseButtonUp(0))
                {
                    // If user presses tab key it will cancel the warm up
                    if (allowCancel && Input.GetKeyDown(KeyCode.Tab))
                    {
                        // set heat back to 0 ready for the user to charge up again
                        m_heat = 0.0f;
                    }
                    else
                    {
                        Launch();
                    }
                }
            }
        }

        // checks if counter is greater than the rate 
        if (m_burtCounter >= m_burstRate)
        {
            m_burstTimer += Time.deltaTime;
            if (m_burstTimer >= m_burstPause)
            {
                // Reset the burst
                m_burtCounter = 0;
                m_burstTimer = 0.0f;
            }
        }

        // Reload on press
        if (Input.GetKeyDown(KeyCode.R) && m_currentAmmo != m_ammoCapacity)
        {
            Reload();
        }

        if (Input.GetMouseButtonUp(0))
        {
            canFire = true;
        }
    }

    private void Launch()
    {
        m_shootTimer = 0.0f;
        m_burtCounter++;

        // Checks if the weapon is a semi auto, if it is then the player cannot fire the weapon until they let up the shoot button
        if (m_fireType == ShootingType.SEMI_AUTO)
        {
            canFire = false;
        }

        // Checks if there is ammo
        if (m_currentAmmo <= 0)
        {
            // dry fire if there is no ammo
            DryShoot();
            return;
        }

        // Decrease ammo
        if (!m_infiniteAmmo)
        {
            m_currentAmmo--;
        }

        for (int i = 0; i < m_roundsPerShot; i++)
        {
            GameObject obj = Instantiate(m_projectile, m_projectileSpawnSpot.position, m_projectileSpawnSpot.rotation);

            if (chargeUp)
            {
                if (multiplyPower)
                {
                    obj.SendMessage("MultiplyDamage", m_heat * m_powerMultiplier, SendMessageOptions.DontRequireReceiver);
                }

                if (multiplyForce)
                {
                    obj.SendMessage("MultiplyForce", m_heat * m_initialForceMultiplier, SendMessageOptions.DontRequireReceiver);
                }

                m_heat = 0.0f;
            }
        }

        if (recoil)
        {
            Recoil();
        }

        // Muzzle flash effects
        if (createMuzzleFlashEffect)
        {
            //GameObject muzfx = m_muzzleFlashEffects[UnityEngine.Random.Range(0, m_muzzleFlashEffects.Length)];
            //if (muzfx != null)
            //Instantiate(m_muzzleFlashEffect, m_muzzleFlashEffectPosition.position, m_muzzleFlashEffectPosition.rotation);
        }

        // Instantiate shell props
        if (spitShells)
        {
            GameObject shellGO = Instantiate(m_shell, m_shellSpitPosition.position, m_shellSpitPosition.rotation) as GameObject;
            shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(m_shellSpitForce + UnityEngine.Random.Range(0, m_shellForceRandom), 0, 0), ForceMode.Impulse);
            shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(m_shellSpitTorqueX + UnityEngine.Random.Range(-m_shellTorqueRandom, m_shellTorqueRandom), m_shellSpitTorqueY + UnityEngine.Random.Range(-m_shellTorqueRandom, m_shellTorqueRandom), 0), ForceMode.Impulse);
        }

        GetComponent<AudioSource>().PlayOneShot(m_shootSound);
    }

    private void Shoot()
    {
        m_shootTimer = 0.0f;
        m_burtCounter++;

        // Checks if the weapon is a semi auto, if it is then the player cannot fire the weapon until they let up the shoot button
        if (m_fireType == ShootingType.SEMI_AUTO)
        {
            canFire = false;
        }

        // Checks if there is ammo
        if (m_currentAmmo <= 0)
        {
            // dry fire if there is no ammo
            DryShoot();
            return;
        }

        // Decrease ammo
        if (!m_infiniteAmmo)
        {
            m_currentAmmo--;
        }
        RaycastHit hit;

        for (int i = 0; i < m_roundsPerShot; i++)
        {
            float accuracy = (100 - m_currentAccuracy) / 1000;
            Vector3 direction = m_playerCamera.forward;
            direction.x += UnityEngine.Random.Range(-accuracy, accuracy);
            direction.y += UnityEngine.Random.Range(-accuracy, accuracy);
            direction.z += UnityEngine.Random.Range(-accuracy, accuracy);

            m_currentAccuracy -= m_accuracyDropPerShot;
            if (m_currentAccuracy <= 0.0f)
            {
                m_currentAccuracy = 0.0f;
            }

            Ray ray = new Ray(m_playerCamera.position, direction);

            if (Physics.Raycast(ray, out hit, m_range))
            {
                // warm up heat damage increase
                float damage = m_damage;
                if (chargeUp)
                {
                    damage *= m_heat * m_powerMultiplier;
                    m_heat = 0.0f;
                }

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var enemy = hit.transform.gameObject.GetComponent<Entity>();

                    if (createHitEffects)
                    {
                        Instantiate(m_bloodHitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    }

                    if (enemy != null)
                    {
                        enemy.Damage(m_damage);
                    }
                }
                else if ( hit.collider.gameObject.layer != LayerMask.NameToLayer("NotShootable") || hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                {
                    if (createHitEffects)
                    {
                        Instantiate(m_groundHitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    }
                }
            }
        }

        if (recoil)
        {
            Recoil();
        }

        // Muzzle flash effects
        if (createMuzzleFlashEffect)
        {
            GameObject muzfx = m_muzzleFlashEffects[UnityEngine.Random.Range(0, m_muzzleFlashEffects.Length)];
            if (muzfx != null)
                Instantiate(muzfx, m_muzzleFlashEffectPosition.position, m_muzzleFlashEffectPosition.rotation);
        }

        // Instantiate shell props
        if (spitShells)
        {
            GameObject shellGO = Instantiate(m_shell, m_shellSpitPosition.position, m_shellSpitPosition.rotation) as GameObject;
            shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(m_shellSpitForce + UnityEngine.Random.Range(0, m_shellForceRandom), 0, 0), ForceMode.Impulse);
            shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(m_shellSpitTorqueX + UnityEngine.Random.Range(-m_shellTorqueRandom, m_shellTorqueRandom), m_shellSpitTorqueY + UnityEngine.Random.Range(-m_shellTorqueRandom, m_shellTorqueRandom), 0), ForceMode.Impulse);
        }

        GetComponent<AudioSource>().PlayOneShot(m_shootSound);

    }

    private void Reload()
    {
        isReloading = true;
        m_rotationTime = 0f;
        m_currentAmmo = m_ammoCapacity;
        m_shootTimer = -m_reloadTime;

        GetComponent<AudioSource>().PlayOneShot(m_reloadSound);
    }

    private void Recoil()
    {
        // Make sure the user didn't leave the weapon model field blank
        if (m_weaponModel == null)
        {
            Debug.Log("Weapon Model is null.  Make sure to set the Weapon Model field in the inspector.");
            return;
        }

        float kickBack = UnityEngine.Random.Range(m_recoilKickBackMin, m_recoilKickBackMax);
        float kickRot = UnityEngine.Random.Range(recoilRotationMin, recoilRotationMax);

        m_weaponModel.transform.Translate(new Vector3(0, 0, -kickBack), Space.Self);
        m_weaponModel.transform.Rotate(new Vector3(-kickRot, 0, 0), Space.Self);
    }

    private void DryShoot()
    {
        GetComponent<AudioSource>().PlayOneShot(m_dryShotSound);
    }

    private IEnumerator DelayShot()
    {
        m_shootTimer = 0f;
        m_burtCounter++;
        if (m_fireType == ShootingType.SEMI_AUTO)
        {
            canFire = false;
        }

        yield return new WaitForSeconds(m_delayBeforeFire);
        Shoot();
    }

    private IEnumerator DelayLaunch()
    {
        m_shootTimer = 0f;
        m_burtCounter++;
        if (m_fireType == ShootingType.SEMI_AUTO)
        {
            canFire = false;
        }

        yield return new WaitForSeconds(m_delayBeforeFire);
        Launch();
    }

    // Crosshairs
    private void OnGUI()
    {
        if (m_type == WeaponType.PROJECTILE)
        {
            m_currentAccuracy = m_accuracy;
        }

        if (playerWeapon && showCrosshair)
        {
            // Hold the location of the center of the screen in a variable
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);

            // Draw the crosshairs based on the weapon's inaccuracy
            // Left
            Rect leftRect = new Rect(center.x - m_crosshairLength - m_currentCrosshairSize, center.y - (m_crosshairWidth / 2), m_crosshairLength, m_crosshairWidth);
            GUI.DrawTexture(leftRect, m_crosshairTexture, ScaleMode.StretchToFill);
            // Right
            Rect rightRect = new Rect(center.x + m_currentCrosshairSize, center.y - (m_crosshairWidth / 2), m_crosshairLength, m_crosshairWidth);
            GUI.DrawTexture(rightRect, m_crosshairTexture, ScaleMode.StretchToFill);
            // Top
            Rect topRect = new Rect(center.x - (m_crosshairWidth / 2), center.y - m_crosshairLength - m_currentCrosshairSize, m_crosshairWidth, m_crosshairLength);
            GUI.DrawTexture(topRect, m_crosshairTexture, ScaleMode.StretchToFill);
            // Bottom
            Rect bottomRect = new Rect(center.x - (m_crosshairWidth / 2), center.y + m_currentCrosshairSize, m_crosshairWidth, m_crosshairLength);
            GUI.DrawTexture(bottomRect, m_crosshairTexture, ScaleMode.StretchToFill);
        }
    }

    public void WeaponPickup(Transform weaponHolder)
    {
        if (isHeld) return;

        Destroy(m_capsuleCollider);
        //Destroy(m_weaponFloatingFX);
        gameObject.SetActive(false);
        transform.parent = weaponHolder;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        switch (m_name)
        {
            case "Bazooka":
                m_weaponBobScript.m_InitPos = transform.localPosition = new Vector3(.42f, -0.517f, .527f);
                break;
            case "M16":
                m_weaponBobScript.m_InitPos = transform.localPosition = new Vector3(.41f, -.609f, 0.558f);
                break;
            case "Revolver":
                m_weaponBobScript.m_InitPos = transform.localPosition = new Vector3(.305f, -.305f, .474f);
                break;
            case "Shotgun":
                m_weaponBobScript.m_InitPos = transform.localPosition = new Vector3(.26f, -0.31f, 0.34f);
                break;
        }

        m_weaponBobScript.enabled = true;
        playerWeapon = true;
        hasPickedup = true;
        isHeld = true;
        AudioManager.Instance.PlaySound(SoundType.WEAPON_PICKUP);
    }

    public int GetCurrentAmmo()
    {
        return m_currentAmmo;
    }

    public int GetAmmoCapacity()
    {
        return m_ammoCapacity;
    }
}
