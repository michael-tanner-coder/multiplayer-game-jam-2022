using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Texture2D cursorTexture;
    void Start()
    {
        // Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
    
    // void Update()
    // {
    //     // Vector3 mousePosition = Input.mousePosition;
    //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     transform.position = mousePosition;
    // }
}
