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
    [SerializeField] private GameObject _hungryCowPrefab;
    [SerializeField] private GameObject _specialCowPrefab;

    private int _poolLenght;
    private int _hungryCowLenght;
    private int _specialCowLenght;

    private List<GameObject> _cowPool;
    public List<GameObject> CowPool {  get { return _cowPool; } }

    float totalLevelTime;

    float timeToFirstSpawn;

    float timeToLastSpawn;

    private Coroutine _coroutine;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        totalLevelTime = _levelSettings.gameLevelSettings.levelDuration;
        _poolLenght = _levelSettings.gameLevelSettings.totalCommonCows;
        _hungryCowLenght = _levelSettings.gameLevelSettings.totalHungryCows;
        _specialCowLenght = _levelSettings.gameLevelSettings.totalSpecialCows;
        timeToFirstSpawn = _levelSettings.cowSettings.timeToSpawnFirstCow;
        timeToLastSpawn = _levelSettings.cowSettings.timeToLastSpawn;
        _cowPool = _poolingAndEnabling.InstantiatePool(_poolLenght, _hungryCowLenght, _specialCowLenght, _cowPrefab,_hungryCowPrefab,_specialCowPrefab);
        foreach (GameObject cow in _cowPool)
        {
            cow.GetComponent<Cow>().Initialize();
        }
        _coroutine = StartCoroutine(_poolingAndEnabling.ShowRandomTimes(_poolLenght+_hungryCowLenght+_specialCowLenght,totalLevelTime, timeToFirstSpawn,timeToLastSpawn,_cowPool));
    }

    public void Conclude()
    {
        StopCoroutine(_coroutine);
    }
}
