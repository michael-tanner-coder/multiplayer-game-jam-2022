using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject explosion;
    
    public void Init(){}

    public void SetDirection(){}

    public void Explode() 
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    { 
      if (other.gameObject.GetComponent<Health>() != null)
      {
        Explode();
      }
    }
}
