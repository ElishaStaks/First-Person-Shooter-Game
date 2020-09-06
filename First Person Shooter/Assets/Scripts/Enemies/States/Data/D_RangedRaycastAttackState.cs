using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedRaycastAttackStateData", menuName = "Data/State Data/Ranged Raycast Attack State")]
public class D_RangedRaycastAttackState : ScriptableObject
{
    public float m_range = 500f;
    public GameObject m_muzzleFlash;
    public LayerMask whatIsPlayer;
}
