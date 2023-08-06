using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Normal : MonoBehaviour
{
    Animator enemyAni;
    // Start is called before the first frame update
    void Start()
    {
        enemyAni=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Die()
    {
        enemyAni.SetTrigger("Die");
    }
}
