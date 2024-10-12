using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmersController : MonoBehaviour
{
    [SerializeField] private EnemyFarmerBase[] enemyFarmers;
    
    public void Initialize(int index)
    {
        print("Initialize: " + index);
        enemyFarmers[index].Initialize();
    }

    public void Conclude(int index)
    {
        enemyFarmers[index].Conclude();
    }

    //Test
    [ContextMenu("EnableEnemyFarmerCows")]
    public void EnableEnemyFarmerCows()
    {
        Initialize(0);
    }

    [ContextMenu("EnableEnemyFarmerCowsAway")]
    public void EnableEnemyFarmerCowsAway()
    {
        enemyFarmers[0].DriveAway();
    }

    [ContextMenu("EnableEnemyFarmerPlants")]
    public void EnableEnemyFarmerPlants()
    {
        Initialize(1);
    }

    [ContextMenu("EnableEnemyFarmerPlantsAway")]
    public void EnableEnemyFarmerPlantsAway()
    {
        enemyFarmers[1].DriveAway();
    }
}
