using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private float _cameraRotation = 0f;
    private Rigidbody rb;
   
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void SetRotation(Vector3 rotation)
    {
        _rotation = rotation;
    }
    public void SetCameraRotation(float rotation)
    {
        _cameraRotation -= rotation;
        _cameraRotation = Mathf.Clamp(_cameraRotation, -85f, 85f);
    }

    

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        
    }

    private void PerformMovement()
    {
        if (_velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
        }
    }
    private void PerformRotation()
    {
        if (_rotation != Vector3.zero)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
        if (_cam != null)
        { 
            _cam.transform.localRotation = Quaternion.Euler(_cameraRotation, 0,0);
        }
    }

   
    
}
