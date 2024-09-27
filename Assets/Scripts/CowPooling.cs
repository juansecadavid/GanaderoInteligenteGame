using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowPooling : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    [SerializeField] private int _poolLenght;

    [SerializeField] private List<GameObject> _poolElements;
    // Start is called before the first frame update
    void Start()
    {
        InstantiatePool();
        //StartCoroutine(PrintWithRandomIntervals(10,50f, 3f,5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstantiatePool()
    {
        for (int i = 0; i < _poolLenght; i++)
        {
            GameObject clone = Instantiate(_prefab, transform.parent);
            clone.SetActive(false);
            _poolElements.Add(clone);
        }
    }
    
    IEnumerator PrintWithRandomIntervals(int x, float totalTime, float minInterval, float maxInterval)
    {
        float remainingTime = totalTime;

        for (int i = 0; i < x; i++)
        {
            // Si estamos en el último print, aseguramos que usemos el tiempo restante
            if (i == x - 1)
            {
                Debug.Log($"Print {i + 1} at {Time.time}");
                yield return new WaitForSeconds(remainingTime);
                break;
            }

            // Generar un intervalo aleatorio
            float randomInterval = Random.Range(minInterval, maxInterval);

            // Si el intervalo aleatorio es mayor al tiempo restante, ajustamos
            if (randomInterval > remainingTime / (x - i))
            {
                randomInterval = remainingTime / (x - i);
            }

            Debug.Log($"Print {i + 1} at {Time.time}");
            
            // Esperar por el intervalo generado
            yield return new WaitForSeconds(randomInterval);

            // Restar el intervalo al tiempo restante
            remainingTime -= randomInterval;
        }

        Debug.Log("Fin de los prints");
    }
}
