using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSlots : MonoBehaviour
{
    
    [SerializeField] public PartScriptableObject weaponSlot;

    
    [SerializeField] public PartScriptableObject mobilitySlot;
    
    
    [SerializeField] public PartScriptableObject targetingSlot;

    public void SetSlot(PartScriptableObject newPart)
    {
        if (newPart.type == PartType.WEAPON) 
        {
            weaponSlot = newPart;
        }

        if (newPart.type == PartType.MOBILITY) 
        {
            mobilitySlot = newPart;
        }
        
        if (newPart.type == PartType.TARGETING) 
        {
            targetingSlot = newPart;
        }
    }

    public PartScriptableObject GetSlot(PartType type)
    {
        if (type == PartType.WEAPON) 
        {
           return weaponSlot;
        }

        if (type == PartType.MOBILITY) 
        {
           return mobilitySlot;
        }
        
        if (type == PartType.TARGETING) 
        {
           return targetingSlot;
        }

        return weaponSlot;
    }
}
