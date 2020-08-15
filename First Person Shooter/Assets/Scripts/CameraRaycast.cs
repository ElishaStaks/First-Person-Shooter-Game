using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public float m_pickUpRange;
    public float m_pickUpRadius;
    private IPickupable m_currentTarget;

    private void Update()
    {
        Raycast();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (m_currentTarget != null)
            {
                m_currentTarget.OnInteract();
            }
        }
    }

    private void Raycast()
    {
        RaycastHit hit;

        if (Physics.CapsuleCast(transform.position, transform.position + transform.forward * m_pickUpRange, 0.25f, transform.forward, out hit))
        {
            IPickupable item = hit.collider.GetComponent<IPickupable>();

            if (item != null)
            {
                if (item == m_currentTarget)
                {
                    return;
                }
                else if (m_currentTarget != null)
                {
                    m_currentTarget.OnEnd();
                    m_currentTarget = item;
                    m_currentTarget.OnStart();
                }
                else
                {
                    m_currentTarget = item;
                    m_currentTarget.OnStart();
                }    
            }
            else
            {
                if (m_currentTarget != null)
                {
                    m_currentTarget.OnEnd();
                    m_currentTarget = null;
                }
            }
        }
        else
        {
            if (m_currentTarget != null)
            {
                m_currentTarget.OnEnd();
                m_currentTarget = null;
            }
        }
    }
}
