using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _timer = 2f;
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;
    [SerializeField] private AudioSource _audio;
    private float _coutdown;

    private bool _hasExploded;
    
    [SerializeField] private GameObject _exploParticle;
    // Start is called before the first frame update
    void Start()
    {
        _coutdown = _timer;
    }

    // Update is called once per frame
    void Update()
    {
        _coutdown -= Time.deltaTime;
        if (_coutdown < 0 && !_hasExploded)
        {
           
            Explode();
        }
    }

    private void Explode()
    {
        _audio.Play();
        GameObject part = Instantiate(_exploParticle, transform.position, Quaternion.identity);
        Destroy(part, 0.7f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var coll in colliders)
        {
            HP hp = coll.GetComponent<HP>();
            if(coll.CompareTag("Enemy") && hp != null)
            {
                hp.GetDamage(_damage);
            }
        }

        _hasExploded = true;
        Destroy(gameObject, 1);
    }
    
}
