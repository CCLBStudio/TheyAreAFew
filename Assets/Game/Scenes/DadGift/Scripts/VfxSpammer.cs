using System.Collections.Generic;
using UnityEngine;

public class VfxSpammer : MonoBehaviour
{
    public List<GameObject> vfx;
    public float timeBetweenSpawns = .05f;
    public float totalSpawnTime = 5f;
    public Transform aroundTarget;
    public float randomRange = 5f;

    private bool _spawn;
    private float _timer;
    private float _totalTimer;

    private void Update()
    {
        if (!_spawn)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            SpawnVfx();
            _timer = timeBetweenSpawns;
        }

        _totalTimer += Time.deltaTime;
        if (_totalTimer >= totalSpawnTime)
        {
            _spawn = false;
        }
    }

    private void SpawnVfx()
    {
        var pos = aroundTarget.position;
        pos.x += Random.Range(-randomRange, randomRange);
        pos.y += Random.Range(-randomRange, randomRange);
        var fx = vfx[Random.Range(0, vfx.Count)];
        Instantiate(fx, pos, Quaternion.identity).AddComponent<VfxDestroyer>();
        
    }

    public void LaunchSpawn()
    {
        _spawn = true;
        _timer = 0f;
        _totalTimer = 0f;
    }
}
