using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
  [Networked] private TickTimer life { get; set; }
  private Vector3 _direction { get; set; }
  [SerializeField] private float _speed = 10f;

  public void SetDirection(Vector3 direction)
  {
    _direction = direction.normalized;
    Debug.Log("SetDirection: " + _direction);
  }

  public void Init()
  {
    life = TickTimer.CreateFromSeconds(Runner, 5.0f);
  }

  public override void FixedUpdateNetwork()
  {
    if (life.Expired(Runner))
    {
        Runner.Despawn(Object);
    }
    else 
    {
        transform.position += _speed * _direction * Runner.DeltaTime;
    }
  }
}