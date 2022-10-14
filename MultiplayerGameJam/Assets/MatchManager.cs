using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    void Update() 
    {
        Player[] robots = FindObjectsOfType<Player>();
        if (robots.Length == 1)
        {
            Debug.Log("WINNER IS: ");
            Debug.Log(robots[0].name);
            SoundManager.instance.StopAll();
            SoundManager.instance.Play("Victory Song");
            SceneManager.LoadScene("VictoryScreen");
        }
    }
}
