using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [Header("Targeting Controller Settings")]
    [SerializeField] private int maxTargetCount;
    [SerializeField] private float targetRange;
    [SerializeField] public float damageDebuff;
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
    }

    void AddTarget(GameObject target)  
    {
        if (targets.Count < maxTargetCount) 
        {
            targets.Add(target);
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
    }

    public List<GameObject> GetTargets() 
    {   
        return targets;
    }
    
    public void UpdateTargetingProperties(PartScriptableObject part) 
    {
        if (part == null) 
        {
            // TODO: get default values
            maxTargetCount = 0;
            targetRange = 1.5f;
            damageDebuff = 0f;
            return;
        }
        
        if (part.type == PartType.TARGETING)
        {
            maxTargetCount = part.targetCount;
            targetRange = part.targetRange;
            damageDebuff = part.damageDebuff;
        }
    }
}
