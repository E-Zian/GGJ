using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject bloodSplatter;
    private string enemyTag = "Enemy";

    void OnCollisionEnter2D(Collision2D other)
    {
        //Blood Splatter and Normal Calculation
        //Why does it not work properly?
        //Cause math
        if (other.gameObject.CompareTag(enemyTag))
        {
            foreach (var contact in other.contacts)
            {
                float angle = Vector3.Angle(contact.point, contact.normal);

                Instantiate(bloodSplatter, other.gameObject.transform.position, Quaternion.Euler(new Vector3(angle, 90, -90)));

            }
        }
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
