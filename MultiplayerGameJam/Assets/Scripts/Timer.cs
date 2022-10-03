using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float delay = 1;
    public float time;
    public static Timer None = new Timer();
    public bool isRunning = false;

    public void Update() {
        if(time > 0f && isRunning) {
            time -= Time.deltaTime;
            isRunning = true;

            if(time < 0f) time = 0f;
        }

        if(time == 0f){
            isRunning = false;
        }
    }

    public bool Expired() {
        return time == 0f;
    }

    public void Reset() {
        time = delay;
        isRunning = true;
    }

    public bool ExpiredOrNotRunning() {
        return time == 0f || isRunning == false;
    }

    // FIXME: conflicts with ExpiredOrNotRunning
    public void Pause() {
        isRunning = false;
    }

    public static Timer CreateFromSeconds(float seconds) {
        Timer newTimer = new Timer();
        newTimer.delay = seconds;
        newTimer.time = newTimer.delay;
        newTimer.isRunning = true;
        return newTimer;
    }
}
