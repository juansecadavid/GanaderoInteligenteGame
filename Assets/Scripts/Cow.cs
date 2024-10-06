using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cow : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private GetRandomPointInPolygon _randomPointInPolygon;
    [SerializeField] private RandomPointInSquare _randomPointInSquare;
    public static Action<float,GameObject> eatTerrain;
    private int numberOfHitMax;
    private int numberOfHitMin;
    private int numberOfHits;
    float timeToEatTerrain;
    private float percentajeWhenEat;
    private Vector2 _currentTarget;
    public float _speed;
    
    
    
    enum state
    {
        WalkingFree,
        WalkingToCorral,
        InCorral
    }
    private state _state = state.WalkingFree;
    public void Initialize()
    {
        numberOfHitMax = _levelSettings.cowSettings.maximumHitsToGo;
        numberOfHitMin = _levelSettings.cowSettings.minimunHitsToGo;
        timeToEatTerrain = _levelSettings.cowSettings.timeToEatTerrain;
        percentajeWhenEat = _levelSettings.cowSettings.percentajeWhenEat;
        SetTargetPoint(-3f,8f, -4.3f, 2f);
        SetHitNumbers();
    }
    void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;
        
        float distanceToTarget = Vector2.Distance(currentPosition, _currentTarget);
        
        Vector2 direction = (_currentTarget - currentPosition).normalized;
        
        Vector2 newPosition = currentPosition + direction * (_speed * Time.fixedDeltaTime);
        
        transform.position = newPosition;
        
        switch (_state)
        {
            case state.WalkingFree:
                if (distanceToTarget < 0.1f)
                {
                    SetTargetPoint(-3f,8f, -4.3f, 2f);
                }
                break;
            case state.WalkingToCorral:
                if (distanceToTarget < 0.1f)
                {
                    //SetTargetPoint(-8.2f,-3.5f, 2f, 4.3f);
                    _currentTarget = (_randomPointInPolygon.GetRandomPoint());
                    _state = state.InCorral;
                }
                break;
            case state.InCorral:
                if (distanceToTarget < 0.1f)
                {
                    //SetTargetPoint(-8.2f,-3.5f, 2f, 4.3f);
                    _speed = 0.5f;
                    _currentTarget = (_randomPointInPolygon.GetRandomPoint());
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state==state.WalkingFree && other.gameObject.CompareTag("Player"))
        {
            numberOfHits--;
            if (numberOfHits <= 0)
            {
                _state = state.WalkingToCorral;
                _speed = 2f;
                _currentTarget = _randomPointInPolygon.GetRandomPoint();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    private void SetHitNumbers()
    {
        numberOfHits = Random.Range(numberOfHitMin, numberOfHitMax);
    }
    private void SetTargetPoint(float x1, float x2, float y1, float y2)
    {
        _currentTarget = new Vector2(Random.Range(x1, x2), Random.Range(y1,y2));
    }
    private void OnEnable()
    {
        _randomPointInSquare.PositionObjectOnEdge(gameObject);
        StartCoroutine(EatTerrain());
    }
    public void SettingFree()
    {
        _state = state.WalkingFree;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    private IEnumerator EatTerrain()
    {
        while (_state==state.WalkingFree)
        {
            yield return new WaitForSeconds(timeToEatTerrain);
            if (_state == state.WalkingFree)
            {
                eatTerrain?.Invoke(percentajeWhenEat,gameObject);
            }
        }
    }
}
