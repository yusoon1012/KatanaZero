using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip slowMotionClip;
    [SerializeField] private AudioClip[] slashClip;
    AudioSource soundEffect;
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        soundEffect = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SlowSound()
    {
        soundEffect.clip = slowMotionClip;
        soundEffect.Play();
    }
    public void AttackSound()
    {
        int randomIdx = Random.Range(0, 4);
        soundEffect.clip = slashClip[randomIdx];
        soundEffect.Play();
    }
}
