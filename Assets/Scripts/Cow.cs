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
    public static Action<int> cowAmount;
    public static Action cowHit;
    private int numberOfHitMax;
    private int numberOfHitMin;
    private int numberOfHits;
    float timeToEatTerrain;
    private float percentajeWhenEat;
    private Vector2 _currentTarget;
    public float _speed;
    [SerializeField] private Type _type;

    private State _state = State.WalkingFree;
    public State State {  get { return _state; } }

    
    void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;
        
        float distanceToTarget = Vector2.Distance(currentPosition, _currentTarget);
        
        Vector2 direction = (_currentTarget - currentPosition).normalized;
        
        Vector2 newPosition = currentPosition + direction * (_speed * Time.fixedDeltaTime);

        if (_state != State.gameEnded)
        {
            transform.position = newPosition;
        }
        
        
        switch (_state)
        {
            case State.WalkingFree:
                if (distanceToTarget < 0.1f)
                {
                    SetTargetPoint(-1.5f,8f, -4.3f, 2f);
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
            case State.gameEnded:
                direction = Vector2.zero;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state == State.WalkingFree && other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var playerController) && !playerController.IsAttached)
            {
                numberOfHits--;
                cowHit?.Invoke();
                if (numberOfHits <= 0)
                {
                    _state = State.WalkingToCorral;
                    _speed = 2f;
                    _currentTarget = _randomPointInPolygon.GetRandomPoint();
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    cowAmount?.Invoke(1);
                }
            }
        }
    }

    public void Initialize()
    {
        numberOfHitMax = _levelSettings.cowSettings.maximumHitsToGo;
        numberOfHitMin = _levelSettings.cowSettings.minimunHitsToGo;
        timeToEatTerrain = _levelSettings.cowSettings.timeToEatTerrain;

        SetStartValues();
        SetTargetPoint(-1.5f, 8f, -4.3f, 2f);
        SetHitNumbers();
    }

    public void Conclude()
    {
        StopAllCoroutines();
        _state = State.gameEnded;
    }

    private void SetStartValues()
    {
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
        cowAmount?.Invoke(-1);
        _state = State.WalkingFree;
        GetComponent<BoxCollider2D>().enabled = true;
        SetStartValues();
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

public enum Type
{
    Common,
    Hungry,
    Special
}

public enum State
{
    WalkingFree,
    WalkingToCorral,
    InCorral,
    gameEnded
}
