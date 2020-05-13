using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtPack : MonoBehaviour
{
    [SerializeField] private float _heel;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            HP playerHP = other.GetComponent<HP>();
            if(playerHP.GetHP() == 100f) 
                return;
            playerHP.Heel(_heel);
            Destroy(gameObject);
        }
    }
}
