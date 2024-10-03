using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CowController : MonoBehaviour
{
    [SerializeField] private PoolingAndEnabling _poolingAndEnabling;
    [SerializeField] private LevelSettings _levelSettings;
    
    [SerializeField] private GameObject _cowPrefab;

    private int _poolLenght;

    private List<GameObject> _cowPool;

    float totalLevelTime;

    float timeToFirstSpawn;

    float timeToLastSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        totalLevelTime = _levelSettings.gameLevelSettings.levelDuration;
        _poolLenght = _levelSettings.gameLevelSettings.totalCows;
        timeToFirstSpawn = _levelSettings.cowSettings.timeToSpawnFirstCow;
        timeToLastSpawn = _levelSettings.cowSettings.timeToLastSpawn;
        _cowPool = _poolingAndEnabling.InstantiatePool(_poolLenght,_cowPrefab);
        StartCoroutine(_poolingAndEnabling.ShowRandomTimes(_poolLenght,totalLevelTime, timeToFirstSpawn,timeToLastSpawn,_cowPool));
    }
}
