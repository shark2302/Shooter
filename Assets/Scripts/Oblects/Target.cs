using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class Target : MonoBehaviour
{
    [SerializeField] private SpawnersController _spawners;
    [SerializeField] private float _secondsToCapture;
    [SerializeField] private float _secondsToHold;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _text;
    [SerializeField] private Controller _controller;
    private float _timer;
    private bool _playerInField;
    private bool _captured;
    private bool _holded;
    private UnityEvent _hold;

    private void OnEnable()
    {
        _slider.maxValue = _secondsToCapture;
        _timer = 0;
        _holded = false;
        _captured = false;
        _playerInField = false;
    }

    private void Update()
    {
        if(_holded)
            return;
        if (_timer == _secondsToHold && _captured)
        {
            _holded = true;
            _controller.SetPlayerDeath(false);
            _controller.ShowEndGameMenu();
            gameObject.SetActive(false);
        }
        if (_timer == _secondsToCapture)
        {
            _captured = true;
            _slider.maxValue = _secondsToHold;
            _text.text = "Удержание :";
            _timer = 0;
            _slider.value = 0;
        }
        
        else if (_playerInField && !_captured)
        {
            _timer += Time.deltaTime;
            if (_timer > _secondsToCapture)
                _timer = _secondsToCapture;
            _slider.value = _timer;
        }
        else if (_captured)
        {
            _timer += Time.deltaTime;
            if (_timer > _secondsToHold)
                _timer = _secondsToHold;
            _slider.value = _timer;
        }

      
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _spawners.MakeAllActive();
            _playerInField = true;
            _panel.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _timer = 0f;
            _playerInField = false;
            _panel.SetActive(false);
        }
    }
}
