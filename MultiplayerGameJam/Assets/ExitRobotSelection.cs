using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRobotSelection : MonoBehaviour
{
    [SerializeField] private int robotCount = 0;
    [SerializeField] private int maxRobotCount;

    private void Start() 
    {
        Player[] players = FindObjectsOfType<Player>();
        maxRobotCount = players.Length;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            robotCount += 1;
        }

        if (robotCount == maxRobotCount)
        {
            SceneManager.LoadScene("MapSelection");
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            robotCount -= 1;
        }
    }
}
