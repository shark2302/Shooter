using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Weapon))]
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    private Weapon _weapon;
    [SerializeField] private ParticleSystem _flash;
    [SerializeField] private GameObject _impact;
    private float _nextTimeToFire = 0f;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private int _ammo;
    [SerializeField] private Text _ammoInfo;
    void Start()
    {
        _weapon = GetComponent<Weapon>();
        if (_cam == null)
        {
            Debug.LogError("No camera");
            enabled = false;
        }

        _ammoInfo.text = "Ammo : " + _ammo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _weapon.GetFireRate();
            Shoot();
        }
    }

    private void Shoot()
    {
        if(_ammo == 0)
            return;
        _flash.Play();
        _audio.Play();
        _ammo--;
        _ammoInfo.text = "Ammo : " + _ammo;
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _weapon.GetRange()))
        {
            GameObject impact = Instantiate(_impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.1f);
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(-hit.normal * 100);
            HP hp = hit.transform.GetComponent<HP>();
            if (hp != null)
            {
                hp.GetDamage(_weapon.GetDamage());
            }
        }
    }

    public void AddAmmo(int addedAmmo)
    {
        _ammo += addedAmmo;
        _ammoInfo.text = "Ammo : " + _ammo;
    }
}
