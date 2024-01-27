using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        transform.position = new Vector3(mousePos.x, mousePos.y, -5);
    }
}
