using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarmersController : MonoBehaviour
{
    [SerializeField] private EnemyFarmerBase[] enemyFarmers;
    
    public void EnableEnemyFarmer(int index, bool value)
    {
        enemyFarmers[index].gameObject.SetActive(value);
    }
}
