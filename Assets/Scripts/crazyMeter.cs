using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class crazyMeter : MonoBehaviour
{
    public static float clownMeterValue = 0;
    private Slider slider;
    void Start()
    {
        clownMeterValue = 0;
        Clownborder.clownStatus = 0;
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clownMeterValue >= 100 ) { 
        clownMeterValue = 100;
            }
        if (clownMeterValue < 25)
        {
            Clownborder.clownStatus = 0;
        }
        else if (clownMeterValue < 50) {
            Clownborder.clownStatus = 1;
        }
        else if (clownMeterValue < 99)
        {
            Clownborder.clownStatus = 2;
        }
        else if (clownMeterValue >= 100)
        {
            Clownborder.clownStatus = 3;
        }
        slider.value = clownMeterValue; 

    }
}
