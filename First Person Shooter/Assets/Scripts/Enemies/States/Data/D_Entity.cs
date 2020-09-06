using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewEntityData", menuName ="Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float m_closeRangeDistance = 2f;
    public float m_attackDamage = 10f;
    public float m_gravity = -9.81f;

    public LayerMask whatIsPlayer;
}
