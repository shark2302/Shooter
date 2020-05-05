using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float lookSensetivity;
    private PlayerMotor _motor;
    private HP _health;
    public UnityEvent Death;
    [SerializeField] private PlayerShoot _shoot;

    void Start()
    {
        _motor = GetComponent<PlayerMotor>();
        _health = GetComponent<HP>();
        _shoot = GetComponent<PlayerShoot>();
    }

   
    void Update()
    {
        if (_health.GetHP() <= 0)
        {
            Death.Invoke();
            _shoot.enabled = false;
        }
        float xMove = Input.GetAxisRaw("Horizontal"); 
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _speed;
        _motor.SetVelocity(velocity);
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensetivity;
        _motor.SetRotation(rotation);
        float xRot = Input.GetAxisRaw("Mouse Y") * 25;
        _motor.SetCameraRotation(xRot); 
    }

    public void SetContoller(Controller controller)
    {
        Death.AddListener(controller.GetEndGameAction(true));
    }
    

}
