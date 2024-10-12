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

    private bool isMoving;
    private Vector3 startPos;
    private float elapsedTime;
    private Coroutine destroyingCoroutine;

    private bool isDestroying;
    protected bool isChasedAway;
    private bool hasChanged = false;

    public virtual void Initialize()
    {
        transform.position = movePoints[Random.Range(0, movePoints.Length)].position;
        gameObject.SetActive(true);
    }

    protected void MoveToTarget(Transform targetPos)
    {
        if (isMoving) return;

        startPos = transform.position;
        StartCoroutine(MoveOverTime(targetPos.position));
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

        isDestroying = false;
        DestroyCompleted?.Invoke(!hasChanged);
    }

    public void DriveAway()
    {
        isDestroying = false;
        isChasedAway = true;
        MoveToTarget(movePoints[Random.Range(0, movePoints.Length)]);
    }

    public virtual void Conclude()
    {
        StopCoroutine(destroyingCoroutine);
        gameObject.SetActive(false);
    }
}
