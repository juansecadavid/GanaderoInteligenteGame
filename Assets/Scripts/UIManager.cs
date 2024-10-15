using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private TextMeshProUGUI pointsTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI cowTxt;
    [SerializeField] private TextMeshProUGUI seedTxt;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LooseScreen;
    public static Action<bool> OnGameEnded;
    private float timeRemaining;
    private int totalCows = 0;
    private int currentPoints;
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
        LevelSettings.GameLevelSettings gameLevelSettings= _levelSettings.gameLevelSettings;
        totalCows = gameLevelSettings.totalCommonCows + gameLevelSettings.totalHungryCows +
                        gameLevelSettings.totalSpecialCows;
        if (pointsTxt != null) pointsTxt.text = "0";
        StartCoroutine(LevelTimer());
    }

    void UpdateSeedAmount(int amount)
    {
        SeedAmount += amount;
        seedTxt.text = $"{SeedAmount}";
    }
    
    void UpdatePoints(int newPoints)
    {
        currentPoints = newPoints;
        pointsTxt.text = $"{newPoints}";
    }

    void UpdateCowAmount(int amount)
    {
        CowAmount += amount;
        cowTxt.text = $"{CowAmount}";
        if(CowAmount>=totalCows)
            OnGameEnded?.Invoke(true);
    }

    public void ShowResult(bool hasWon)
    {
        if (hasWon)
        {
            WinScreen.SetActive(true);
            WinScreen.transform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>().text = $"Score\n{currentPoints}";
            
        }
        else
        {
            LooseScreen.SetActive(true); 
            LooseScreen.transform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>().text = $"Score\n{currentPoints}";
        }
    }

    IEnumerator LevelTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            
            timeTxt.text = string.Format("{0:0}:{1:00}", minutes, seconds);

            // Esperar hasta el siguiente frame
            yield return null;
        }
        
        timeTxt.text = "0:00";
        OnGameEnded?.Invoke(false);
    }

    public void Conclude()
    {
        PointsSystem.pointsChanged -= UpdatePoints;
        Cow.cowAmount -= UpdateCowAmount;
        Seed.seedCounter -= UpdateSeedAmount;
        
        StopAllCoroutines();
    }
}
