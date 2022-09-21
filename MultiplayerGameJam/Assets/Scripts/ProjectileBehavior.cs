using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 4f;
    public Vector3 direction = new Vector3(1f, 0f, 0f);

    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    void OnCollisionEnter2D(Collision2D other) 
    { 
        if (other.gameObject.tag != "Player") 
        {
            Destroy(gameObject);
        }
    }
}
