using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noise : MonoBehaviour
{
    public AudioSource[] noises;

    public float Interval = 2.5f;
    private void Awake()
    {
        StartCoroutine(RandomTime());
    }
  
    IEnumerator RandomTime()
    {
        noises[Random.Range(0, noises.Length)].Play();
        yield return new WaitForSeconds(Interval);
        Destroy(gameObject);
    }

   
}
