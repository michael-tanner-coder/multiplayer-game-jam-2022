using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Vector3 scale = new Vector3(1f, 1f, 1f);
    private float startingScale = 1.0f;
    private float maxScale = 5f;
    private float timeElapsed = 0f;
    private float explosionDuration = 1f;
    private float explosionDamage = 10f;

    void Start()
    {
      SoundManager.instance.Play("Explosion");
    }

    void Update() 
    {
        if (timeElapsed < explosionDuration)
        {
            float currentScale = Mathf.Lerp(startingScale, maxScale, timeElapsed/explosionDuration);
            timeElapsed += Time.deltaTime;
            transform.localScale  = new Vector3(scale.x * currentScale, scale.y * currentScale, scale.z * currentScale);
        }
        else 
        {
            startingScale = maxScale;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    { 
      if (other.gameObject.GetComponent<Health>() != null) 
      {
        Health health = other.gameObject.GetComponent<Health>();
        health.TakeDamage(explosionDamage);
      }

      // Destroy any destructible objects
      if (other.gameObject.GetComponent<IDestructible>() != null) 
      {
        IDestructible destructible = other.gameObject.GetComponent<IDestructible>();
        destructible.Destruct();
      }
    }
}
