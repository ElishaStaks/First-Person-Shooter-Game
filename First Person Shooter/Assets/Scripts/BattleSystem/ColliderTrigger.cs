/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour {

    public event EventHandler OnPlayerEnterTrigger;

    private void OnTriggerEnter(Collider collider) {
        PlayerManager player = collider.GetComponent<PlayerManager>();
        if (player != null) {
            // Player inside trigger area!
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }

}
