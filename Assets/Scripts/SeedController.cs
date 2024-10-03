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
    // Start is called before the first frame update
    void Start()
    {
        _poolLenght = _levelSettings.gameLevelSettings.totalSeeds;
        timeToLastSpawn = _levelSettings.seedSettings.timeToLastSpawn;
        percentajeToFirstSeeed = _levelSettings.seedSettings.percentajeToFirstSpawn;
        _seedPool = _poolingAndEnabling.InstantiatePool(_poolLenght, _seedPrefab);
    }

    public void StartSeedSpawn(float percent)
    {
        if (percent > percentajeToFirstSeeed)
        {
            float timeLeft = _levelSettings.gameLevelSettings.levelDuration - Time.time;
            StartCoroutine(_poolingAndEnabling.ShowRandomTimes(_poolLenght,timeLeft, 0f,timeToLastSpawn,_seedPool));
        }
        
    }
}
