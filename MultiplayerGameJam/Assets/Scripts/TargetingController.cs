using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [Header("Targeting Controller Settings")]
    [SerializeField] private int maxTargetCount;
    [SerializeField] private float targetingDelay;
    [SerializeField] private float targetRange;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float damageDebuff;
    [SerializeField] private bool isAutoTarget;
    [SerializeField] private GameObject _reticlePrefab;
    [SerializeField] private GameObject _targetRange;

    [Header("Target Lists")]
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private List<GameObject> potentialTargets = new List<GameObject>();

    void Start() 
    {
        TargetRange.foundTarget += AddTarget;
        TargetRange.lostTarget += RemoveTarget;
    }

    void Update() 
    {
        _targetRange.GetComponent<TargetRange>().SetRange(targetRange);

        // if a target is removed, look for a potentialTArget that does not exists in the current target list
        // use the potential target as a replacement
    }

    void AddTarget(GameObject target)  
    {
        if (targets.Count < maxTargetCount) 
        {
            targets.Add(target);
            Debug.Log("targets.Count");
            Debug.Log(targets.Count);

            GameObject reticle = Instantiate(_reticlePrefab, target.transform.position, Quaternion.identity);
            reticle.transform.parent = target.transform;
            reticle.GetComponent<Reticle>().SetTarget(target);
        }

        if (!potentialTargets.Contains(target))
        {
            potentialTargets.Add(target);
        }
    }

    void RemoveTarget(GameObject target) 
    {
        targets.Remove(target);
        potentialTargets.Remove(target);

        foreach(GameObject potentialTarget in potentialTargets)
        {
            if (!targets.Contains(potentialTarget))
            {
                AddTarget(potentialTarget);
            }
        }

        Debug.Log("targets.Count");
        Debug.Log(targets.Count);
    }
    
    public void UpdateTargetingProperties(PartScriptableObject part) 
    {
        if (part == null) 
        {
            // TODO: get default values
            maxTargetCount = 0;
            targetingDelay = 0f;
            targetRange = 1.5f;
            rechargeTime = 0f;
            damageDebuff = 0f;
            isAutoTarget = false;
            return;
        }
        
        if (part.type == PartType.TARGETING)
        {
            maxTargetCount = part.targetCount;
            targetingDelay = part.targetingDelay;
            targetRange = part.targetRange;
            rechargeTime = part.rechargeTime;
            damageDebuff = part.damageDebuff;
            isAutoTarget = part.isAutoTarget;
        }
    }
}
