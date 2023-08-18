using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMusic : MonoBehaviour
{
    AudioSource bgm;
    TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
        bgm = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.isTimeSlow==true)
        {
            bgm.pitch = 0.5f;
        }
        else if(timeManager.isTimeSlow==false)
        {
            bgm.pitch = 1;
        }

    }
}
