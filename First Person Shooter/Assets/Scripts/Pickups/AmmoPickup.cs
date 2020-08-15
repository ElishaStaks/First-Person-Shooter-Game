using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public AmmoTypes m_ammoType;
    public int m_ammoAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
        {
            var gun = FindObjectOfType<Gun>();

            switch (m_ammoType)
            {
                case AmmoTypes.PISTOL:
                    AmmoManager.instance.AddAmmo(gun.m_ammo, m_ammoAmount);
                    Destroy(gameObject);
                    break;
                case AmmoTypes.AUTOMATIC:
                    AmmoManager.instance.AddAmmo(gun.m_ammo, m_ammoAmount);
                    Destroy(gameObject);
                    break;
                case AmmoTypes.SHELLS:
                    AmmoManager.instance.AddAmmo(gun.m_ammo, m_ammoAmount);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
