using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] private GameObject _grenade;
    [SerializeField] private Transform _hands;
    [SerializeField] private float _force;
    [SerializeField] private int _count;
    [SerializeField] private Text _grenadeInfo;

    private void Start()
    {
        _grenadeInfo.text = "Grenade : " + _count;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        if (_count <= 0)
            return;
        GameObject g = Instantiate(_grenade, _hands.position, Quaternion.identity);
        g.GetComponent<Rigidbody>().AddForce(_hands.forward * _force, ForceMode.Impulse);
        _count--;
        _grenadeInfo.text = "Grenade : " + _count;
    }

    public void AddGrenade()
    {
        _count++;
        _grenadeInfo.text = "Grenade : " + _count;
    }
}
