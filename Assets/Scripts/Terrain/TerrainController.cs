using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grassTerrainSR;
    [SerializeField] private Slider grassTerrainSlider;
    [SerializeField] private LevelSettings _levelSettings;

    [SerializeField] private float minTerrainValue;
    [SerializeField] private float maxTerrainValue;

    private float regenerationRate;
    private float currentGrassSize;
    private float terrainRange; // Rango entre min y max

    public void Initialize()
    {
        terrainRange = maxTerrainValue - minTerrainValue;

        regenerationRate = (_levelSettings.terrainSettings.regenerationPercentage / 100f) * terrainRange;

        grassTerrainSlider.minValue = minTerrainValue;
        grassTerrainSlider.maxValue = maxTerrainValue;

        currentGrassSize = minTerrainValue;
        grassTerrainSlider.value = currentGrassSize;

        grassTerrainSlider.onValueChanged.AddListener(OnGrassValueChanged);
        OnGrassValueChanged(minTerrainValue);

        StartCoroutine(ManageTerrainRegeneration());
    }

    private void OnGrassValueChanged(float value)
    {
        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, value);
    }

    private IEnumerator ManageTerrainRegeneration()
    {
        while (currentGrassSize < maxTerrainValue)
        {
            yield return new WaitForSeconds(_levelSettings.terrainSettings.regerationTime);

            UpdateTerrainSize(currentGrassSize + regenerationRate);
        }
    }

    private void UpdateTerrainSize(float newSize)
    {
        currentGrassSize = Mathf.Clamp(newSize, minTerrainValue, maxTerrainValue);

        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, currentGrassSize);
        grassTerrainSlider.value = currentGrassSize;
    }

    public void Conclude()
    {
        grassTerrainSlider.onValueChanged.RemoveListener(OnGrassValueChanged);

        StopAllCoroutines();
    }
}



