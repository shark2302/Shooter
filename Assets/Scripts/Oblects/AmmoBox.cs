using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
   [SerializeField] private int _countAmmoInTheBox;

   private void OnTriggerEnter(Collider other)
   {
       PlayerShoot ps = other.GetComponent<PlayerShoot>();
       if (ps != null)
       {
           ps.AddAmmo(_countAmmoInTheBox);
           gameObject.SetActive(false);
       }
   }
}
