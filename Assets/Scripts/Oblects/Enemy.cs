using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour
{
    
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _range;
    [SerializeField] private float _safeDistance;
    [SerializeField] private GameObject _impact;
    [SerializeField] private float _panicDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    private GameObject _target;
    private GameObject _player;
    [SerializeField] private ParticleSystem _flash;
    [SerializeField] private AudioSource _audio;
    private GameObject[] _players;
    private SpawnersController _spawners;

    private GameObject _mainTarget;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        gameObject.GetComponent<Animator>().GetBehaviour<EnemyIdle>()?.SetEnemy(this);
        gameObject.GetComponent<Animator>().GetBehaviour<IdleBehaviour>()?.SetBot(this);
    }

    public void SetMainPlayer(GameObject player)
    {
        _player = player;
    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

    public float GetSpeed()
    {
        return _runSpeed;
    }

    public float GetRange()
    {
        return _range;
    }

    public float GetSafeDistance()
    {
        return _safeDistance;
    }

    public float GetPanicDistance()
    {
        return _panicDistance;
    }

    public GameObject GetTarget()
    {
        return _target;
    }
    
    public float GetFireRate()
    {
        return _fireRate;
    }

    public float GetWalkSpeed()
    {
        return _walkSpeed;
    }

    public NavMeshAgent GetAgent()
    {
        return _agent;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public GameObject GetMainTarget()
    {
        return _mainTarget;
    }
    public void SetMainTarget(GameObject mainTarget)
    {
        _mainTarget = mainTarget;
    }
    public void SetSpawners(SpawnersController spawners)
    {
        _spawners = spawners;
    }
    
    public void RotateToTarget(Transform transform, GameObject target)
    {
        if (_target != null)
        {
            var deltaRotation = target.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(deltaRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
        }
        if(transform.position.y > 1)
            Destroy(gameObject);
    }

    public void Follow(float speed, GameObject target)
    {
        if (_target != null)
        {
            GetRigidbody().position = GetRigidbody().position +
                                      (target.transform.position -
                                       GetRigidbody().position).normalized * speed *
                                      Time.deltaTime;
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
            new Vector3(_flash.transform.forward.x, _flash.transform.forward.y + 5, _flash.transform.forward.z)
        };
        if (Physics.Raycast(_flash.transform.position, directions[Random.Range(0, 2)], out hit, _range+ 20f))
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
        if (Physics.Raycast(_flash.transform.position, _flash.transform.forward, out hit, _range + 20f))
        {
            if (gameObject.CompareTag("Bot") &&
                (hit.transform.gameObject.layer == 10 || hit.transform.gameObject.layer == 8))
            { 
                return false;
            }
               
            if (gameObject.CompareTag("Enemy") && (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 9))
                return false;
        }
        return true;
    }

    public void SetPlayers(GameObject[] players)
    {
        _players = players;
    }

    public void FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float distance = 10000;
        if (gameObject.CompareTag("Enemy"))
        {
            foreach (var player in _players)
            {
                if(player == null)
                    continue;
                float d = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if (player != null && d < distance)
                {
                    distance = d;
                    nearestEnemy = player;
                }
            }
        }
        else if (gameObject.CompareTag("Bot") && _spawners != null)
        {
            foreach (var enemy in _spawners.GetAllSpawnedUnits())
            {
                if(enemy == null)
                    continue;
                float d = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
                if (enemy != null && d < distance)
                {
                    distance = d;
                    nearestEnemy = enemy;
                }
            }
        }
        _target = nearestEnemy;
    }
    
    
}

