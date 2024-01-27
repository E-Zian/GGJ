using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public List<GameObject> finalSpawners;
    public static List<GameObject> activeSpawners;
    public static float currentSpawnedEnemy;
    public float maxSpawnedEnemy;
    public float maxEnemyNormal;
    public float maxEnemyCrazy;
    public int enemySpawnGroupSize;
    public int specialCounter;
    public int killToSpawnSpecial;
    public GameObject[] finalEnemies;
    public float finalEnemiesCount;
    public bool inFinalStage;

    //Prefabs
    public GameObject enemy;
    public GameObject fatEnemy;
    public GameObject runnerEnemy;

    public AudioSource bgm;

    //Boolean to run once
    public bool runOnce;

    //Post-processing
    public GameObject postVolume;

    //Object pools
    public static List<GameObject> droppedWeaponPool;

    //Strings
    private string enemyTag = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        remainingEnemyAmt = startingEnemyAmt = enemyAmount;
        currentSpawnedEnemy = 0;
        activeSpawners = spawners;
        bgm.loop = true;
        bgm.Play();
        specialCounter = 0;
        runOnce = false;
        inFinalStage = false;
        maxSpawnedEnemy = maxEnemyNormal;
        droppedWeaponPool = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnedEnemy < maxSpawnedEnemy && remainingEnemyAmt > 200)
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
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(fatEnemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
                    }
                }
                else if (randomBoss == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(runnerEnemy, activeSpawners[randomSpawn].transform.position, Quaternion.identity);
                    }
                }
            }
        }
        if (remainingEnemyAmt <= 200 && remainingEnemyAmt > 100)
        {
            //Spawn point selection
            int randomSpawn = Random.Range(0, finalSpawners.Count);
            Instantiate(enemy, finalSpawners[randomSpawn].transform.position, Quaternion.identity);
            remainingEnemyAmt--;
        }
        if (remainingEnemyAmt <= 100 && remainingEnemyAmt > 0)
        {
            int randomSpawn = Random.Range(0, finalSpawners.Count);
            int randomBoss = Random.Range(1, 3);
            if (randomBoss == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(fatEnemy, finalSpawners[randomSpawn].transform.position, Quaternion.identity);
                }
            }
            else if (randomBoss == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(runnerEnemy, finalSpawners[randomSpawn].transform.position, Quaternion.identity);
                }
            }
            remainingEnemyAmt--;
        }
        if (remainingEnemyAmt <= 0 && finalEnemiesCount <= 0)
        {
            inFinalStage = true;
            finalEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
            foreach (var item in finalEnemies)
            {
                finalEnemiesCount++;
            }
            enemiesLeftText.text = finalEnemiesCount.ToString();
        }
        if (finalEnemiesCount <= 0 && inFinalStage)
        {
            SceneManager.LoadScene("GameEndingScene");
        }
        if (remainingEnemyAmt > 0)
        {
            enemiesLeftText.text = remainingEnemyAmt.ToString();
        }


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
        if (droppedWeaponPool.Count > 5)
        {
            Destroy(droppedWeaponPool[0].gameObject);
            droppedWeaponPool.RemoveAt(0);
        }
        Debug.Log(droppedWeaponPool.Count);
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
