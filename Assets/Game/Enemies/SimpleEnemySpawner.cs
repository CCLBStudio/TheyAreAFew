using System.Collections.Generic;
using CCLBStudio.ScriptablePooling;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SimpleEnemySpawner : MonoBehaviour
{
    [FormerlySerializedAs("enemyPool")] [SerializeField] private List<ScriptablePool> enemyPools;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] float timeBetweenSpawns = .5f;
    [Tooltip("Number of entities to spawn. 0 for infinite spawn.")]
    [SerializeField] private int spawnCount = 0;

    private float _spawnTimer;
    private int _spawned;

    private void Start()
    {
        foreach (var p in enemyPools)
        {
            p.Initialize();
        }
        _spawnTimer = timeBetweenSpawns;
    }

    private void Update()
    {
        if (spawnCount > 0 && _spawned >= spawnCount)
        {
            return;
        }
        
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            Spawn();
            _spawnTimer = timeBetweenSpawns;
        }
    }

    private void Spawn()
    {
        _spawned++;
        var pos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        var obj = enemyPools[Random.Range(0, enemyPools.Count)].RequestObjectAs<EnemyFacade>();
        obj.transform.position = pos;
    }
}
