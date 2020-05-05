using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersController : MonoBehaviour
{
    [SerializeField] private Spawner[] _spawners;
    private int _indexOfCurSpawner;
    private List<GameObject> _allSpawned;
    private void OnEnable()
    {
        _allSpawned = new List<GameObject>();
        _indexOfCurSpawner = 0;
        for (int i = 0; i < _spawners.Length; i++)
        {
            if (i != _indexOfCurSpawner)
            {
                _spawners[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        _allSpawned.Clear();
    }

    public void DestroyAll()
    {
        foreach (var unit in _allSpawned)
        { 
            if(unit != null)
                Destroy(unit);
        }
    }

    public List<GameObject> GetAllSpawnedUnits()
    {
        return _allSpawned;
    }

    public void MakeAllActive()
    {
        foreach (var spawner in _spawners)
        {
            if (!spawner.gameObject.active)
            {
                spawner.gameObject.SetActive(true);
            }
            spawner.SetEndGameMode(true);
        }
    }

    public void MakeActiveNextSpawner()
    {
        if (_indexOfCurSpawner != _spawners.Length - 1)
        {
            _indexOfCurSpawner++;
            _spawners[_indexOfCurSpawner].gameObject.SetActive(true);
        }
    }

    public void SetPlayers(GameObject[] players)
    {
        foreach (var spawner in _spawners)
        {
            spawner.SetPlayers(players);
        }
    }
}
