using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    TutorialPlayerFirstControlScript tutorialPlayer;

    TutorialGunEnemyScript tutorialEnemy;

    Coroutine coroutineBoxing;
    WaitForFixedUpdate waitForFixed;
    WaitForSeconds waitForSeconds;

    bool enemyShotEventTime = false;
    bool didTimeScaleEvent = false;

    // Start is called before the first frame update
    void Start()
    {
        FirstInIt();
        tutorialEnemy.enemyShotEvent += EnemyShotBoolEvent;

    }

    // Update is called once per frame
    void Update()
    {
        TimeSclaseEvent();
    }

    private void FirstInIt()
    {
        if (tutorialEnemy == default || tutorialEnemy == null)
        {
            tutorialEnemy = FindObjectOfType<TutorialGunEnemyScript>();
        }
        else { /*PASS*/ }

        if (tutorialPlayer == default || tutorialPlayer == null)
        {
            tutorialPlayer = GetComponent<TutorialPlayerFirstControlScript>();
        }
        else { /*PASS*/ }

        if(waitForSeconds == default || waitForSeconds == null)
        {
            waitForSeconds = new WaitForSeconds(0.1f);
        }
    }

    public void EnemyShotBoolEvent(bool enemyShot)
    {
        enemyShotEventTime = enemyShot;
        //Debug.LogFormat("�̺�Ʈ ������ bool�� -> {0}", enemyShotEventTime);
    }

    public void TimeSclaseEvent()
    {

        if(didTimeScaleEvent == false && enemyShotEventTime == true)
        {
            didTimeScaleEvent = true;
            coroutineBoxing = StartCoroutine(TimeCoroutine());
        }

    }

    public IEnumerator TimeCoroutine()
    {
        for(int i = 0; i <= 5; i++)
        {
            Time.timeScale -= 0.19f;
            yield return waitForSeconds;
            //Debug.LogFormat("�پ��� ���� Ÿ�� ������ -> {0}", Time.timeScale);
        }
        //Debug.LogFormat("�� �پ���� Ÿ�� ������ -> {0}", Time.timeScale);
        Time.timeScale = 0f;
    }
}
