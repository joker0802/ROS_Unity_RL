using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simple_Timer : MonoBehaviour
{
    private float TimerCounter = 0f;
    private int TimeSec = 0;
    private int TimeMin = 0;
    public Text TextBox;
    void Awake()
    {
        TextBox.text = TimeMin.ToString("D2") + ":" + TimeSec.ToString("D2");
    }

    void Update()
    {
        TimerCounter += Time.deltaTime;
        TimeSec = (int)(TimerCounter%61);
        if (TimeSec == 60)
        {
            TimerCounter = 0;
            TimeSec = 0;
            TimeMin += 1;
        }
        TextBox.text = TimeMin.ToString("D2") + ":" + TimeSec.ToString("D2");
    }
}
