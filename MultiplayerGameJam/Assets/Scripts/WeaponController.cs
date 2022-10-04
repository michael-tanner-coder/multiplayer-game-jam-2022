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
    [SerializeField] private Projectile _prefabProjectile;

    [Header("Timers")]
    private Timer charge = new Timer();
    private Timer timeBetweenShots = new Timer();
    private Timer cooldown = new Timer();

    // Events
    public delegate void OnShoot(Vector3 recoilDirection, float recoilAmount);
    public static OnShoot onShoot;

    void Update() 
    {
        // Update all timers
        charge.Update();
        cooldown.Update();
        timeBetweenShots.Update();

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

                // spawn the projectile with a normalized direction
                Projectile projectile = Instantiate(_prefabProjectile, transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
                projectile.SetDirection(aimDirection.normalized);
                projectile.Init();

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
        recoil = part.recoil;
        fireRate = part.fireRate;
        chargeTime = part.chargeTime;
        automatic = part.automatic;
        damage = part.damage;
        cooldownTime = part.cooldownTime;
        fireTime = part.fireTime;
        timeUntilCooldown = part.fireTime;
    }
}
