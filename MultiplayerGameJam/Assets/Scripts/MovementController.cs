using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MovementController : NetworkBehaviour
{
    private float speed = 0f;
    private float targetSpeed = 0f;
    private float maxSpeed = 8.0f;
    private float mass = 1f;
    private float timeToMaxSpeed = 2;

    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    private Vector3 deceleration = Vector3.zero;
    private Vector3 movement = Vector3.zero; 

    public void Update()
    {
      float changeRatePerSecond = 1 / timeToMaxSpeed * Runner.DeltaTime;
      targetSpeed = 0;

      if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) 
      {
        targetSpeed = maxSpeed;
      }

      if (Input.GetKey(KeyCode.LeftShift))
      {
          // Reach destination value twice as fast
          // when Shift is held down
          changeRatePerSecond *= 2;
      }

      speed = Mathf.MoveTowards(speed, targetSpeed, changeRatePerSecond);

      // * ----- *
      // clamp speed to maximum limit
      // if (speed < maxSpeed) 
      // {
      //   speed = maxSpeed;
      // }

      // // 
      // velocity += acceleration;
      // acceleration *= 0;

      // movement = velocity * Runnder.DeltaTime;

      // transform.position += velocity * Time.deltaTime;
    }

    public void AddForce(Vector3 force)
    {
        Vector3 f = force;
        f = f / mass;
        
        acceleration += f;

        deceleration += f;
    }

    public void Move(Vector3 direction, float dt) 
    {
      transform.position += direction * speed * dt;
    }
}
