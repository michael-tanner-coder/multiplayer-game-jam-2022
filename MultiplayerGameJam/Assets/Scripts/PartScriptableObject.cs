using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType {MOBILITY, WEAPON, TARGETING};
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PartScriptableObject", order = 1)]
public class PartScriptableObject : ScriptableObject
{
    [Header("Part Type")]
    public string name;
    public PartType type;

    [Header("Movement")]
    public float acceleration;
    public float braking;
    public float maxSpeed;
    public float boostPower;
    public float boostRechargeTime;
    public float maxFlySpeed;
    public bool hasBoost;
    public bool canFly;

    [Header("Firing")]
    public float fireRate;
    public float reloadTime;
    public float recoil;
    public bool automatic;
    public float chargeTime;
    public float damage;
    public float cooldownTime;
    public float fireTime;
    public GameObject projectile;

    [Header("Targeting")]
    public int targetCount;
    public float targetRange;
    [Range(0.0f, 1.0f)]
    public float damageDebuff;
}
