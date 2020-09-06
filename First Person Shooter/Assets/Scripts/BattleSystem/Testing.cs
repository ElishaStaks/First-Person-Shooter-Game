/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private BattleSystem battleSystem;

    private void Start()
    {
        battleSystem.OnBattleStarted += BattleSystem_OnBattleStarted;
        battleSystem.OnBattleOver += BattleSystem_OnBattleOver;
    }

    private void BattleSystem_OnBattleOver(object sender, System.EventArgs e) {
        Debug.Log("Battle Over");
    }

    private void BattleSystem_OnBattleStarted(object sender, System.EventArgs e) {
        Debug.Log("Battle Started");
    }
}
