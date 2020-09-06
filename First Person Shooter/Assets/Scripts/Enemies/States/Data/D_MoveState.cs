using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data container that saves large amounts of data independent of class instances
[CreateAssetMenu(fileName = "NewMoveStateData", menuName = "Data/State Data/Move State")]
public class D_MoveState : ScriptableObject
{
    public float m_movementSpeed = 5f;
    public float m_turnSpeed = 3f;
}
