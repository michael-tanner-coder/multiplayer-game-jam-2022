using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Controller Settings")]
    public float recoil = 0f;
    public float fireRate = 0f;
    public float reloadTime = 0f;
    public float chargeTime = 0f;
    public float damage = 0f;
    public bool automatic = false;

    [Header("Timers")]
    private Timer reload = new Timer();
    private Timer charge = new Timer();
    private Timer timeBetweenShots = new Timer();
    private Timer timeUntilCooldown = new Timer();


    void Update() 
    {
        // Update all timers
        reload.Update();
        charge.Update();
        timeBetweenShots.Update();
        timeUntilCooldown.Update();

        // Check for fire input
    }

    public void UpdateWeaponProperties(PartScriptableObject part) 
    {
        recoil = part.recoil;
        fireRate = part.fireRate;
        reloadTime = part.reloadTime;
        chargeTime = part.chargeTime;
        automatic = part.automatic;
        damage = part.damage;
    }
}
