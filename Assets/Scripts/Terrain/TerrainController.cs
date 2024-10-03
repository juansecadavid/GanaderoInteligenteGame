using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grassTerrainSR;
    [SerializeField] private Slider grassTerrainSlider;
    [SerializeField] private LevelDataController levelData;

    private float initialTerrainSize;
    private float regenerationRate;
    private float degradationRate;
    private int cowsAppeared;
    private float currentGrassSize;
    private bool isCowFed;
    private bool isSeedPlanted;

    private void Awake()
    {
        initialTerrainSize = grassTerrainSR.size.y;
        regenerationRate = levelData.regenerationPercentage / 100f * initialTerrainSize;
        degradationRate = levelData.degradationPercentage / 100f * initialTerrainSize;

        grassTerrainSlider.maxValue = initialTerrainSize;
        grassTerrainSlider.value = initialTerrainSize;

        grassTerrainSlider.onValueChanged.AddListener(OnGrassValueChanged);

        grassTerrainSlider.value = 0;
        currentGrassSize = 0;

        StartCoroutine(ManageTerrainRegeneration());
        //StartCoroutine(ManageCows());
        //StartCoroutine(ManageTerrainDegradation());
        //StartCoroutine(ManageFeedConsumption());
    }

    private void OnGrassValueChanged(float value)
    {
        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, value);
    }

    private IEnumerator ManageTerrainRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(levelData.regerationTime);  // Cada 6 segundos
            UpdateTerrainSize(currentGrassSize + regenerationRate);
        }
    }

    private void UpdateTerrainSize(float newSize)
    {
        currentGrassSize = newSize;
        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, currentGrassSize);
        grassTerrainSlider.value = currentGrassSize;
    }

    //private IEnumerator ManageCows()
    //{
    //    cowsAppeared = 0;
    //    float cowInterval = (levelData.degradationTime - 6f) / levelData.cowsCount;  // Espaciadas en el tiempo
    //    yield return new WaitForSeconds(2f);  // Primera vaca aparece a los 2 segundos

    //    while (cowsAppeared < levelData.cowsCount)
    //    {
    //        cowsAppeared++;
    //        Debug.Log("Vaca apareció: " + cowsAppeared);
    //        yield return new WaitForSeconds(cowInterval);
    //    }
    //}

    //private IEnumerator ManageTerrainDegradation()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(levelData.degradationTime);  // Cada 8 segundos
    //        UpdateTerrainSize(currentGrassSize - degradationRate);
    //    }
    //}

    //private IEnumerator ManageFeedConsumption()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(2f);  // Cada 2 segundos
    //        if (!isCowFed)  // Si no se alimenta la vaca
    //        {
    //            UpdateTerrainSize(currentGrassSize - (levelData.feedPercentage / 100f * initialTerrainSize));
    //        }
    //    }
    //}

    //public void PlantSeed()
    //{
    //    if (!isSeedPlanted)
    //    {
    //        isSeedPlanted = true;
    //        StartCoroutine(SowSeed());
    //    }
    //}

    //private IEnumerator SowSeed()
    //{
    //    yield return new WaitForSeconds(levelData.sowingTime);  // Tiempo de siembra de 2 segundos
    //    float seedRegeneration = (levelData.seedsRegenerationPercentage / 100f * initialTerrainSize);
    //    regenerationRate += seedRegeneration;
    //    Debug.Log("Semilla plantada, regeneración aumentada: " + regenerationRate);
    //    isSeedPlanted = false;
    //}
}

