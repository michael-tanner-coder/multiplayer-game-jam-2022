using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
  [Networked] private TickTimer life { get; set; }
  private Vector3 _direction { get; set; }

  public void SetDirection(Vector3 direction)
  {
    _direction = direction;
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
        transform.position += 5 * _direction * Runner.DeltaTime;
    }
  }
}