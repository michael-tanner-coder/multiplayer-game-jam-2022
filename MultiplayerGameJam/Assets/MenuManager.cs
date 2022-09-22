using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager { get; private set; }

    public void StartMatch() 
    {
        SceneManager.LoadScene("Arena");
    }

    public void WinMatch() 
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    public void LoseMatch() 
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
 
}
