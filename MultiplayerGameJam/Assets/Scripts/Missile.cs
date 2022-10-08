using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject explosion;
    private Vector3 _direction { get; set; }
    private float _damage = 10f;
    [SerializeField] private float _speed = 10f;
    
    public void Init()
    {
        GetComponent<Rigidbody2D>().velocity = _direction * _speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    public void SetDamage(float damage) 
    {
      _damage = damage;
    }

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
