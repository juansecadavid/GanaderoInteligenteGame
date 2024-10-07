using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CowController _cowController;

    [SerializeField] private TerrainController _terrainController;

    [SerializeField] private SeedController _seedController;

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private LevelSettings _levelSettings;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Cow.eatTerrain += EatTerrain;
        Seed.increaseRegeneration += IncreaseRegeneration;
    }

    public void EatTerrain(float percent,GameObject cow)
    {
        //IMplementar que se baje el terreno
        _terrainController.UpdateTerrain(-percent);
        //print($"Comer {percent}% de terreno");
    }

    public void IncreaseRegeneration(float regenerationPercentajeToIncrease)
    {
        //Implementar que ahora la regeneracion sube su porcentaje
    }

    IEnumerator CheckTerrainPercentage()
    {
        while (_terrainController.GetCurrentTerrainPercentage()<_levelSettings.seedSettings.percentajeToFirstSpawn)
        {
            print($"Estoy en el bucle {_terrainController.GetCurrentTerrainPercentage()}");
            yield return null;
        }
        print("Salí del bucle");

        _seedController.StartSeedSpawn();
    }

    public void Initialize()
    {
        _cowController.Initialize();
        _seedController.Initialize();
        _playerController.Initialize();
        _terrainController.Initialize();
        StartCoroutine(CheckTerrainPercentage());
    }

    public void Conclude()
    {
        _cowController.Conclude();
        _seedController.Conclude();
        _playerController.Conclude();
        _terrainController.Conclude();
    }
}
