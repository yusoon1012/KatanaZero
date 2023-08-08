using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  AudioClip cassetClip;
    public  AudioClip backgroundClip;
    public AudioClip SlowmotionClip;
    public  AudioSource bgm;
    private bool isSlow=false;
    TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = backgroundClip;
        timeManager = FindAnyObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
       if( timeManager.isTimeSlow==true)
        {
            if(isSlow==false)
            {
            bgm.PlayOneShot(SlowmotionClip);
                isSlow = true;
            }
            bgm.pitch = 0.4f;
        }
       else
        {
            bgm.pitch = 1f;
            isSlow = false;
        }
    }
    public void IntroAction()
    {
        bgm.Stop();
        bgm.PlayOneShot(cassetClip);
        StartCoroutine(IntroTimer());
    }
    private IEnumerator IntroTimer()
    {
        yield return new WaitForSeconds(3);
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
    }
}
