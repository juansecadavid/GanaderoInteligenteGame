using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingAndEnabling : MonoBehaviour
{
    public List<GameObject> InstantiatePool(int x, GameObject gameObject)
    {
        List<GameObject> instantiatedList = new List<GameObject>();
        for (int i = 0; i < x; i++)
        {
            GameObject clone = Instantiate(gameObject);
            clone.SetActive(false);
            instantiatedList.Add(clone);
        }
        return instantiatedList;
    }

    public IEnumerator ShowRandomTimes(int x, float y, float e, float p, List<GameObject> gameObjectsList)
    {
        // Validación de parámetros
        if (x <= 0 || y <= 0 || e < 0 || p < 0 || e + p >= y)
        {
            Debug.LogError("Los parámetros no son válidos.");
            yield break;
        }
        
        // Generar tiempos aleatorios para las impresiones intermedias
        List<float> times = new List<float>();
        times.Add(e); // Tiempo de la primera impresión
        times.Add(y - p); // Tiempo de la última impresión

        int numMiddlePrints = x - 2; // Número de impresiones intermedias

        for (int i = 0; i < numMiddlePrints; i++)
        {
            float randomTime = Random.Range(e, y - p);
            times.Add(randomTime);
        }

        // Ordenar los tiempos
        times.Sort();

        // Programar las impresiones
        float previousTime = 0f;

        for (int i = 0; i < times.Count; i++)
        {
            float waitTime = times[i] - previousTime;
            yield return new WaitForSeconds(waitTime);
            previousTime = times[i];
            gameObjectsList[i].SetActive(true);
            //Debug.Log("Impresión " + (i + 1) + " en el tiempo " + times[i]);
        }
    }
}
