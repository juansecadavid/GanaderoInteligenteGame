using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmerCows : EnemyFarmerBase
{
    [SerializeField] private Transform fencePoint;
    [SerializeField] private Transform interactableFence;

    public override void Initialize()
    {
        base.Initialize();

        MoveToTargetFinished += OnFencePoint;
        MoveToTarget(fencePoint);
    }

    private void OnFencePoint()
    {
        MoveToTargetFinished -= OnFencePoint;
        
        Destroy();
    }

    protected override void Destroy()
    {
        DestroyCompleted += OnDestroyCompleted;
        base.Destroy();
    }

    private void OnDestroyCompleted(bool value)
    {
        DestroyCompleted -= OnDestroyCompleted;
        interactableFence.gameObject.SetActive(!value);

        //Hacer que una vaca salga.
    }
    

    public override void Conclude()
    {
        base.Conclude();
    }
}
