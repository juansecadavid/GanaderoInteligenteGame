using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    public static int Points { get; private set; }
    public static Action<int> pointsChanged;

    public void Initialize()
    {
        Points = 0;
    }
    public void AddPoints(int amount)
    {
        Points += amount;
        pointsChanged?.Invoke(Points);
    }

    public void ReducePoints(int amount)
    {
        Points += amount;
        pointsChanged?.Invoke(Points);
    }
}
