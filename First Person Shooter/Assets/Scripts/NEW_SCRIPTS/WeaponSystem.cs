using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("System")]
    public GameObject[] m_weapons;                // Array that holds all the weapons the player can carry
    public int m_startingIndex = 0;               // Weapon index the player will start with

    private int m_currentWeaponIndex;             // Current index of active weapon

    private void Awake()
    {
        // Starting weapon is the first indexed weapon
        m_currentWeaponIndex = m_startingIndex;
        SetActiveWeapon(m_currentWeaponIndex);
    }

    private void Update()
    {
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
        if (index >= m_weapons.Length || index < 0)
        {
            return;
        }

        // Assigns new index to the current weapon index
        m_currentWeaponIndex = index;

        // deactivates all weapons
        for (int i = 0; i < m_weapons.Length; i++)
        {
            m_weapons[i].SetActive(false);
        }

        // Activates chosen weapon
        m_weapons[index].SetActive(true);
    }

    // Selects the next weapon in the Array of weapons
    public void NextWeapon()
    {
        m_currentWeaponIndex++;
        // Checks if the next index is greater than the weapons array length
        if (m_currentWeaponIndex > m_weapons.Length - 1)
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
            m_currentWeaponIndex = m_weapons.Length - 1;
        }

        SetActiveWeapon(m_currentWeaponIndex);
    }
}
