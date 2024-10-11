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

        MoveToTargetFinished += OnSeedPos;

        SelectActiveSeed();
    }

    private void SelectActiveSeed()
    {
        do
        {
            currentSeedSelected = Random.Range(0, currentSeedsPlanted.Count);
        }
        while (!currentSeedsPlanted[currentSeedSelected].activeInHierarchy);

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
        currentSeedsPlanted[currentSeedSelected].gameObject.SetActive(!value);

        //Revisar que debe pasar acà segun "value"
    }

    public override void Conclude()
    {
        base.Conclude();
    }
}
