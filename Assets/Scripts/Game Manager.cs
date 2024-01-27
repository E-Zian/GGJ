using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Enemy Amount Settings and Variables
    public static float remainingEnemyAmt;
    public static float startingEnemyAmt;
    public static int enemiesLeft;
    public float enemyAmount;
    public TextMeshProUGUI enemiesLeftText;
    //Enemy Spawn & Spawners
    public List<GameObject> spawners;
    public static List<GameObject> activeSpawners;
    public static float currentSpawnedEnemy;
    public float maxSpawnedEnemy;
    public int enemySpawnGroupSize;
    public GameObject enemy;

    public AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        enemiesLeft = 5000;
        remainingEnemyAmt = startingEnemyAmt = enemyAmount;
        activeSpawners = spawners;
        bgm.loop = true;
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnedEnemy < maxSpawnedEnemy)
        {
            //Spawn point selection
            int randomSpawn = Random.Range(0, activeSpawners.Count);
            //for (int i = 0; i < enemySpawnGroupSize; i++)
            //{
                
            //}
            Instantiate(enemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
            currentSpawnedEnemy++;
        }
        enemiesLeftText.text = enemiesLeft.ToString();
    }
}
