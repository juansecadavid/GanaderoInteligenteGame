using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grassTerrainSR;

    [SerializeField] private Slider grassTerrainSlider;

    private void Awake()
    {
        grassTerrainSlider.maxValue = grassTerrainSR.size.y;
        grassTerrainSlider.value = grassTerrainSlider.maxValue;
        grassTerrainSlider.onValueChanged.AddListener(OnGrassValueChanged);
    }

    private void OnGrassValueChanged(float value)
    {
        grassTerrainSR.size = new Vector2(grassTerrainSR.size.x, value);
    }
}
