using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


    public class Spawner : MonoBehaviour
    {
        private Coroutine _spawnRoutine;
        [SerializeField] private GameObject _prefab;
        private GameObject[] _players;
        [SerializeField] private int _maxUnitsToSpawn;
        [SerializeField] private SpawnersController _spawnersController;
        private bool _endGameMode;
        private List<GameObject> _spawnedUnits;
        private int _counter;
        private void OnEnable()
        {
            _spawnedUnits = new List<GameObject>();
            _counter = 0;
            _endGameMode = false;
        }

        private void OnDisable()
        {
            _spawnedUnits.Clear();
            if (_spawnRoutine != null)
                StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }

        private void FixedUpdate()
        {
            if (_players == null)
                return;
            if (_players != null && _spawnRoutine == null)
            {
                _spawnRoutine = StartCoroutine(SpawnRoutine());
            }
                
            foreach (var player in _players)
            {
                if (player == null)
                    continue;
                if (!_endGameMode && Vector3.Distance(transform.position, player.transform.position) < 10)
                {
                    _spawnersController.MakeActiveNextSpawner();
                    gameObject.SetActive(false);
                }
            }
            
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(3);
            while (_counter < _maxUnitsToSpawn)
            { 
                GameObject en = Instantiate(_prefab, transform.position, Quaternion.identity);
                en.GetComponent<Enemy>().SetPlayers(_players);
                _spawnersController.GetAllSpawnedUnits().Add(en);
                _counter++;
                yield return new WaitForSeconds(3);
            }
        }


        public void SetEndGameMode(bool b)
        {
            _endGameMode = b;
        }

        public void SetPlayers(GameObject[] players)
        {
            _players = players;
        }
    }
