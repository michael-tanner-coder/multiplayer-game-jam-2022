using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private Health _health;

    // public void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.tag == "Projectile") {
    //         Debug.Log("HIT");
    //         _health.TakeDamage(5);
    //     }
    // }
}
