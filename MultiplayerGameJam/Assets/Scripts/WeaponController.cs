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
        if (Input.GetAxis("Horizontal" + gameObject.name) > 0) {firingDirection = new Vector3(1f, 0f, 0f);}
        if (Input.GetAxis("Horizontal" + gameObject.name) < 0) {firingDirection = new Vector3(-1f, 0f, 0f);}
        if (Input.GetAxis("Vertical" + gameObject.name) > 0) {firingDirection = new Vector3(0f, 1f, 0f);}
        if (Input.GetAxis("Vertical" + gameObject.name) < 0) {firingDirection = new Vector3(0f, -1f, 0f);}
        // if (Input.GetKeyDown("right"))  {   firingDirection = new Vector3(1f, 0f, 0f);  }
        // if (Input.GetKeyDown("left"))   {   firingDirection = new Vector3(-1f, 0f, 0f); }
        // if (Input.GetKeyDown("up"))     {   firingDirection = new Vector3(0f, 1f, 0f);  }
        // if (Input.GetKeyDown("down"))   {   firingDirection = new Vector3(0f, -1f, 0f); }
        
        if (Input.GetButtonDown("Attack" + gameObject.name)) 
        {
            GameObject newBullet = Instantiate(bullet, gameObject.transform.position, transform.rotation);
            newBullet.GetComponent<ProjectileBehavior>().direction = firingDirection;
            newBullet.GetComponent<ProjectileBehavior>().shooter = gameObject;
            Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
