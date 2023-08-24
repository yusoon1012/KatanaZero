using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSound : MonoBehaviour
{
   
    public AudioClip laserHitSound;
    private AudioSource laserSource;
    private bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        laserSource = GetComponent<AudioSource>();
        laserSource.clip = laserHitSound;
    }

  
   
    public void LaserHitSound()
    {

        laserSource.clip = laserHitSound;
        if(isPlaying==false)
        {
            isPlaying = true;
        laserSource.Play();
            
        }
        if (laserSource.isPlaying == false)
        {

            isPlaying = false;

        }
    }
}
