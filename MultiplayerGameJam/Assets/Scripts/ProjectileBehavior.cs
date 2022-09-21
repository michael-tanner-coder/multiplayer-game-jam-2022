using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 4f;
    public Vector3 direction = new Vector3(1f, 0f, 0f);
    public GameObject shooter;

    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    void OnCollisionEnter2D(Collision2D other) 
    { 
        if (other.gameObject != shooter) 
        {
            if (other.gameObject.tag == "Player") {
                Health health = other.gameObject.GetComponent<Health>();
                health.TakeDamage(10f);
            }
            
            Destroy(gameObject);
        }
    }
}
