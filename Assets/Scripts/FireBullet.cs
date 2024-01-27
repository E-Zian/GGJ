using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    // Start is called before the first frame update

    private string enemyTag = "Enemy";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.GetComponent<EnemyController>().startKenaFire();
            Debug.Log("Fire");
        }
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
