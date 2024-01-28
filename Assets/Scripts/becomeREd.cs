using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class becomeREd : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fill;
    private void Start()
    {
        fill = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isCrazy) {
            fill.color = new Color(195,0,0);
        }
        if (!PlayerController.isCrazy) {  
            
            fill.color = Color.white;
        
        }
    }
}
