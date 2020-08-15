using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void AddAmmo(int gunAmmo, int ammoToGive)
    {
        gunAmmo += ammoToGive;
    }
}