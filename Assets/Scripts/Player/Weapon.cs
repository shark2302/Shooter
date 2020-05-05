using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField] private String _name;
    [SerializeField] private float _range;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    public String GetName()
    {
        return _name;
    }

    public float GetRange()
    {
        return _range;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public float GetFireRate()
    {
        return _fireRate;
    }
}
