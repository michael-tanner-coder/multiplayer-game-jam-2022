using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private List<GameObject> robots = new List<GameObject>();

    public static PlayerData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayerJoin(PlayerInput playerInput) 
    {
        // get player, add to robots list
        robots.Add(playerInput.gameObject);
    }

    public void OnPlayerLeave(PlayerInput playerInput)
    {
        // get player, remove from robots list
        robots.Remove(playerInput.gameObject);
    } 
}
