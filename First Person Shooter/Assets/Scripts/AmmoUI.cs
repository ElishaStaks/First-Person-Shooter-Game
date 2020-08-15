using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI m_ammoText;
    private Gun m_gun;

    public void SetUpAmmoUI(Gun gun)
    {
        m_gun = gun;
        m_gun.OnAmmoChanged += Instance_OnAmmoChanged;
        UpdateAmmoUI();
    }

    private void Instance_OnAmmoChanged(object sender, System.EventArgs e)
    {
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        m_ammoText.text = m_gun.m_ammo.ToString() + " | " + m_gun.m_maxAmmo.ToString();
    }
}
