using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmerCows : EnemyFarmerBase
{
    [SerializeField] private CowController cowController;
    [SerializeField] private Transform fencePoint;
    [SerializeField] private Transform interactableFence;

    private Coroutine releaseCoroutine;

    public override void Initialize()
    {
        print("Initialize EnemyFarmerCows");
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
        print("Destroy");
        DestroyCompleted += OnDestroyCompleted;
        base.Destroy();
    }

    

    private void OnDestroyCompleted(bool value)
    {
        print("OnDestroyCompleted: " + value);
        DestroyCompleted -= OnDestroyCompleted;
        interactableFence.gameObject.SetActive(!value);

        if (value)
        {
            releaseCoroutine = StartCoroutine(CheckAndReleaseCow());
        }

        // Hacer que una vaca salga.
    }

    private IEnumerator CheckAndReleaseCow()
    {
        while (!isChasedAway)
        {
            SelectReleasedCow();
            yield return new WaitForSeconds(destructionTime);
        }
    }

    private void SelectReleasedCow()
    {
        print("SelectReleasedCow");

        if (!isChasedAway)
        {
            int currentCowSelected;
            do
            {
                currentCowSelected = Random.Range(0, cowController.CowPool.Count);
            }
            while (cowController.CowPool[currentCowSelected].GetComponent<Cow>().State != State.InCorral);

            cowController.CowPool[currentCowSelected].GetComponent<Cow>().SettingFree();
        }
    }


    public override void Conclude()
    {
        StopCoroutine(releaseCoroutine);
        base.Conclude();
    }
}
