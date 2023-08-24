using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TutorialManager : MonoBehaviour
{
    TutorialPlayerFirstControlScript tutorialPlayer;

    TutorialGunEnemyScript tutorialEnemy;

    // { LEGACY : �޹��
    public GameObject timeBackGround;
    SpriteRenderer timeBackGroundRenderer;
    Color timeBackGroundColor;
    // } LEGACY : �޹��

    public GameObject playerClickImg;
    public GameObject backGroundLight;
    public GameObject playerLight;
    public GameObject enemyLight;
    public Light2D globalLight;


    public GameObject nextStageLight;


    Coroutine coroutineBoxing;
    WaitForFixedUpdate waitForFixed;
    WaitForSeconds waitForSeconds;

    private AudioSource audioSource;
    //  0��° �迭 = �������� �Ҹ�    1��° �迭 = �층 �Ҹ�
    [SerializeField] AudioClip[] audioClip;

    bool enemyShotEventTime = false;
    bool didTimeScaleEvent = false;


    float backGroundRgb_A = 4;

    
    void Start()
    {
        nextStageLight.SetActive(false);
        backGroundLight.SetActive(true);


        FirstInIt();
        tutorialEnemy.enemyShotEvent += EnemyShotBoolEvent;
        //globalLight = GetComponent<Light2D>();
        globalLight = backGroundLight.GetComponent<Light2D>();
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = 0.3f;

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

        timeBackGroundRenderer = timeBackGround.GetComponent<SpriteRenderer>();
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
        enemyLight.SetActive(true);
        playerLight.SetActive(true);

        audioSource.clip = audioClip[0];
        audioSource.Play();
        for(int i = 0; i <= 5; i++)
        {
            Time.timeScale -= 0.15f;

            for(int j =0; j <= 10; j++)
            {
                timeBackGroundColor = globalLight.color;

                timeBackGroundColor.r = timeBackGroundColor.r - backGroundRgb_A;
                timeBackGroundColor.g = timeBackGroundColor.g - backGroundRgb_A;
                timeBackGroundColor.b = timeBackGroundColor.b - backGroundRgb_A;

                globalLight.color = timeBackGroundColor;

            }
            #region LEGACY : �޹�� SpareObj ����
            //for (int j = 0; j <= 10; j++)
            //{
            //    timeBackGroundColor = timeBackGroundRenderer.color;
            //    //Debug.LogFormat("{0}", timeBackGroundRenderer.color);
            //    timeBackGroundColor.a += backGroundRgb_A;
            //    timeBackGroundRenderer.color = timeBackGroundColor;
            //}
            #endregion LEGACY : �޹�� SpareObj ����
            yield return waitForSeconds;
            
        }

        playerClickImg.SetActive(true);
        audioSource.clip = audioClip[1];
        audioSource.Play();
        Time.timeScale = 0f;


    }

}
