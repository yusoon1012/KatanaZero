using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private EnemyRay enemyParent;
   
    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyRay>();
      

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        gameObject.SetActive(false);
        enemyParent.target = collision.transform;
        enemyParent.inRange = true;
        enemyParent.hotZone.SetActive(true);



    }

}
