using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CowController _cowController;

    [SerializeField] private TerrainController _terrainController;

    [SerializeField] private SeedController _seedController;

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private LevelSettings _levelSettings;

    [SerializeField] private PointsSystem _pointsSystem;

    [SerializeField] private UIManager _uiManager;

    [SerializeField] private EnemyFarmersController enemyFarmersController;

    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private bool hasFarmers;

    [SerializeField] private bool isFirstLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (!isFirstLevel)
        {
            Initialize();
        }
        
    }

    public void EatTerrain(float percent,GameObject cow)
    {
        //IMplementar que se baje el terreno
        _terrainController.UpdateTerrain(-percent);
        //print($"Comer {percent}% de terreno");
    }

    public void IncreaseRegeneration(float regenerationPercentajeToIncrease)
    {
        //print("IncreaseRegeneration: " + regenerationPercentajeToIncrease);
        _terrainController.UpdateRegenerationRate(regenerationPercentajeToIncrease);
    }

    IEnumerator CheckCowForSeed()
    {
        int cowA = _levelSettings.seedSettings.cowsToFirstSpawn;
        while (_uiManager.CowAmount<cowA)
        {
            yield return null;
        }
        
        _seedController.StartSeedSpawn();
    }

    IEnumerator CheckTerrainPercentage()
    {
        while (_terrainController.GetCurrentTerrainPercentage()<_levelSettings.pointsSettings.objectivePercentageToGetPoints)
        {
            //print($"Estoy en el bucle {_terrainController.GetCurrentTerrainPercentage()}");
            yield return null;
        }
        //print("Salí del bucle");

        _seedController.StartSeedSpawn();
    }

    public void Initialize()
    {
        StartCoroutine(WaitingBeforeStart());
    }

    IEnumerator CheckFarmerCow()
    {
        int cowA = _levelSettings.farmerSettings.cowAmountNeed;
        while (_uiManager.CowAmount<cowA)
        {
            yield return null;
        }
        
        enemyFarmersController.Initialize(0);
    }
    
    IEnumerator CheckFarmerSeed()
    {
        int seedA = _levelSettings.farmerSettings.seedAmountNeed;
        while (_uiManager.SeedAmount < seedA)
        {
            yield return null;
        }
        
        enemyFarmersController.Initialize(1);
    }

    IEnumerator CheckTerrainRegeneration()
    {
        while (_uiManager.SeedAmount <= 0)
        {
            yield return null;
        }

        _terrainController.EnableTerrainRegenaration(true);
    }

    IEnumerator WaitingBeforeStart()
    {
        _terrainController.Initialize();
        yield return new WaitForSeconds(3);
        
        Cow.eatTerrain += EatTerrain;
        Seed.increaseRegeneration += IncreaseRegeneration;
        UIManager.OnGameEnded += CheckVictory;
        _cowController.Initialize();
        _seedController.Initialize();
        _playerController.Initialize();
        _pointsSystem.Initialize();
        _uiManager.Initialize();
        _audioManager.Initialize();
        //StartCoroutine(CheckTerrainPercentage());
        StartCoroutine(CheckTerrainRegeneration());
        StartCoroutine(CheckCowForSeed());
        if (hasFarmers)
        {
            StartCoroutine(CheckFarmerCow());
            StartCoroutine(CheckFarmerSeed());
        }
    }

    public void CheckVictory(bool reachedCow)
    {
        Conclude();
        if (!reachedCow)
        {
            _uiManager.ShowResult(false);
            return;
        }

        if (_terrainController.GetCurrentTerrainPercentage() <
            _levelSettings.pointsSettings.objectivePercentageToGetPoints)
        {
            _uiManager.ShowResult(false);
            return;
        }
        
        _uiManager.ShowResult(true);
    }

    public void Conclude()
    {
        Cow.eatTerrain -= EatTerrain;
        Seed.increaseRegeneration -= IncreaseRegeneration;
        UIManager.OnGameEnded -= CheckVictory;
        _cowController.Conclude();
        _seedController.Conclude();
        _playerController.Conclude();
        _terrainController.Conclude();
        _uiManager.Conclude();
        _pointsSystem.Conclude();
        _audioManager.Conclude();
        enemyFarmersController.Conclude(0);
        enemyFarmersController.Conclude(1);
        StopAllCoroutines();
    }

    public void OpenScene(int scene)
    {
        switch (scene)
        {
            case 1:
                SceneManager.LoadScene("FirstLevelScene");
                break;
            case 2:
                SceneManager.LoadScene("SecondLevelScene");
                break;
            case 3:
                SceneManager.LoadScene("ThirdLevelScene");
                break;
        }
    }
}
