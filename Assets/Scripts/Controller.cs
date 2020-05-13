using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    private GameObject _player;
    private GameObject _bot0;
    private GameObject _boss;
    

    [SerializeField] private SpawnersController _spawners;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _holdPanel;
    [SerializeField] private Text _text;
    [SerializeField] private UnityAction _endGame;
    private bool _playerIsDeath;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _botPrefab;
    [SerializeField] private Transform _playerSpot;
    [SerializeField] private Transform _bot0spot;
    [SerializeField] private Transform _bossSpot;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _bossPrefab;
    private void Start()
    {
        _endGame += ShowEndGameMenu;
        _playerIsDeath = false;
    }

    public void OnStartButton()
    {
        _menuPanel.SetActive(false);
        StartGame();
    }
    public void StartGame()
    {
        _player = Instantiate(_playerPrefab, _playerSpot.position, Quaternion.identity);
        _player.GetComponent<PlayerController>().SetContoller(this);
        _bot0 = Instantiate(_botPrefab, _bot0spot.position, Quaternion.identity);
        _boss = Instantiate(_bossPrefab, _bossSpot.position, Quaternion.identity);
        Enemy b0 = _bot0.GetComponent<Enemy>();
        b0.SetMainPlayer(_player);
        b0.SetMainTarget(_target);
        b0.SetSpawners(_spawners);
        _boss.GetComponent<Boss>()?.SetTarget(_player);
        _spawners.gameObject.SetActive(true);
        _spawners.SetPlayers(new[] {_player, _bot0});
        _target.SetActive(true);
    }

    public void ShowEndGameMenu()
    {
        StartCoroutine(EndGameCoroutine());
    }

    public IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        if(_player != null)
            Destroy(_player);
        if(_bot0 != null)
            Destroy(_bot0);
        if(_boss != null)
            Destroy(_boss);
        _spawners.GetComponent<SpawnersController>().DestroyAll();
        _spawners.gameObject.SetActive(false);
        _target.SetActive(false);
        _endGamePanel.SetActive(true);
        _holdPanel.SetActive(false);
        if (_playerIsDeath)
        {
            _text.text = "Вы погибли!";
        }
        else
        {
            _text.text = "Победа";
        }
    }

    public UnityAction GetEndGameAction(bool playerDeath)
    {
        _playerIsDeath = playerDeath;
        return _endGame;
    }

    public void SetPlayerDeath(bool playerIsDeath)
    {
        _playerIsDeath = playerIsDeath;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        _endGamePanel.SetActive(false);
        StartGame();
    }
    
}
