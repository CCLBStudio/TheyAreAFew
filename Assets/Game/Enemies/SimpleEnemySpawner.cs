using System.Collections.Generic;
using CCLBStudio.ScriptablePooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleEnemySpawner : MonoBehaviour
{
    [SerializeField] private ScriptablePool enemyPool;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] float timeBetweenSpawns = .5f;

    private float _spawnTimer;

    private void Start()
    {
        enemyPool.Initialize();
        _spawnTimer = timeBetweenSpawns;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            Spawn();
            _spawnTimer = timeBetweenSpawns;
        }
    }

    private void Spawn()
    {
        var pos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        var obj = enemyPool.RequestObjectAs<EnemyFacade>();
        obj.transform.position = pos;
    }
}
