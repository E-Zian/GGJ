using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Enemy Amount Settings and Variables
    public static float remainingEnemyAmt;
    public static float startingEnemyAmt;
    public float enemyAmount;

    //Enemy Spawn & Spawners
    public List<GameObject> spawners;
    public static float currentSpawnedEnemy;
    

    // Start is called before the first frame update
    void Start()
    {
        remainingEnemyAmt = startingEnemyAmt = enemyAmount;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
