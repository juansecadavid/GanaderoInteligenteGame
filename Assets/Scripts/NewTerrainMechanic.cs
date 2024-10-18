using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTerrainMechanic : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    public GameObject objectPrefab; // Prefab del objeto a instanciar
    int numberOfObjects; // Número 'x' de objetos a instanciar
    public Sprite[] randomSprites; // Arreglo de sprites para cambiar

    private float percentagePerObject;
    private int previousLevel;

    private List<ObjectSpriteChanger> originalObjects = new List<ObjectSpriteChanger>();
    private List<ObjectSpriteChanger> changedObjects = new List<ObjectSpriteChanger>();

    void Start()
    {
        numberOfObjects = _levelSettings.terrainSettings.numberOfBadTerrain;
        // Calcula el porcentaje de cambio por objeto
        percentagePerObject = 100f / numberOfObjects;

        // Instancia 'x' objetos y los agrega a la lista de objetos originales
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            ObjectSpriteChanger osc = obj.GetComponent<ObjectSpriteChanger>();
            osc.Initialize(randomSprites);
            originalObjects.Add(osc);
        }

        // Inicializa el nivel previo basado en el valor inicial del slider
        previousLevel = 0;

        // Agrega un listener para detectar cambios en el slider
        TerrainController.OnTerrainPercentageChanged += UpdateObjectsBasedOnSlider;
    }
    

    void UpdateObjectsBasedOnSlider(int currentPercentage)
    {
        int currentLevel = Mathf.FloorToInt(currentPercentage / percentagePerObject);

        if (currentLevel > previousLevel)
        {
            // El slider aumentó y pasó uno o más umbrales
            for (int level = previousLevel + 1; level <= currentLevel; level++)
            {
                if (originalObjects.Count > 0)
                {
                    // Selecciona un objeto que aún no ha cambiado
                    ObjectSpriteChanger osc = originalObjects[0];
                    originalObjects.RemoveAt(0);

                    // Cambia su sprite por uno aleatorio del arreglo
                    osc.ChangeToRandomSprite();

                    // Mueve el objeto a la lista de objetos cambiados
                    changedObjects.Add(osc);
                }
            }
        }
        else if (currentLevel < previousLevel)
        {
            // El slider disminuyó y pasó uno o más umbrales
            for (int level = previousLevel - 1; level >= currentLevel; level--)
            {
                if (changedObjects.Count > 0)
                {
                    // Selecciona un objeto que haya cambiado previamente
                    ObjectSpriteChanger osc = changedObjects[0];
                    changedObjects.RemoveAt(0);

                    // Restaura su sprite original
                    osc.ResetToOriginalSprite();

                    // Mueve el objeto de vuelta a la lista de objetos originales
                    originalObjects.Add(osc);
                }
            }
        }

        previousLevel = currentLevel;
    }
}
