using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomZombieNoises : MonoBehaviour
{
    public GameObject zombieNoise; 
    public float minInterval = 1.0f; 
    public float maxInterval = 2.5f;
    void Start()
    {
        StartCoroutine(RandomTime());
    }


    IEnumerator RandomTime()
    {
        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        Instantiate(zombieNoise);
        if (GameManager.remainingEnemyAmt >0) {
            StartCoroutine(RandomTime());
        }
        
    }
}
