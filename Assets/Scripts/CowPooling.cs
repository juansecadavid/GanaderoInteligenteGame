using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CowPooling : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    
    [SerializeField] private GameObject _prefab;

    [SerializeField] private int _poolLenght;

    [SerializeField] private List<GameObject> _poolElements;

    float totalLevelTime;

    float timeToFirstSpawn;

    float timeToLastSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        totalLevelTime = _levelSettings.gameLevelSettings.levelDuration;
        _poolLenght = _levelSettings.gameLevelSettings.totalCommonCows;
        timeToFirstSpawn = _levelSettings.cowSettings.timeToSpawnFirstCow;
        timeToLastSpawn = _levelSettings.cowSettings.timeToLastSpawn;
        InstantiatePool();
        StartCoroutine(PrintRandomTimes(_poolLenght,totalLevelTime, timeToFirstSpawn,timeToLastSpawn));
    }

    private void InstantiatePool()
    {
        for (int i = 0; i < _poolLenght; i++)
        {
            GameObject clone = Instantiate(_prefab, transform.parent);
            clone.SetActive(false);
            _poolElements.Add(clone);
        }
    }
    
    IEnumerator PrintRandomTimes(int x, float y, float e, float p)
    {
        // Validación de parámetros
        if (x <= 0 || y <= 0 || e < 0 || p < 0 || e + p >= y)
        {
            Debug.LogError("Los parámetros no son válidos.");
            yield break;
        }
        
        // Generar tiempos aleatorios para las impresiones intermedias
        List<float> times = new List<float>();
        times.Add(e); // Tiempo de la primera impresión
        times.Add(y - p); // Tiempo de la última impresión

        int numMiddlePrints = x - 2; // Número de impresiones intermedias

        for (int i = 0; i < numMiddlePrints; i++)
        {
            float randomTime = Random.Range(e, y - p);
            times.Add(randomTime);
        }

        // Ordenar los tiempos
        times.Sort();

        // Programar las impresiones
        float previousTime = 0f;

        for (int i = 0; i < times.Count; i++)
        {
            float waitTime = times[i] - previousTime;
            yield return new WaitForSeconds(waitTime);
            previousTime = times[i];
            _poolElements[i].SetActive(true);
            Debug.Log("Impresión " + (i + 1) + " en el tiempo " + times[i]);
        }
    }
}
