using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelData")]
public class LevelDataController : ScriptableObject
{
    public int levelIndex;
    public float regerationTime;
    public float regenerationPercentage;
    public float degradationTime;
    public float degradationPercentage;
    public int cowsCount;
    public float feedPercentage;
    public int seedsCount;
    public float seedsRegenerationPercentage;
    public float sowingTime;
}
