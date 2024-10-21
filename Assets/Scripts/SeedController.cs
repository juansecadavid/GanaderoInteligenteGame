using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    [SerializeField] private PoolingAndEnabling _poolingAndEnabling;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private GameObject _seedPrefab;

    private int _poolLenght;
    private List<GameObject> _seedPool;
    private float percentajeToFirstSeeed;
    float timeToLastSpawn;
    private float timeLeft;

    public List<GameObject> SeedPool {  get { return _seedPool; } }
    
    public void Initialize()
    {
        _poolLenght = _levelSettings.gameLevelSettings.totalSeeds;
        timeToLastSpawn = _levelSettings.seedSettings.timeToLastSpawn;
        percentajeToFirstSeeed = _levelSettings.seedSettings.percentajeToFirstSpawn;
        _seedPool = _poolingAndEnabling.InstantiatePool(_poolLenght, _seedPrefab);
        UIManager.timeLeft += UpdateTimeLeft;
        foreach (GameObject seed in _seedPool)
        {
            seed.GetComponent<Seed>().Initialize();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartSeedSpawn();
        }
    }

    public void StartSeedSpawn()
    {
        print("Llamé a las seed");
        StartCoroutine(_poolingAndEnabling.ShowRandomTimes(_poolLenght,timeLeft, 0f,timeToLastSpawn,_seedPool));
    }

    private void UpdateTimeLeft(float timeLeft)
    {
        this.timeLeft = timeLeft;
    }

    public void Conclude()
    {
        foreach (GameObject seed in _seedPool)
        {
            seed.GetComponent<Seed>().Conclude();
        }
    }
}
