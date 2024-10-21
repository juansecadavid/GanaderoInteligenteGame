using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cow : MonoBehaviour
{
    public static Action<float,GameObject> eatTerrain;
    public static Action<int> cowAmount;
    public static Action cowHit;
    public float _speed;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private GetRandomPointInPolygon _randomPointInPolygon;
    [SerializeField] private RandomPointInSquare _randomPointInSquare;
    [SerializeField] private Type _type;

    private int numberOfHitMax;
    private int numberOfHitMin;
    private int numberOfHits;
    private float timeToEatTerrain;
    private float percentajeWhenEat;
    private Vector2 _currentTarget;
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    private State _state = State.WalkingFree;
    public State State {  get { return _state; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }


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

        // Check if the cow is moving to the left or right
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        switch (_state)
        {
            case State.WalkingFree:
                if (distanceToTarget < 0.1f)
                {
                    SetTargetPoint(-1.5f, 8f, -4.3f, 2f);
                }
                break;
            case State.WalkingToCorral:
                if (distanceToTarget < 0.1f)
                {
                    _currentTarget = _randomPointInPolygon.GetRandomPoint();
                    _state = State.InCorral;
                }
                break;
            case State.InCorral:
                if (distanceToTarget < 0.1f)
                {
                    int currentAction = Random.Range(0, 3);
                    print("currentAction: " + currentAction);

                    switch (currentAction)
                    {
                        case 0:
                            if (!animator.GetBool("Muu") && !animator.GetBool("Eating"))
                            {
                                InCorral();
                            }
                            break;

                        case 1:
                            if (!animator.GetBool("Muu"))
                            {
                                _speed = 0f;
                                animator.SetBool("Muu", true);
                                LeanTween.delayedCall(2f, InCorral);
                            }
                            break;

                        case 2:
                            if (!animator.GetBool("Eating"))
                            {
                                _speed = 0f;
                                animator.SetBool("Eating", true);
                                LeanTween.delayedCall(2f, InCorral);
                            }
                            break;
                    }

                    void InCorral()
                    {
                        animator.SetBool("Muu", false);
                        animator.SetBool("Eating", false);
                        _speed = 0.5f;
                        _currentTarget = _randomPointInPolygon.GetRandomPoint();
                    }
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
                SetTargetPoint(-1.5f, 8f, -4.3f, 2f);
                if (numberOfHits <= 0)
                {
                    _state = State.WalkingToCorral;
                    _speed = 2f;
                    _currentTarget = _randomPointInPolygon.GetRandomPoint();
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    cowAmount?.Invoke(1);

                    bool enableWord = Random.Range(0, 2) == 0;

                    if (enableWord)
                        playerController.GetComponentInChildren<PlayerWordsController>().EnableWord();
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
        animator.enabled = false;
        eatTerrain = null;
        cowAmount = null;
        cowHit = null;
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
