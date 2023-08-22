using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialGunEnemyScript : MonoBehaviour
{
    #region �̺�Ʈ Action
    public event Action<bool> enemyShotEvent;
    // Start is called before the first frame update
    private bool isShot = false;

    public bool IsShot
    {
        get { return isShot; }

        set
        {
            if (isShot != value)
            {
                isShot = value;
                enemyShotEvent?.Invoke(isShot);
            }
        }
    }
    #endregion

    public GameObject bulletMaker;
    public GameObject bloodPrefab;
    Coroutine letShot;
    Coroutine bloodCorouTine;
    Coroutine enemyTextCoroutine;

    WaitForFixedUpdate waitForFixedUpdate = default;
    WaitForSeconds waitForSeconds = default;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;

    Animator animator;
    AudioSource audioSource;


    public GameObject meetEnemyText;
    public GameObject meetPlayerText;

    // [0] = �������� �Ҹ�    [1] = �� �¾����� �� �Ҹ�
    [SerializeField] AudioClip[] audioClip;

    private GameObject bloodClone;

    //�׾�����
    bool isDead = false;

    // �������� �ִ���
    bool oneMakeBlood = false;

    // �������
    bool Bloodmaking = false;



    void Start()
    {
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        if (rigid == default || rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        if (boxCollider == default || boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        if (animator == default || animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (waitForSeconds == default || waitForSeconds == null)
        {
            waitForSeconds = new WaitForSeconds(0.1f);
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true && oneMakeBlood == false && Bloodmaking == false)
        {
            bloodCorouTine = StartCoroutine(MakeBlood());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.CompareTag("Player"))
        //{
        //    letShot = StartCoroutine(StartShot());
        //}


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            letShot = StartCoroutine(StartShot());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyTextCoroutine = StartCoroutine(MeetEnemyTextStart());
        }

        if (collision.gameObject.CompareTag("SG_Bullet"))
        {
            meetPlayerText.gameObject.SetActive(false);

            // ������ �ڷ� ���ư��°� ����
            if (this.gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {                
                audioSource.clip = audioClip[1];
                audioSource.Play();

                isDead = true;
                // �ڷ� ���ư�
                rigid.AddForce(new Vector2(-13f, 5f), ForceMode2D.Impulse);
                rigid.gravityScale = 1f;
                boxCollider.isTrigger = false;
                animator.SetTrigger("EnemyDieTrigger");

            }
            else if (this.gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                audioSource.clip = audioClip[1];
                audioSource.Play();

                isDead = true;
                // �շ� ���ư�
                rigid.AddForce(new Vector2(13f, 5f), ForceMode2D.Impulse);
                rigid.gravityScale = 1f;
                boxCollider.isTrigger = false;
                animator.SetTrigger("EnemyDieTrigger");
            }
            if (isDead == true)
            {
                audioSource.clip = audioClip[0];
                audioSource.Play();
            }
        }
    }


    private IEnumerator StartShot()
    {
        meetPlayerText.gameObject.SetActive(true);
        //  waitForFixed�� 5�� ���ƾ� 0.1��
        for (int i = 0; i <= 53; i++)
        {
            yield return waitForFixedUpdate;
        }
        bulletMaker.gameObject.SetActive(true);

        IsShot = true;

    }

    private IEnumerator MakeBlood()
    {
        Bloodmaking = true;
        for (int i = 0; i <= 20; i++)
        {
            bloodClone = Instantiate(bloodPrefab, this.transform);
            yield return waitForSeconds;
        }
    }

    private IEnumerator MeetEnemyTextStart()
    {
        meetEnemyText.gameObject.SetActive(true);
        for (int i = 0; i <= 5; i++)
        {
            yield return waitForSeconds;
        }
        meetEnemyText.gameObject.SetActive(false);
    }

}
