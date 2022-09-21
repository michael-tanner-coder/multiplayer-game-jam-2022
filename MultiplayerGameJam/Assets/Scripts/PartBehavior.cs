using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehavior : MonoBehaviour
{
    public PartScriptableObject part;

    void OnCollisionEnter2D(Collision2D other) 
    { 
        if (other.gameObject.tag == "Player") 
        {
            PartSlots slots = other.gameObject.GetComponent<PartSlots>();
            if (slots) 
            {
                slots.SetSlot(part);
            }
            
            Destroy(gameObject);
        }
    }
}
