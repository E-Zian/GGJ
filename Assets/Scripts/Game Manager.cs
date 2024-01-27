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
    public float maxEnemyNormal;
    public float maxEnemyCrazy;
    public int enemySpawnGroupSize;
    public int specialCounter;
    public int killToSpawnSpecial;

    //Prefabs
    public GameObject enemy;
    public GameObject fatEnemy;
    public GameObject runnerEnemy;

    public AudioSource bgm;

    //Boolean to run once
    public bool runOnce;

    //Post-processing
    public GameObject postVolume;

    // Start is called before the first frame update
    void Start()
    {
        remainingEnemyAmt = startingEnemyAmt = enemyAmount;
        activeSpawners = spawners;
        bgm.loop = true;
        bgm.Play();
        specialCounter = 0;
        runOnce = false;
        maxSpawnedEnemy = maxEnemyNormal;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnedEnemy < maxSpawnedEnemy && remainingEnemyAmt > 0)
        {
            //Spawn point selection
            int randomSpawn = Random.Range(0, activeSpawners.Count);     
            Instantiate(enemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
            currentSpawnedEnemy++;
            specialCounter++;
            if (specialCounter >= killToSpawnSpecial)
            {
                specialCounter = 0;
                int randomBoss = Random.Range(1, 3);
                if (randomBoss == 1)
                {
                    Instantiate(fatEnemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
                }
                else if (randomBoss == 2)
                {
                    Instantiate(runnerEnemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
                }
            }
        }
        if(remainingEnemyAmt <= 0)
        {
            //CHANGE SCENE
        }
        enemiesLeftText.text = remainingEnemyAmt.ToString();

        //Player Crazy Mode
        if (PlayerController.isCrazy)
        {
            maxSpawnedEnemy = maxEnemyCrazy;
            if (!runOnce)
            {
                StartCoroutine(crazyMode());
                postVolume.SetActive(true);
                runOnce = true;
            }
        }
    }

    IEnumerator crazyMode()
    {
        yield return new WaitForSeconds(PlayerController.crazyDuration - 10);
        maxSpawnedEnemy = maxEnemyNormal;
        yield return new WaitForSeconds(10f);
        postVolume.SetActive(false);
        runOnce = false;
    }
}
