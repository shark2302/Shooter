using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private ParticleSystem _flash;
    [SerializeField] private GameObject _impact; 
    private float _nextTimeToFire = 0f;
    [SerializeField] private float fireRate;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private float _damage;
    private GameObject _target;
    private bool rotatingRight = true;
    private int _scatter;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (_target != null && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / fireRate;
            if(rotatingRight)
                transform.Rotate(0,2,0);
            else
                transform.Rotate(0, -2, 0);
            Shoot();
            if (transform.rotation.y > 5)
                rotatingRight = false;
            if (transform.rotation.y <= 0)
                rotatingRight = true;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_target == null && (other.CompareTag("Player") || other.CompareTag("Player")))
        {
            _target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _target)
            _target = null;
    }

    public void Shoot()
    {
        _flash.Play();
        _audio.Play();
        RaycastHit hit;
        Vector3 dir = new Vector3(_flash.transform.forward.x, _flash.transform.forward.y - 0.01f, _flash.transform.forward.z);
        if (Physics.Raycast(_flash.transform.position, 
            dir, out hit, 200f))
        {
            GameObject impact = Instantiate(_impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.1f);
            HP hp = hit.transform.GetComponent<HP>();
            Armour armour = hit.transform.GetComponent<Armour>();
            if (armour != null && armour.enabled)
            {
                armour.GetDamage(_damage);
                return;
            }
            if (hp != null && hit.transform.tag != transform.tag)
            {
                hp.GetDamage(_damage);
                
            }
        }

    }
}
