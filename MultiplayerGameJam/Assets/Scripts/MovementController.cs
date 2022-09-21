using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    
    public float speed = 4.0f;

    void Update()
    {
      var move = new Vector3(Input.GetAxis("Horizontal" + gameObject.name), Input.GetAxis("Vertical" + gameObject.name), 0);
      transform.position += move * speed * Time.deltaTime;
    }
}
