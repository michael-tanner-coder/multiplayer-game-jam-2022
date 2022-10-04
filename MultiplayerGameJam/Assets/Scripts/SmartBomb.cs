using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBomb : MonoBehaviour, IProjectile
{
    private Rigidbody2D _rb;
    private float speed = 10f;
    
    public void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Start() 
    {
        WeaponController.onShootAttempt += Explode;
    }
    
    public void Init()
    {
        Debug.Log("SPAWNED SMART BOMB");
    }

    public void SetDirection(Vector3 direction)
    {
        _rb.AddForce(direction * speed);
    }

    public void Explode() 
    {
        WeaponController.onShootAttempt -= Explode;
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    { 
      if (other.gameObject.GetComponent<IProjectile>() != null|| other.gameObject.GetComponent<Health>() != null) 
      {
        Explode();
      }
    }
}
