using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSuicideAttackStateData", menuName = "Data/State Data/Suicide Attack State")]
public class D_SuicideAttackState : ScriptableObject
{
    public float m_attackRadius = 2f;
    public LayerMask whatIsPlayer;
    public GameObject m_explosionPrefab;
}
