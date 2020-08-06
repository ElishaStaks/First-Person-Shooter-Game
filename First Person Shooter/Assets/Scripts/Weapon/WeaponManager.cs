using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public float m_pickUpRange;
    public float m_pickUpRadius;
    public int m_weaponLayer;

    public Transform m_weaponHolder;
    public Transform m_playerCamera;
    public TextMeshProUGUI m_ammoText;
    public GameObject m_reticle;

    private bool isWeaponHeld;
    private Weapon heldWeapon;
    private Image m_reticleColour;

    private void Awake()
    {
        m_reticleColour = m_reticle.transform.Find("Reticle").GetComponent<Image>();
        m_reticle.gameObject.SetActive(false);
        m_ammoText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isWeaponHeld)
        {
            RaycastHit hit;
            if (Physics.Raycast(m_playerCamera.position, m_playerCamera.forward, out hit, 1000))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    m_reticleColour.color = Color.green;
                }
                else
                {
                    m_reticleColour.color = Color.white;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                heldWeapon.Drop(m_playerCamera);
                heldWeapon = null;
                isWeaponHeld = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            var hitList = new RaycastHit[256];
            var hitNumber = Physics.CapsuleCastNonAlloc(m_playerCamera.position, m_playerCamera.position + m_playerCamera.forward *
                m_pickUpRange, m_pickUpRadius, m_playerCamera.forward, hitList);

            var realList = new List<RaycastHit>();
            for (int i = 0; i < hitNumber; i++)
            {
                var hit = hitList[i];
                if (hit.transform.gameObject.layer != m_weaponLayer) continue;
                if (hit.point == Vector3.zero)
                {
                    realList.Add(hit);
                }
                else if (Physics.Raycast(m_playerCamera.position, hit.point - m_playerCamera.position, out var hitInfo, hit.distance + .1f) && hitInfo.transform == hit.transform)
                {
                    realList.Add(hit);
                }
            }

            if (realList.Count == 0) return;

            realList.Sort((hit1, hit2) =>
            {
                var dist1 = GetDistance(hit1);
                var dist2 = GetDistance(hit2);
                return Mathf.Abs(dist1 - dist2) < .001 ? 0 : dist1 < dist2 ? -1 : 1;
            });

            isWeaponHeld = true;
            heldWeapon = realList[0].transform.GetComponent<Weapon>();
            heldWeapon.PickUp(m_weaponHolder, m_playerCamera, m_ammoText, m_reticle);
        }
    }

    private float GetDistance(RaycastHit hit)
    {
        return Vector3.Distance(m_playerCamera.position, hit.point == Vector3.zero ? hit.transform.position : hit.point);
    }
}
