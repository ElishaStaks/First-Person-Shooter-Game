using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollState : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetRigidBodyState(true);
        SetColliderState(false);
    }

    public void Die()
    {
        GetComponent<Animator>().enabled = false;
        SetRigidBodyState(false);
        SetColliderState(true);
        Destroy(gameObject, 5f);
    }

    public void SetRigidBodyState(bool state)
    {
        Rigidbody[] rigidBody = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigid in rigidBody)
        {
            rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigid.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    public void SetColliderState(bool state)
    {
        Collider[] collide = GetComponentsInChildren<Collider>();

        foreach (Collider collider in collide)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
        
    }
}
