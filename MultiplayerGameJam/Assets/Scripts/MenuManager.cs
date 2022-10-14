using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager { get; private set; }
    
    public void StartMapSelection() 
    {
        SceneManager.LoadScene("MapSelection");
    }

    public void StartRobotSelection() 
    {
        SceneManager.LoadScene("RobotSelection");
    }
    
    public void StartMatch() 
    {
        SceneManager.LoadScene("Arena");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Battle Song");
    }

    public void WinMatch() 
    {
        SceneManager.LoadScene("VictoryScreen");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Victory Song");
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
        // SoundManager.instance.Play("Menu Song");
    }
 
    public void Options() 
    {
        SceneManager.LoadScene("Options");
    }

    public void ToggleFullScreen() 
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetMasterVolume(System.Single volume) 
    {
        AudioSource[] sounds = FindObjectsOfType<AudioSource>() as AudioSource[]; 
        foreach(AudioSource audio in sounds) 
        { 
            audio.volume=volume; 
        }
    }

    public void StartMetalLevel() 
    {
        SceneManager.LoadScene("MetalLevel");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Battle Song");
    }

    public void StartIceLevel() 
    {
        SceneManager.LoadScene("IceLevel");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Battle Song");
    }

    public void StartSandLevel() 
    {
        SceneManager.LoadScene("SandLevel");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Battle Song");
    }

    public void StartGapLevel() 
    {
        SceneManager.LoadScene("GapLevel");
        SoundManager.instance.StopAll();
        SoundManager.instance.Play("Battle Song");
    }

}
