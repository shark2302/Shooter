﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Armour armour = other.GetComponent<Armour>();
        if (armour != null)
        {
            if(!armour.enabled)
                armour.enabled = true;
            else 
                armour.HeelArmour();
            gameObject.SetActive(false);
        }
    }
}
