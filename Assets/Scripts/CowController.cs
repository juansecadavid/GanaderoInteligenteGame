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

    private Coroutine _coroutine;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        totalLevelTime = _levelSettings.gameLevelSettings.levelDuration;
        _poolLenght = _levelSettings.gameLevelSettings.totalCows;
        timeToFirstSpawn = _levelSettings.cowSettings.timeToSpawnFirstCow;
        timeToLastSpawn = _levelSettings.cowSettings.timeToLastSpawn;
        _cowPool = _poolingAndEnabling.InstantiatePool(_poolLenght,_cowPrefab);
        foreach (GameObject cow in _cowPool)
        {
            cow.GetComponent<Cow>().Initialize();
        }
        _coroutine = StartCoroutine(_poolingAndEnabling.ShowRandomTimes(_poolLenght,totalLevelTime, timeToFirstSpawn,timeToLastSpawn,_cowPool));
    }

    public void Conclude()
    {
        StopCoroutine(_coroutine);
    }
}
