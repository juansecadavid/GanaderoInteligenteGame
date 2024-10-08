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
    [SerializeField] private Type _type;
    private enum Type
    {
        Common,
        Hungry,
        Special
    }
    
    enum State
    {
        WalkingFree,
        WalkingToCorral,
        InCorral
    }
    private State _state = State.WalkingFree;
    public void Initialize()
    {
        numberOfHitMax = _levelSettings.cowSettings.maximumHitsToGo;
        numberOfHitMin = _levelSettings.cowSettings.minimunHitsToGo;
        timeToEatTerrain = _levelSettings.cowSettings.timeToEatTerrain;
        switch (_type)
        {
            case Type.Common:
                percentajeWhenEat = _levelSettings.cowSettings.commonPercentajeWhenEat;
                _speed = _levelSettings.cowSettings.commonCowSpeed;
                break;
            case Type.Hungry:
                percentajeWhenEat = _levelSettings.cowSettings.hungryPercentajeWhenEat;
                _speed = _levelSettings.cowSettings.hungryCowSpeed;
                break;
            case Type.Special:
                percentajeWhenEat = _levelSettings.cowSettings.specialPercentajeWhenEat;
                _speed = _levelSettings.cowSettings.specialCowSpeed;
                break;
        }
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
            case State.WalkingFree:
                if (distanceToTarget < 0.1f)
                {
                    SetTargetPoint(-3f,8f, -4.3f, 2f);
                }
                break;
            case State.WalkingToCorral:
                if (distanceToTarget < 0.1f)
                {
                    //SetTargetPoint(-8.2f,-3.5f, 2f, 4.3f);
                    _currentTarget = (_randomPointInPolygon.GetRandomPoint());
                    _state = State.InCorral;
                }
                break;
            case State.InCorral:
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
        if (_state==State.WalkingFree && other.gameObject.CompareTag("Player") && !other.GetComponent<PlayerController>().IsAttached)
        {
            numberOfHits--;
            if (numberOfHits <= 0)
            {
                _state = State.WalkingToCorral;
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
        _state = State.WalkingFree;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    private IEnumerator EatTerrain()
    {
        while (_state==State.WalkingFree)
        {
            yield return new WaitForSeconds(timeToEatTerrain);
            if (_state == State.WalkingFree)
            {
                eatTerrain?.Invoke(percentajeWhenEat,gameObject);
            }
        }
    }
}
