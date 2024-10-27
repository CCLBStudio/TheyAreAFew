using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform GroundContainer => groundContainer;
    public Transform MovingObjectsContainer => movingObjectsContainer;
    public Transform BandsContainer => bandsContainer;

    [Header("Containers")]
    [SerializeField] private Transform groundContainer;
    [SerializeField] private Transform movingObjectsContainer;
    [SerializeField] private Transform bandsContainer;
    
    [Header("Ground Settings")]
    [SerializeField] private List<GameObject> groundPrefabs;
    [SerializeField] private int groundWidthCount = 50;
    [SerializeField] private int groundDepthCount = 1;
    [SerializeField] private Vector3 spaceBetweenGroundBlocks = new (1f, 0f, 1f);
    [SerializeField] private Vector3 groundCenter;

    [Header("Moving Objects Settings")]
    [SerializeField] private List<GameObject> movingObjectPrefabs;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = .5f;
    [SerializeField] private int movingObjectDepthCount = 5;
    [SerializeField] private List<int> availableChannels;
    [SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    
    [Header("Moving Objects Settings")]
    [SerializeField] private List<GameObject> bandLevelPrefabs;
    [SerializeField] private Vector3 bandsCenter;
    [SerializeField] private int bandLevelCount = 7;
    [SerializeField] private float spaceBetweenBands = 4f;

    public void BuildLevel()
    {
        ClearAll();
        SpawnGround();
        SpawnMovingObjects();
        SpawnBandLevels();
    }

    private void SpawnGround()
    {
        float sideWidth = groundWidthCount / 2f;
        float sideDepth = groundDepthCount / 2f;
        
        Vector3 startPoint = groundCenter;
        startPoint.x -= sideWidth;
        startPoint.z -= sideDepth;

        groundContainer = new GameObject("Ground Container").transform;
        groundContainer.SetParent(transform, false);
        

        for (int i = 0; i < groundDepthCount; i++)
        {
            for (int j = 0; j < groundWidthCount; j++)
            {
                Vector3 pos = startPoint;
                pos.x += j * spaceBetweenGroundBlocks.x;
                pos.z += i * spaceBetweenGroundBlocks.z;

                var prefab = groundPrefabs[Random.Range(0, groundPrefabs.Count)];
                var obj = Instantiate(prefab, groundContainer);
                obj.transform.position = pos;
            }
        }
    }

    private void SpawnMovingObjects()
    {
        float sideWidth = groundWidthCount / 2f;
        float sideDepth = groundDepthCount / 2f;
        
        Vector3 startPoint = groundCenter;
        startPoint.x -= sideWidth;
        startPoint.z += sideDepth + 1f;
        
        movingObjectsContainer = new GameObject("Moving Objects Container").transform;
        movingObjectsContainer.SetParent(transform, false);
        
        for (int i = 0; i < movingObjectDepthCount; i++)
        {
            for (int j = 0; j < groundWidthCount; j++)
            {
                if (Random.Range(0f, 1f) > spawnChance)
                {
                    continue;
                }
                
                Vector3 pos = startPoint;
                pos.x += j * spaceBetweenGroundBlocks.x;
                pos.z += i * spaceBetweenGroundBlocks.z;

                var prefab = movingObjectPrefabs[Random.Range(0, movingObjectPrefabs.Count)];
                var obj = Instantiate(prefab, movingObjectsContainer);
                obj.transform.position = pos;

                if(!obj.TryGetComponent<MMRadioReceiver>(out var receiver))
                {
                    Debug.LogError("No radio receiver on prefab " + prefab.name);
                    continue;
                }

                receiver.Channel = availableChannels[Random.Range(0, availableChannels.Count)];
                receiver.CanListen = true;
                receiver.RandomizeLevel = true;

                float normalizedForward = (i + 1) / (float)movingObjectDepthCount;
                float rand = Random.Range(minRandom, maxRandom) * normalizedForward;
                receiver.MinRandomLevelMultiplier = rand;
                receiver.MaxRandomLevelMultiplier = rand;
            }
        }
    }

    private void SpawnBandLevels()
    {
        float sideWidth = bandLevelCount / 2f;
        Vector3 startPoint = bandsCenter;
        startPoint.x -= sideWidth * spaceBetweenBands;
        var audioAnalyzer = FindAnyObjectByType<MMAudioAnalyzer>();
        
        bandsContainer = new GameObject("Bands Container").transform;
        bandsContainer.SetParent(transform, false);

        for (int i = 0; i < bandLevelCount; i++)
        {
            Vector3 pos = startPoint;
            pos.x += i * spaceBetweenBands;
            
            var prefab = bandLevelPrefabs[Random.Range(0, bandLevelPrefabs.Count)];
            var obj = Instantiate(prefab, bandsContainer);
            obj.transform.position = pos;
            
            if(!obj.TryGetComponent<FloatController>(out var c))
            {
                Debug.LogError("No float controller on prefab " + prefab.name);
                continue;
            }

            c.AudioAnalyzer = audioAnalyzer;
            c.NormalizedLevelID = i;
        }
    }

    private void ClearAll()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
