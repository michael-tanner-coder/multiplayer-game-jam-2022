using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehavior : MonoBehaviour
{
    public PartScriptableObject part;
    public delegate void OnCollect(PartScriptableObject newPart);
    public static OnCollect onCollect;


    void OnCollisionEnter2D(Collision2D other) 
    { 
        if (other.gameObject.GetComponent<PartSlots>() != null) 
        {
            PartSlots slots = other.gameObject.GetComponent<PartSlots>();
            if (slots) 
            {
                slots.SetSlot(part);
                onCollect?.Invoke(part);
            }
            
            Destroy(gameObject);
        }
    }
}
