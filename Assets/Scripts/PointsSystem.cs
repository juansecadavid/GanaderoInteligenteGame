using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    public int Points { get; private set; }
    public static Action<int> pointsChanged;
    public static PointsSystem Instance;
    private Coroutine waitCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        Cow.cowAmount += AddCowPoints;
        Seed.seedCounter += AddSeedPoints;
        TerrainController.OnTerrainPercentageChanged += CheckNumber;
        Points = 0;
    }

    private void AddSeedPoints(int add)
    {
        AddPoints(_levelSettings.pointsSettings.seedPoints);
    }

    private void AddCowPoints(int add)
    {
        AddPoints(_levelSettings.pointsSettings.cowPoints);
    }
    public void AddPoints(int amount)
    {
        Points += amount;
        pointsChanged?.Invoke(Points);
    }
    
    private void CheckNumber(int number)
    {
        if (number >= _levelSettings.pointsSettings.objectivePercentageToGetPoints)
        {
            if (waitCoroutine == null)
            {
                waitCoroutine = StartCoroutine(WaitToAwardPoints());
            }
        }
        else
        {
            StopWaiting();
        }
    }

    private void StopWaiting()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }

    private IEnumerator WaitToAwardPoints()
    {
        yield return new WaitForSeconds(_levelSettings.pointsSettings.TimeToCheckPoints);
        Debug.Log("Pasó la condición, otorgando puntos.");
        // Llamar aquí la función para otorgar puntos
        waitCoroutine = null;
        AddPoints(_levelSettings.pointsSettings.terrainPoints);
    }

    public void Conclude()
    {
        Cow.cowAmount -= AddCowPoints;
        Seed.seedCounter -= AddSeedPoints;
        TerrainController.OnTerrainPercentageChanged -= CheckNumber;
        StopAllCoroutines();
    }
}
