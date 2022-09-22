using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm { get; private set; }
    public float timer = 1000f;
    public int robotsLeft = 0;

    private void Awake()
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (gm != null && gm != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            gm = this; 
        } 

        EventManager.current.onRobotSpawn += OnRobotSpawn;
        EventManager.current.onRobotDestroy += OnRobotDestroy;
    }

    void OnRobotSpawn() {
        robotsLeft++;
    }

    void OnRobotDestroy() {
        robotsLeft--;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        timer -= 1f * Time.deltaTime;

        if (robotsLeft <= 1)
        {
            var robots = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log("WINNER!");
            Debug.Log(robots[0]);
            SceneManager.LoadScene("VictoryScreen");
        }
    }
}
