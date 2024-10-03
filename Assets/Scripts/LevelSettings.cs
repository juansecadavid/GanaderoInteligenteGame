using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelSettings", menuName = "Game Settings/Level Settings")]
public class LevelSettings : ScriptableObject
{
    [System.Serializable]
    public class CowSettings
    {
        [Tooltip("Tiempo en segundos para la aparición de la primera vaca.")]
        public int timeToSpawnFirstCow = 2;
        [Tooltip("Tiempo en segundos para la aparición de la última vaca antes que se cumpla el tiempo del nivel.")]
        public int timeToLastSpawn = 4;
        [Tooltip("Numero de toques minimos necesarios para que la vaca vaya hacia el corral.")]
        public int minimunHitsToGo = 3;
        [Tooltip("Numero de toques maximos necesarios para que la vaca vaya hacia el corral.")]
        public int maximumHitsToGo = 6;
    }

    [System.Serializable]
    public class SeedSettings
    {
        [Tooltip("Tiempo en segundos para la aparición de la última semilla antes que se cumpla el tiempo del nivel.")]
        public int timeToLastSpawn = 10;
        [Tooltip("Porcentaje del terreno para la aparición de la primera semilla")]
        public int percentajeToFirstSpawn = 20;
        
    }
    
    [System.Serializable]
    public class GameLevelSettings
    {
        public int levelDuration = 1920;
        public int totalCows = 3;
        public int totalSeeds = 3;
    }
    
    public CowSettings cowSettings;
    public GameLevelSettings gameLevelSettings;
    public SeedSettings seedSettings;
}
