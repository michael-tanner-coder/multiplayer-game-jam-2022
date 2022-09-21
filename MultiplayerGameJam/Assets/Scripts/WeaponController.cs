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
        if (Input.GetKeyDown("right"))  {   firingDirection = new Vector3(1f, 0f, 0f);  }
        if (Input.GetKeyDown("left"))   {   firingDirection = new Vector3(-1f, 0f, 0f); }
        if (Input.GetKeyDown("up"))     {   firingDirection = new Vector3(0f, 1f, 0f);  }
        if (Input.GetKeyDown("down"))   {   firingDirection = new Vector3(0f, -1f, 0f); }
        
        if (Input.GetKeyDown("space")) 
        {
            GameObject newBullet = Instantiate(bullet, gameObject.transform.position, transform.rotation);
            newBullet.GetComponent<ProjectileBehavior>().direction = firingDirection;
        }
    }
}
