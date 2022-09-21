using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSlots : MonoBehaviour
{
    [SerializeField]
    private PartScriptableObject weaponSlot;

    [SerializeField]
    private PartScriptableObject mobilitySlot;
    
    [SerializeField]
    private PartScriptableObject targetingSlot;

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
}
