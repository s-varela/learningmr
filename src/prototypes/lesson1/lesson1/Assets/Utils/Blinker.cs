using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Blinker : MonoBehaviour
{

    GameObject component;
    Stopwatch counterBlink;
    Stopwatch counterTotal;
    bool active;
    bool end;
    int totalSeconds;

    // Use this for initialization
    void Start()
    {
        this.counterTotal = new Stopwatch();
        this.counterBlink = new Stopwatch();
    }

    void Restart(Stopwatch counter)
    {
        counter.Reset();
        counter.Start();
    }

    bool ElapsedTime(int seconds, Stopwatch counter)
    {
        return counter.ElapsedMilliseconds > seconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (ElapsedTime(totalSeconds, counterTotal))
        {
            end = true;
        }
        if (!end && ElapsedTime(1000, counterBlink))
        {
            active = component.activeSelf;
            component.SetActive(!active);
            Restart(counterBlink);
            // setActive toggle
        }
    }

    public void SetComponent(ref GameObject component, int totalSeconds)
    {
        this.component = component;
        this.component.SetActive(false);
        this.active = true;
        this.end = false;
        this.totalSeconds = totalSeconds * 1000;
        Restart(counterTotal);
        Restart(counterBlink);
    }
}