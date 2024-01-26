using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    public void startDecay(float decayTime)
    {
        StartCoroutine(selfDecay(decayTime));
    }

    IEnumerator selfDecay(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
