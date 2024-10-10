using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmerPlants : EnemyFarmerBase
{
    private Transform[] currentSeedsPlanted;

    private int currentSeedSelected;

    public override void Initialize()
    {
        //currentSeedsPlanted = ;
        base.Initialize();

        MoveToTargetFinished += OnSeedPos;
        currentSeedSelected = Random.Range(0, currentSeedsPlanted.Length);
        MoveToTarget(currentSeedsPlanted[currentSeedSelected]);
    }

    private void OnSeedPos()
    {
        MoveToTargetFinished -= OnSeedPos;
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
        currentSeedsPlanted[currentSeedSelected].gameObject.SetActive(!value);

        //Revisar que debe pasar acà segun "value"
    }

    public override void Conclude()
    {
        base.Conclude();
    }
}
