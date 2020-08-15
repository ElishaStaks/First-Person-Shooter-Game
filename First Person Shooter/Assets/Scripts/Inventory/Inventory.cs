using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Max Number of Item Slots")]
    public const int m_numItemSlots = 3;

    [Header("Inventory")]
    public Image[] m_itemImages = new Image[m_numItemSlots];
    public Item[] m_items = new Item[m_numItemSlots];
    public int[] m_ammoAmount = new int[m_numItemSlots];

    public void AddItem(Item item)
    {
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i] == null)
            {
                m_items[i] = item;
                m_itemImages[i].sprite = item.m_sprite;
                m_itemImages[i].enabled = true;
                m_ammoAmount[i] = item.m_ammo;
                return;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i] == item)
            {
                m_items[i] = null;
                m_itemImages[i].sprite = null;
                m_itemImages[i].enabled = false;
                m_ammoAmount[i] = 0;
                return;
            }
        }
    }
}
