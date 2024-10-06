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
    private float targetGrassSize; // Tamaño hacia el que se mueve el terreno
    private bool isUpdating = false; // Bandera para saber si ya se está actualizando

    public void Initialize()
    {
        terrainRange = maxTerrainValue - minTerrainValue;

        regenerationRate = (_levelSettings.terrainSettings.regenerationPercentage / 100f) * terrainRange;

        grassTerrainSlider.minValue = minTerrainValue;
        grassTerrainSlider.maxValue = maxTerrainValue;

        currentGrassSize = minTerrainValue;
        targetGrassSize = currentGrassSize;
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
        while (true)
        {
            yield return new WaitForSeconds(_levelSettings.terrainSettings.regerationTime);

            AddToTarget(regenerationRate);
        }
    }

    public void UpdateTerrain(float value)
    {
        float newTargetValue = (value / 100f) * terrainRange;
        AddToTarget(newTargetValue);
    }

    private void AddToTarget(float value)
    {
        targetGrassSize = Mathf.Clamp(targetGrassSize + value, minTerrainValue, maxTerrainValue);

        if (!isUpdating)
        {
            StartCoroutine(UpdateTerrainSize());
        }
    }

    private IEnumerator UpdateTerrainSize()
    {
        isUpdating = true;
        float elapsedTime = 0f;
        float duration = _levelSettings.terrainSettings.regerationTime;

        while (Mathf.Abs(currentGrassSize - targetGrassSize) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            currentGrassSize = Mathf.Lerp(currentGrassSize, targetGrassSize, elapsedTime / duration);
            currentGrassSize = Mathf.Clamp(currentGrassSize, minTerrainValue, maxTerrainValue);

            grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, currentGrassSize);
            grassTerrainSlider.value = currentGrassSize;

            yield return null;
        }

        currentGrassSize = Mathf.Clamp(targetGrassSize, minTerrainValue, maxTerrainValue);
        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, currentGrassSize);
        grassTerrainSlider.value = currentGrassSize;

        isUpdating = false;
    }

    public void Conclude()
    {
        grassTerrainSlider.onValueChanged.RemoveListener(OnGrassValueChanged);

        StopAllCoroutines();
    }

    [ContextMenu("TestUpdateTerrain")]
    public void TestUpdateTerrain()
    {
        int randomValue = Random.Range(-10, 10);
        print("TestUpdateTerrain: " + randomValue);
        UpdateTerrain(randomValue);
    }
}
