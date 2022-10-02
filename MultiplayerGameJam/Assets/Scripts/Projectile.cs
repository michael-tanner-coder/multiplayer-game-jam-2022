using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
  [Networked] private TickTimer life { get; set; }
  private Vector3 _direction { get; set; }
  [SerializeField] private float _speed = 10f;
  public GameObject shooter;

  public void SetDirection(Vector3 direction)
  {
    _direction = direction.normalized;
  }

  public void Init()
  {
    life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    GetComponent<Rigidbody2D>().velocity = _direction * _speed;
  }

  public override void FixedUpdateNetwork()
  {
    if (life.Expired(Runner))
    {
        Runner.Despawn(Object);
    }
  }

  // void OnTriggerEnter2D(Collider2D other) 
  //   { 
  //       if (other.gameObject.GetInstanceID() != shooter.GetInstanceID()) 
  //       {
  //           GameObject parent = other.gameObject.transform.parent.gameObject;
  //           if (parent.tag == "Player") 
  //           {
  //               Health health = parent.GetComponent<Health>();
  //               health.TakeDamage(10f);
  //           }
            
  //           Destroy(gameObject);
  //       }
  //   }

  void OnCollisionEnter2D(Collision2D other) 
    { 
      GameObject parent = other.gameObject.transform.parent.gameObject;
      if (parent.tag == "Player") 
      {
          Health health = parent.GetComponent<Health>();
          health.TakeDamage(10f);
      }
      
      Destroy(gameObject);
    }
}