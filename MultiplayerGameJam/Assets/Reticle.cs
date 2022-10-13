using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    private GameObject _target;
    
    void Start() 
    {
        TargetRange.lostTarget += OnTargetLost;
    }

    void OnTargetLost(GameObject target, GameObject self) 
    {
        if (target.Equals(_target))
        {
            Destroy(gameObject);
            TargetRange.lostTarget -= OnTargetLost;
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
