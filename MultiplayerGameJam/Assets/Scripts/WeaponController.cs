using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 firingDirection;
    public float fireRate = 3f;

    void Start() 
    {
        firingDirection = new Vector3(1f, 0f, 0f);
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0) {firingDirection = new Vector3(1f, 0f, 0f);}
        if (Input.GetAxis("Horizontal") < 0) {firingDirection = new Vector3(-1f, 0f, 0f);}
        if (Input.GetAxis("Vertical") > 0) {firingDirection = new Vector3(0f, 1f, 0f);}
        if (Input.GetAxis("Vertical") < 0) {firingDirection = new Vector3(0f, -1f, 0f);}

        if (Input.GetButtonDown("Attack"))
        {
            GameObject newBullet = Instantiate(bullet, gameObject.transform.position, transform.rotation);
            newBullet.GetComponent<Projectile>().shooter = gameObject;
            Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
