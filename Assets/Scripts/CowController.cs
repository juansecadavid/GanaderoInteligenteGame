using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CowController : MonoBehaviour
{
    [SerializeField] private int numberOfHitMax;

    [SerializeField] private int numberOfHitMin;

    private int numberOfHits;

    private Vector2 _targetPoint;

    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetHitNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        //_rigidbody2D.MovePosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Algo me pegó");
        if (other.gameObject.CompareTag("Player"))
        {
            print("Me pegó el jugador");
            numberOfHits--;
            if (numberOfHits <= 0)
            {
                GoToCorral();
            }
        }
    }

    private void SetHitNumbers()
    {
        numberOfHits = Random.Range(numberOfHitMin, numberOfHitMax);
    }
    
    private void GoToCorral()
    {
        print("Me voy para el corral");
    }

    private void SetTargetPoint()
    {
        _targetPoint = new Vector2(Random.Range(-3f, 8f), Random.Range(-4.3f,2f));
    }
}
