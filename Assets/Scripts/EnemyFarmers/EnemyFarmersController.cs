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
}
