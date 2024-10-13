using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmerPlants : EnemyFarmerBase
{
    [SerializeField] private SeedController seedController;

    private List<GameObject> currentSeedsPlanted;

    private int currentSeedSelected;

    public override void Initialize()
    {
        currentSeedsPlanted = seedController.SeedPool;
        base.Initialize();

        SelectActiveSeed();
    }

    private void SelectActiveSeed()
    {
        do
        {
            currentSeedSelected = Random.Range(0, currentSeedsPlanted.Count);
        }
        while (!currentSeedsPlanted[currentSeedSelected].activeInHierarchy && 
        currentSeedsPlanted[currentSeedSelected].GetComponent<Seed>().State != States.Planted);

        MoveToTargetFinished += OnSeedPos;
        MoveToTarget(currentSeedsPlanted[currentSeedSelected].transform);
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

        if (value)
        {
            currentSeedsPlanted[currentSeedSelected].GetComponent<Seed>().SettingDestroyed();

            if (!isChasedAway)
            {
                SelectActiveSeed();
            }
        }
    }

    public override void Conclude()
    {
        base.Conclude();
    }
}
