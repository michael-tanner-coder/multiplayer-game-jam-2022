using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
  private Timer life = new Timer();
  private Vector3 _direction { get; set; }
  private float _damage = 10f;
  private GameObject _target;
  [SerializeField] private float _speed = 10f;

  public void SetDirection(Vector3 direction)
  {
    _direction = direction.normalized;
  }

  public void SetDamage(float damage)
  {
    _damage = damage;
  }

  public void SetTarget(GameObject target)
  {
    _target = target;
  }

  public void Init()
  {
    life = Timer.CreateFromSeconds(5.0f);
    GetComponent<Rigidbody2D>().velocity = _direction * _speed;
    PlaySound();
  }

  public void PlaySound() 
  {
    SoundManager.instance.Play("Shoot");
  }

  public void Update() 
  {
    life.Update();

    if (_target) 
    {
      Vector3 targetDirection = _target.transform.position - transform.position;
      GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * _speed;
    }

    if (life.Expired())
    {
        Destroy(gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D other) 
    { 
      // Damage any game object with a health component
      if (other.gameObject.GetComponent<Health>() != null) 
      {
          Health health = other.gameObject.GetComponent<Health>();
          health.TakeDamage(_damage);
      }

      // Destroy any destructible objects
      if (other.gameObject.GetComponent<IDestructible>() != null) 
      {
        IDestructible destructible = other.gameObject.GetComponent<IDestructible>();
        destructible.Destruct();
      }
      
      // Destroy self after impact
      if (other.gameObject.tag != "Projectile")
      {
        Destroy(gameObject);
      }
    }
}