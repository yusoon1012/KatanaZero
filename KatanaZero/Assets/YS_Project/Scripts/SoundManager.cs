using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip slowMotionClip;
    [SerializeField] private AudioClip[] slashClip;
    private static SoundManager instance;
    AudioSource soundEffect;
    Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
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
