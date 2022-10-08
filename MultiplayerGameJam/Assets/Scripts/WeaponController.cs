using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Controller Settings")]
    public float recoil = 0f;
    public float fireRate = 0f;
    public float chargeTime = 0f;
    public float damage = 0f;
    public float fireTime = 0f;
    public float cooldownTime = 0f;
    public float timeUntilCooldown = 0f;
    public bool automatic = false;
    [SerializeField] private GameObject _prefabProjectile;

    [Header("Timers")]
    private Timer charge = new Timer();
    private Timer timeBetweenShots = new Timer();
    private Timer cooldown = new Timer();

    // Events
    public delegate void OnShoot(Vector3 recoilDirection, float recoilAmount);
    public delegate void OnShootAttempt();
    public static OnShoot onShoot;
    public static OnShootAttempt onShootAttempt;

    // Controller communication
    private TargetingController _tc;

    void Start() 
    {
        _tc = GetComponent<TargetingController>();
    }

    IProjectile SpawnProjectile(Vector3 projectileDirection, float projectileDamage) 
    {
        GameObject projectile = Instantiate(_prefabProjectile, transform.position, Quaternion.identity);
        IProjectile projectileInterface = projectile.GetComponent<IProjectile>();
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        projectileInterface.SetDirection(projectileDirection.normalized);
        projectileInterface.SetDamage(projectileDamage);
        projectileInterface.Init();
        return projectileInterface;
    }

    void Update() 
    {
        // Update all timers
        charge.Update();
        cooldown.Update();
        timeBetweenShots.Update();

        // Check for smart bomb activation
        if (Input.GetMouseButtonDown(1))
        {
            onShootAttempt?.Invoke();
        }

        // Check for fire input
        if (timeBetweenShots.ExpiredOrNotRunning()) {
            if ((!automatic && Input.GetMouseButtonDown(0) || (automatic && Input.GetMouseButton(0))))
            {
                // don't fire while automatic is cooling down
                if (automatic && cooldown.isRunning)
                {
                    return;
                }
                
                // get vector for projectile based on current aiming direction
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;
                Vector3 aimDirection = mousePosition - transform.position;

                // start delay timer to prevent continous shooting
                timeBetweenShots = Timer.CreateFromSeconds(1/fireRate);

                if (_tc.GetTargets().Count > 0)
                {
                    // foreach target in the tc, spawn a projectile in the direction of the current target
                    foreach(GameObject target in _tc.GetTargets())
                    {
                        Vector3 targetDirection = target.transform.position - transform.position;
                        IProjectile projectileInterface = SpawnProjectile(targetDirection, damage - damage * _tc.damageDebuff);
                        projectileInterface.SetTarget(target);
                    }
                }
                else 
                {
                    // spawn the projectile with a normalized direction
                    SpawnProjectile(aimDirection, damage);
                    
                }

                // enact recoil after firing
                Vector3 recoilDirection = aimDirection.normalized * -1;
                onShoot?.Invoke(recoilDirection, recoil);

                // count down to cooldown phase for automatic weapons
                if (automatic) 
                {
                    HeatupWeapon();
                }
            } 
            else 
            {
                CooldownWeapon();
            } 
        }
    }

    void HeatupWeapon() 
    {
        timeUntilCooldown -= Time.deltaTime;
        if (timeUntilCooldown <= 0 && cooldown.ExpiredOrNotRunning()) 
        {
            cooldown = Timer.CreateFromSeconds(cooldownTime);
            timeUntilCooldown = fireTime;
        }
    }

    void CooldownWeapon() 
    {
        timeUntilCooldown += Time.deltaTime;
        if (timeUntilCooldown >= fireTime) 
        {
            timeUntilCooldown = fireTime;
        } 
    }

    public void UpdateWeaponProperties(PartScriptableObject part) 
    {
        if (part == null) 
        {
             // TODO: need default values
            recoil = 0f;
            fireRate = 0f;
            chargeTime = 0f;
            automatic = false;
            damage = 0f;
            cooldownTime = 0f;
            fireTime = 0f;
            timeUntilCooldown = 0f;
            return;
        }

        if (part.type == PartType.WEAPON)
        {
            recoil = part.recoil;
            fireRate = part.fireRate;
            chargeTime = part.chargeTime;
            automatic = part.automatic;
            damage = part.damage;
            cooldownTime = part.cooldownTime;
            fireTime = part.fireTime;
            timeUntilCooldown = part.fireTime;
            _prefabProjectile = part.projectile;
        }
    }
}
