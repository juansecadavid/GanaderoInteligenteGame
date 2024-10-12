using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private TextMeshProUGUI pointsTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI cowTxt;
    [SerializeField] private TextMeshProUGUI seedTxt;
    private float timeRemaining;
    public int CowAmount { get; private set; }
    public int SeedAmount { get; private set; }

    public void Initialize()
    {
        PointsSystem.pointsChanged += UpdatePoints;
        Cow.cowAmount += UpdateCowAmount;
        Seed.seedCounter += UpdateSeedAmount;
        CowAmount = 0;
        SeedAmount = 0;
        timeRemaining = _levelSettings.gameLevelSettings.levelDuration;
        if (pointsTxt != null) pointsTxt.text = $"{PointsSystem.Points}";
        StartCoroutine(LevelTimer());
    }

    void UpdateSeedAmount(int amount)
    {
        SeedAmount += amount;
        seedTxt.text = $"{SeedAmount}";
    }
    
    void UpdatePoints(int newPoints)
    {
        pointsTxt.text = $"{newPoints}";
    }

    void UpdateCowAmount(int amount)
    {
        CowAmount += amount;
        cowTxt.text = $"{CowAmount}";
    }
    

    IEnumerator LevelTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            
            timeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Esperar hasta el siguiente frame
            yield return null;
        }
        
        timeTxt.text = "00:00";
    }

    public void Conclude()
    {
        PointsSystem.pointsChanged -= UpdatePoints;
        Cow.cowAmount -= UpdateCowAmount;
        Seed.seedCounter -= UpdateSeedAmount;
        
        StopAllCoroutines();
    }
}
