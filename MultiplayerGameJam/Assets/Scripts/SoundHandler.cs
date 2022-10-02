using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    public static void PlaySound(string sound_name) 
    {
       FindObjectOfType<SoundManager>().Play(sound_name);
    }
}
