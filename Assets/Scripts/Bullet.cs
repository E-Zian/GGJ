using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject bloodSplatter;

    void OnCollisionEnter2D(Collision2D other)
    {
        //Blood Splatter and Normal Calculation
        //Why does it not work properly?
        //Cause math
        foreach (var contact in other.contacts)
        {

            Debug.DrawRay(contact.point, contact.normal, Color.red, 2f);

            float angle = Vector3.Angle(contact.point, contact.normal);

            Instantiate(bloodSplatter, other.transform.position, Quaternion.Euler(new Vector3(angle, 90, -90)));

            Debug.Log("Hit normal: " + contact.normal);
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
