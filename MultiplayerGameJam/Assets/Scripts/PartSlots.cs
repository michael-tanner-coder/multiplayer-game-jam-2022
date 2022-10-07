using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSlots : MonoBehaviour
{
    
    [SerializeField] public PartScriptableObject weaponSlot;
    
    [SerializeField] public PartScriptableObject mobilitySlot;
    
    [SerializeField] public PartScriptableObject targetingSlot;

    [SerializeField] private GameObject _partPrefab;

    public delegate void OnPartChange(PartScriptableObject newPart);
    public static OnPartChange onPartChange;

    public void SetSlot(PartScriptableObject newPart)
    {
        if (newPart.type == PartType.WEAPON) 
        {
            SwapPart(weaponSlot);
            weaponSlot = newPart;
        }

        if (newPart.type == PartType.MOBILITY) 
        {
            SwapPart(mobilitySlot);
            mobilitySlot = newPart;
        }
        
        if (newPart.type == PartType.TARGETING) 
        {
            SwapPart(targetingSlot);
            targetingSlot = newPart;
        }

        onPartChange?.Invoke(newPart);
    }

    public void SetAllSlots(Robot robot) 
    {
        if (robot.weapon) 
        {
            SetSlot(robot.weapon);
        }
        else 
        {
            weaponSlot = null;
        }

        if (robot.mobility) 
        {
            SetSlot(robot.mobility);
        }
        else 
        {
            mobilitySlot = null;
        }

        if (robot.targeting) 
        {
            SetSlot(robot.targeting);
        }
        else 
        {
            targetingSlot = null;
        }
    }

    public void SwapPart(PartScriptableObject slot) 
    {
        // if (slot != null) 
        // {
        //     Vector3 newPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);

        //     GameObject swappedPart = Instantiate(_partPrefab, newPosition, Quaternion.identity);
        //     PartBehavior swappedPartBehavior = swappedPart.GetComponent<PartBehavior>();
        //     swappedPartBehavior.part = slot;
        // }
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
