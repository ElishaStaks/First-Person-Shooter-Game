using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSystem : MonoBehaviour
{
    [Header("System")]
    public List<GameObject> m_weapons = new List<GameObject>();                // List that holds all the weapons the player can carry
    public int m_startingIndex = 0;                                            // Weapon index the player will start with

    [Header("Pick Up")]
    public float m_pickupRange;

    public List<TextMeshProUGUI> m_ammoText = new List<TextMeshProUGUI>();
    //public TextMeshProUGUI m_ammoText;

    private int m_currentWeaponIndex;                                          // Current index of active weapon
    private Transform m_playerCamera;
    private Weapon m_weapon;

    private void Awake()
    {
        m_playerCamera = Camera.main.transform;
        // Starting weapon is the first indexed weapon
        m_currentWeaponIndex = m_startingIndex;
        SetActiveWeapon(m_currentWeaponIndex);

        for (int i = 0; i < m_ammoText.Count; i++)
        {
            m_ammoText[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CastPickup();

        // Allow the user to instantly switch to any weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetActiveWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetActiveWeapon(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            SetActiveWeapon(4);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            SetActiveWeapon(5);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            SetActiveWeapon(6);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            SetActiveWeapon(7);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            SetActiveWeapon(8);

        // Allow the user to scroll through the weapons
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            NextWeapon();
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            PreviousWeapon();
    }

    // Sets the weapon based on index number
    public void SetActiveWeapon(int index)
    {
        // Checks if the weapon index is valid 
        if (index >= m_weapons.Count || index < 0)
        {
            return;
        }

        // Assigns new index to the current weapon index
        m_currentWeaponIndex = index;

        // deactivates all weapons
        for (int i = 0; i < m_weapons.Count; i++)
        {
            m_weapons[i].SetActive(false);
            m_ammoText[index].gameObject.SetActive(false);
            
        }

        // Activates chosen weapon
        m_weapons[index].SetActive(true);
        m_ammoText[index].gameObject.SetActive(true);
    }

    public void SetWeaponOnPickup(GameObject weapon)
    {
        for (int i = 0; i < m_weapons.Count; i++)
        {
            m_weapons[i].gameObject.SetActive(false);
        }

        weapon.gameObject.SetActive(true);
    }

    // Selects the next weapon in the Array of weapons
    public void NextWeapon()
    {
        m_currentWeaponIndex++;
        // Checks if the next index is greater than the weapons array length
        if (m_currentWeaponIndex > m_weapons.Count - 1)
        {
            // Set weapon index back to 0
            m_currentWeaponIndex = 0;
        }

        // Activate next indexed weapon
        SetActiveWeapon(m_currentWeaponIndex);
    }

    // Selects the previous weapon in the Array of weapons
    public void PreviousWeapon()
    {
        m_currentWeaponIndex--;
        if (m_currentWeaponIndex < 0)
        {
            m_currentWeaponIndex = m_weapons.Count - 1;
        }

        SetActiveWeapon(m_currentWeaponIndex);
    }

    public void CastPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(m_playerCamera.position, m_playerCamera.forward, out hit, m_pickupRange))
            {
                var weapon = hit.transform.GetComponent<Weapon>();

                if (weapon != null)
                {
                    weapon.WeaponPickup(transform, m_ammoText);
                    m_weapons.Add(weapon.gameObject);
                    SetWeaponOnPickup(weapon.gameObject);
                }
            }
        }
    }
}
