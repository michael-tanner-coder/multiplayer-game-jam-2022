using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotType : MonoBehaviour
{
    [SerializeField] Robot robot;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PartSlots>() != null) 
        {
            PartSlots slots = other.gameObject.GetComponent<PartSlots>();
            slots.SetAllSlots(robot);
            SoundManager.instance.Play("Collect");
        }
    }
}
