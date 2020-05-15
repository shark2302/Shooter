using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    [SerializeField] private float _armour;
    [SerializeField] private HealthBar _armourBar;


    private void OnEnable()
    {
        _armourBar.gameObject.SetActive(true);
        _armourBar?.SetMaxHealth(_armour);
    }

    public float GetArmour()
    {
        return _armour;
    }

    public void SetArmour(float arm)
    {
        _armour = arm;
    }

    public void HeelArmour()
    {
        _armour = 100;
    }
    
    

    public void GetDamage(float _damage)
    {
        _armour -= _damage;
        _armourBar?.SetHealth(_armour);
        if (_armour <= 0)
        {
            _armour = 100;
            _armourBar.gameObject.SetActive(false);
            enabled = false;
        }
    }
}
