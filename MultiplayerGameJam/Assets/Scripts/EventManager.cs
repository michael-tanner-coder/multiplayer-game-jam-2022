using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;
    
    private void Awake() 
    {
        current = this;
    }

    public event Action onRobotSpawn;
    public event Action onRobotDestroy;

    public void SpawnTrigger() 
    {
        if (onRobotSpawn != null) 
        {
            onRobotSpawn();
        }
    }

    public void DestructionTrigger() 
    {
        if (onRobotDestroy != null) 
        {
            onRobotDestroy();
        }
    }
}
