using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clownborder : MonoBehaviour
{
    public Sprite low;
    public Sprite medium;
    public Sprite high;
    public Sprite full;
    public static int clownStatus = 0;
    private Image Border;
    void Start()
    {
        Border = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (clownStatus)
        {

            case 0:
                if (!PlayerController.isCrazy) {
                    Border.sprite = low;
                }
            
                break;
            case 1:
                if (!PlayerController.isCrazy)
                {
                    Border.sprite = medium;
                }
                break;
            case 2:
                if (!PlayerController.isCrazy)
                {
                    Border.sprite = high;
                }
                break;
            case 3:
                Border.sprite = full;
                break;
        }
    }
}
