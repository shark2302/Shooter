using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
  
    [SerializeField] private float _health;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _healthBar?.SetMaxHealth(_health);
    }

    public float GetHP()
    {
        return _health;
    }

    public void SetHp(float hp)
    {
        _health = hp;
    }

    public void Heel(float heel)
    {
        _health += heel;
        if (_health > 100)
            _health = 100;
        _healthBar?.SetHealth(_health);
    }

    public void GetDamage(float _damage)
    {
        _health -= _damage;
        _healthBar?.SetHealth(_health);
        if (_health <= 0)
        {
            if(gameObject.tag == "Player")
                Destroy(gameObject, 1.5f);
            else if(_animator != null)
            {
                _animator.SetTrigger("Die");
                Destroy(gameObject, 1.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
