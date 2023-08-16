using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlyingEnemyScript : MonoBehaviour
{

    Coroutine enemyStart;
    WaitForSeconds waitForSecond;

    Rigidbody2D rigid;


    void Start()
    {
        waitForSecond = new WaitForSeconds(0.25f);
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        // ON

        enemyStart = StartCoroutine(StartEnemy());
    }

    public void OnDisable()
    {
        // Off 

    }


    private IEnumerator StartEnemy()
    {
        for(int i =0; i <= 16; i++)
        {
            yield return waitForSecond;
        }

        this.rigid.AddForce(new Vector2(6f, 2f),ForceMode2D.Impulse);


    }
}
