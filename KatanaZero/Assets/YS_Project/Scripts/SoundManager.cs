using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    
   
    [SerializeField] private AudioClip[] slashClip;
    private static SoundManager instance;
    AudioSource soundEffect;
  

   
    // Start is called before the first frame update
    void Start()
    {
       
        soundEffect = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void AttackSound()
    {
        int randomIdx = Random.Range(0, 4);
        soundEffect.clip = slashClip[randomIdx];
        soundEffect.Play();
    }
}
