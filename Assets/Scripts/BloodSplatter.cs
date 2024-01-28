using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    //This script exists solely for optimisation of the stop time for blood splatter particle


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bloodDecay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator bloodDecay()
    {
        yield return new WaitForSeconds(1f);       //a second is a good amount
        Destroy(this.gameObject);
    }
}
