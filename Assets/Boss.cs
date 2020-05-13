using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    
    [SerializeField] private float _walkSpeed;
    [SerializeField] private GameObject _impact;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private ParticleSystem _flash;
    private GameObject _player;
    [SerializeField] private AudioSource _audio;
    private Vector3[] _spots;
    private float waitTime;
    private Vector3 _curSpot;
    private float _nextTimeToFire;
    private bool _active = false;
    private Rigidbody rb;
    private void Start()
    {
            rb = GetComponent<Rigidbody>();
            waitTime = 2;
            Vector3 pos = transform.position;
            _spots = new[]
            {
                new Vector3(pos.x, pos.y, pos.z + 5), new Vector3(pos.x + 5, pos.y, pos.z),
                new Vector3(pos.x - 5, pos.y, pos.z), new Vector3(pos.x, pos.y, pos.z - 5),
                new Vector3(pos.x - 5, pos.y, pos.z - 5), new Vector3(pos.x + 5, pos.y, pos.z + 5)
            };
            _curSpot = _spots[Random.Range(0, _spots.Length - 1)];
    }

    private void FixedUpdate()
    {
        if(_player == null)
            return;
        if (Vector3.Distance(transform.position, _player.transform.position) < 15)
            _active = true;
        if(!_active)
            return;
        RotateToTarget();
        rb.position = Vector3.MoveTowards(transform.position, _curSpot, _walkSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, _curSpot) < 0.5f || waitTime < 0)
        {
            _curSpot = _spots[Random.Range(0, _spots.Length - 1)];
            waitTime = 2;
        }
        waitTime -= Time.fixedDeltaTime;
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
        
    }

    public void RotateToTarget()
    {
        if (_player != null)
        {
            var deltaRotation = _player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(deltaRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }
    public void Shoot()
    {
        _flash.Play();
        _audio.Play();
        RaycastHit hit;
        Vector3[] directions =
        {
            _flash.transform.forward,
            new Vector3(_flash.transform.forward.x, _flash.transform.forward.y - 0.3f, _flash.transform.forward.z)
        };
        if (Physics.Raycast(_flash.transform.position, directions[Random.Range(0, 2)], out hit, 200f))
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
    
    public bool CanShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_flash.transform.position, _flash.transform.forward, out hit, 200f))
        {
            if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 9)
                return false;
        }
        return true;
    }
    
    
    
    public void SetTarget(GameObject player)
    {
        _player = player;
    }
}
