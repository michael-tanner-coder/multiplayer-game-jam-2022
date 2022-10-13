using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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

    [Header("Aiming")]
    public Vector3 aimDirection;

    // Events
    public delegate void OnShoot(Vector3 recoilDirection, float recoilAmount, GameObject shooter);
    public static OnShoot onShoot;
    
    public delegate void OnShootAttempt(GameObject shooter);
    public static OnShootAttempt onShootAttempt;

    // Controller communication
    private TargetingController _tc;
    private PlayerInput _pInput;

    void Start() 
    {
        _tc = GetComponent<TargetingController>();
        _pInput = GetComponent<PlayerInput>();
    }

    IProjectile SpawnProjectile(Vector3 projectileDirection, float projectileDamage) 
    {
        GameObject projectile = Instantiate(_prefabProjectile, transform.position, Quaternion.identity);
        IProjectile projectileInterface = projectile.GetComponent<IProjectile>();
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        projectileInterface.SetDirection(projectileDirection.normalized);
        projectileInterface.SetDamage(projectileDamage);
        projectileInterface.SetOwner(gameObject);
        projectileInterface.Init();
        return projectileInterface;
    }

    void Update() 
    {
        // Update all timers
        charge.Update();
        cooldown.Update();
        timeBetweenShots.Update();
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

    public void Shoot(InputAction.CallbackContext context)
    {
        // Check for fire input
        if (timeBetweenShots.ExpiredOrNotRunning()) {
            if ((!automatic && context.interaction is PressInteraction || (automatic && context.interaction is HoldInteraction)))
            {
                // don't fire while automatic is cooling down
                if (automatic && cooldown.isRunning)
                {
                    return;
                }
                
                // get vector for projectile based on current aiming direction
                // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                // mousePosition.z = transform.position.z;
                // Vector3 aimDirection = mousePosition - transform.position;

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
                onShoot?.Invoke(recoilDirection, recoil, gameObject);

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

    public Vector3 GetAimDirection()
    {
        return aimDirection;
    }

    public void Aim(InputAction.CallbackContext context)
    {
        // get vector for projectile based on current aiming direction
        Vector2 positionInput = context.ReadValue<Vector2>(); // for gamepad
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(positionInput); // for mouse

        if (_pInput.currentControlScheme == "Keyboard")
        {
            aimDirection = mousePosition.normalized; 
        }

        if (_pInput.currentControlScheme == "Controller") 
        {
            aimDirection = positionInput.normalized; 
        }
    }

    public void onAltFire(InputAction.CallbackContext context)
    {
        ActivateSmartBomb();
    }

    public void ActivateSmartBomb()
    {
        onShootAttempt?.Invoke(gameObject);
    }
}
