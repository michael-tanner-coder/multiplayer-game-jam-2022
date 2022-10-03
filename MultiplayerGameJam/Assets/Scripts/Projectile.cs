using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  private Timer life = new Timer();
  private Vector3 _direction { get; set; }
  [SerializeField] private float _speed = 10f;
  public GameObject shooter;

  public void SetDirection(Vector3 direction)
  {
    _direction = direction.normalized;
  }

  public void Init()
  {
    life = Timer.CreateFromSeconds(5.0f);
    GetComponent<Rigidbody2D>().velocity = _direction * _speed;
  }

  public void Update() 
  {
    life.Update();

    if (life.Expired())
    {
        Destroy(gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D other) 
    { 
      GameObject parent = other.gameObject.transform.parent.gameObject;
      if (parent.GetComponent<Health>() != null) 
      {
          Health health = parent.GetComponent<Health>();
          health.TakeDamage(10f);
      }
      
      Destroy(gameObject);
    }
}