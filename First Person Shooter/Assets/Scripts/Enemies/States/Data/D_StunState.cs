using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStunStateData", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float m_stunTime = 2f;
    public float m_stunResistance = 3f;
    public float m_stunRecoveryTime = 2f;
}
