using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemy : MonoBehaviour
{
    float health;
    private void OnEnable()
    {
        health = GetComponent<EnemyController>().health;
    }
    private void Update()
    {
        if(health <= 0)
        {
            
        }
    }
}
