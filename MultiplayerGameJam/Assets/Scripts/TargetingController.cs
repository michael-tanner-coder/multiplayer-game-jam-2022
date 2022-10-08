using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    
    [Header("Targeting Controller Settings")]
    [SerializeField] private int targetCount;
    [SerializeField] private float targetingDelay;
    [SerializeField] private float targetRange;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float damageDebuff;
    [SerializeField] private bool isAutoTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private GameObject _reticlePrefab;

    void Start() 
    {
        TargetRange.foundTarget += AddTarget;
        TargetRange.lostTarget += RemoveTarget;
    }

    void AddTarget(GameObject target)  
    {
        targets.Add(target);
        Debug.Log("targets.Count");
        Debug.Log(targets.Count);

        GameObject reticle = Instantiate(_reticlePrefab, target.transform.position, Quaternion.identity);
        reticle.GetComponent<Reticle>().SetTarget(target);
    }

    void RemoveTarget(GameObject target) 
    {
        targets.Remove(target);
        Debug.Log("targets.Count");
        Debug.Log(targets.Count);
    }
    
    public void UpdateTargetingProperties(PartScriptableObject part) 
    {
        if (part == null) 
        {
            // TODO: get default values
            targetCount = 0;
            targetingDelay = 0f;
            targetRange = 1.5f;
            rechargeTime = 0f;
            damageDebuff = 0f;
            isAutoTarget = false;
            return;
        }
        
        if (part.type == PartType.TARGETING)
        {
            targetCount = part.targetCount;
            targetingDelay = part.targetingDelay;
            targetRange = part.targetRange;
            rechargeTime = part.rechargeTime;
            damageDebuff = part.damageDebuff;
            isAutoTarget = part.isAutoTarget;
        }
    }
}
