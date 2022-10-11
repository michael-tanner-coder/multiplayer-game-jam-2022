using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject explosion;
    private Vector3 _direction { get; set; }
    private float _damage = 10f;
    private GameObject _target;
    [SerializeField] private float _speed = 10f;
    
    public void Init()
    {
      GetComponent<Rigidbody2D>().velocity = _direction * _speed;
      SetRotation();
    }

    public void SetDirection(Vector3 direction)
    {
      _direction = direction.normalized;
    }

    public void SetRotation()
    {
      Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
      Vector3 dir = Input.mousePosition - pos;
      float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetDamage(float damage) 
    {
      _damage = damage;
    }

    public void SetTarget(GameObject target)
    {
      _target = target;
    }

    void Update() 
    {
      if (_target) 
      {
        Vector3 targetDirection = _target.transform.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * _speed;
      }
    }

    public void Explode() 
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    { 
        Explode();
    }
}
