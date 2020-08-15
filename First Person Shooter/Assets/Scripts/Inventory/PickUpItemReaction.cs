using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemReaction : MonoBehaviour
{
    public Item m_item;
    private Inventory m_inventory;

    protected void SpecificInit()
    {
        m_inventory = FindObjectOfType<Inventory>();
    }

    protected void ImmediateReaction()
    {
        m_inventory.AddItem(m_item);
    }
}
