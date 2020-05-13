using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] private GameObject _grenade;
    [SerializeField] private Transform _hands;
    [SerializeField] private float _force;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }
    
    public void ThrowGrenade()
    {
        GameObject g = Instantiate(_grenade, _hands.position, Quaternion.identity);
        g.GetComponent<Rigidbody>().AddForce(_hands.forward * _force, ForceMode.Impulse);
    }
}
