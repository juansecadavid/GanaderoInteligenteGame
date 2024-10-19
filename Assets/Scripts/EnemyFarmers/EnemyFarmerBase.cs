using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFarmerBase : MonoBehaviour
{
    public Action MoveToTargetFinished;
    public Action<bool> DestroyCompleted;

    [SerializeField] protected float destructionTime;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private int numberOfHits;

    private bool isMoving;
    private Vector3 startPos;
    private float elapsedTime;
    private Coroutine destroyingCoroutine;

    private bool isDestroying;
    protected bool isChasedAway;
    private bool hasChanged = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize()
    {
        transform.position = movePoints[Random.Range(0, movePoints.Length)].position;
        gameObject.SetActive(true);
        spriteRenderer.flipX = false;
    }

    protected void MoveToTarget(Transform targetPos)
    {
        if (isMoving) return;

        startPos = transform.position;
        StartCoroutine(MoveOverTime(targetPos.position));

        animator.SetBool("Destroying", false);
    }

    private IEnumerator MoveOverTime(Vector3 targetPos)
    {
        isMoving = true;
        elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        MoveToTargetFinished?.Invoke();
    }

    protected virtual void Destroy()
    {
        isDestroying = true;
        animator.SetBool("Destroying", true);
        destroyingCoroutine = StartCoroutine(Destroying());
    }

    private IEnumerator Destroying()
    {
        float elapsedTime = 0f;
        bool initialValue = isDestroying;
        hasChanged = false;

        while (elapsedTime < destructionTime)
        {
            if (isDestroying != initialValue)
            {
                hasChanged = true;
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //isDestroying = false;
        DestroyCompleted?.Invoke(!hasChanged);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroying && other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var playerController) && !playerController.IsAttached)
            {
                numberOfHits--;
                if (numberOfHits <= 0)
                {
                    DriveAway();
                }
            }
        }
    }

    protected virtual void DriveAway()
    {
        isDestroying = false;
        isChasedAway = true;
        spriteRenderer.flipX = true;
        MoveToTarget(movePoints[Random.Range(0, movePoints.Length)]);
    }

    public virtual void Conclude()
    {
        isDestroying = false;
        isChasedAway = false;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
