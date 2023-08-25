using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_Doctor : MonoBehaviour
{

    //�÷��̾��� ��ġ Ȯ�������� ����
    public Transform playerPotision;

    Animator animator;
    Rigidbody2D rigid;

    Coroutine coroutine;
    WaitForFixedUpdate waitForFixedUpdate;

    public GameObject doctorHeadPrefab;
    GameObject headClone;
    SpriteRenderer spriteRenderer;

    AudioSource audioSource;
    BoxCollider2D doctorCollider;
    /// <summary>
    /// [0] = �ڻ� ������ϴ¼Ҹ� [1] = ���� ��ź�� ���� �Ҹ� [2] = �� ��ź�� ������ �Ҹ�
    /// </summary>
    public AudioClip[] audioClip;

    // �÷��̾�� �������� Ȯ��
    private bool meetPlayer = false;
    // ó�� ���� �Ͼ�� �ƽ��� ����Ǿ����� Ȯ��
    private bool isStandCutin = false;
    //�ڻ��� �Ӹ��� ������������ Ȯ���� ����
    private bool doctorBoomTime = false;
    // �Ӹ��� �������� Ȯ���ϴ� ����
    private bool doctorBoom = false;
    // �ڻ簡 �ٰ� �ִ��� üũ�� ����
    private bool doctorRun = false;
    // �ڻ� �Ӹ��� Instance �ߴ��� Ȯ���� ����
    private bool makeHead = false;

    // �ڻ� cutin �� �������� �˷��� ���� �ڻ簡 �������� 2~3�ʵڿ� ������ ����ǰ� �ϱ����� ����
    public bool isCutinEnding = false;

    private bool neckWarningSoundPlay = false;

    //�÷��̾ �� x ��ǥ����
    //�ڿ� ������ isBack = true
    //�տ� ������ isFront = true
    private bool isFront = false;
    private bool isBack = false;

    Vector3 frontScale;
    Vector3 backScale;


    private float dis;
    private float moveSpeed = 4f;


    public void Awake()
    {
        doctorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        waitForFixedUpdate = new WaitForFixedUpdate();

        audioSource.volume = 0.3f;
        frontScale = new Vector3(1.0f, 1.0f, 1.0f);
        backScale = new Vector3(-1.0f, 1.0f, 1.0f);

        this.gameObject.transform.localScale = backScale;
        animator.Play("Chair");

    }

    void Start()
    {

    }


    void Update()
    {
        dis = Vector3.Distance(playerPotision.position, this.transform.position);
        IsMeet();
        DoctorRun();
        WarningNeckSound();
        MakeHead();

    }


    // PlayCutin 001
    public void IsMeet()
    {
        if (meetPlayer == false && isStandCutin == false)
        {
            if (playerPotision.position.x > 26.3f && playerPotision.position.y > 9f)
            {
                meetPlayer = true;
                coroutine = StartCoroutine(PlayCutin002());
            }

        }
    }

    // PlayCutin 002
    IEnumerator PlayCutin002()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return waitForFixedUpdate;
        }
        animator.SetTrigger("ChairToStandUp");

        for (int i = 0; i < 100; i++)
        {
            yield return waitForFixedUpdate;
        }

        isStandCutin = true;
    }

    // PlayCutin 003
    // �÷��̾�� �����Ÿ��� �����ϸ鼭 �־����� �پ ���󰡾���
    public void DoctorRun()
    {
        // �Ͼ�� �ƽ��� �����ڿ� �ȿ� ���ǿ� ���� ����
        if (meetPlayer == true && isStandCutin == true)
        {
            //�÷��̾ ������ �տ� �ֳ� �ڿ��ֳ��� ���� �����ϰ� �������ִ� if            
            if (this.transform.position.x > playerPotision.position.x && doctorBoomTime == false) //�÷��̾� X �������� �ڻ��� X ������ ���� ������
            {
                this.gameObject.transform.localScale = backScale;
                isBack = true;
                isFront = false;
            }
            else if (this.transform.position.x < playerPotision.position.x && doctorBoomTime == false) //�÷��̾� X �������� �ڻ��� X ������ ���� Ŭ��
            {
                this.gameObject.transform.localScale = frontScale;
                isFront = true;
                isBack = false;
            }

            // �÷��̾ ���ͺ��� X ��ǥ �������� ������
            if (isBack == true && dis > 1.5f && doctorBoomTime == false)
            {
                doctorRun = true;
                animator.SetBool("DoctorRun", doctorRun);

                Vector3 move = new Vector3(-moveSpeed, rigid.velocity.y, 0f);
                rigid.velocity = move;
                //rigid.velocity
            }
            else if (isBack == true && dis <= 1.5f && doctorBoomTime == false)
            {
                doctorRun = false;
                animator.SetBool("DoctorRun", doctorRun);

                rigid.velocity = Vector3.zero;
            }

            // �÷��̾ ���ͺ��� X ��ǥ Ŭ���� ������
            if (isFront == true && dis > 1.5f && doctorBoomTime == false)
            {
                doctorRun = true;
                animator.SetBool("DoctorRun", doctorRun);

                Vector3 move = new Vector3(moveSpeed, rigid.velocity.y, 0f);
                rigid.velocity = move;
                //rigid.velocity
            }
            else if (isFront == true && dis <= 1.5f && doctorBoomTime == false)
            {
                doctorRun = false;
                animator.SetBool("DoctorRun", doctorRun);

                rigid.velocity = Vector3.zero;
            }

            // �Ӹ��� ���� �ð����� Ȯ��
            if (doctorBoomTime == false && this.transform.position.x <= 17f)
            {
                doctorBoomTime = true;
                coroutine = StartCoroutine(DoctorBoom());
            }
            else { /*PASS*/ }


        }
    }   //DoctorRun()

    // PlayCutin 004
    // �ڻ��� �Ӹ� ������ ����� �ڷ�ƾ
    IEnumerator DoctorBoom()
    {
        rigid.velocity = Vector3.zero;

        for (int i = 0; i <= 5; i++)
        {
            yield return waitForFixedUpdate;
        }
        audioSource.clip = audioClip[0];
        audioSource.Play();
        animator.Play("DoctorFall");

        doctorBoom = true;
        isFront = false;
        isBack = false;

        // 10 �ʵ� ���� ������ �ϱ� ���� ������
        yield return new WaitForSeconds(3f);
        isCutinEnding = true;
    }

    //PlayCutin 005
    // sprite �� �Ӹ� ������ �����϶��� �ڻ�Ӹ��� Instance�ϴ��Լ�
    public void MakeHead()
    {
        if(spriteRenderer.sprite.name == ("spr_factorysci_ground_head_explode_3") && makeHead == false)
        {
            audioSource.clip = audioClip[2];
            audioSource.Play();
            makeHead = true;
            headClone = Instantiate(doctorHeadPrefab, this.gameObject.transform.position, this.gameObject.transform.localRotation);

        }
    }

    //!Sound [1]�� Ŭ���� ���� �������� �ڷ�ƾ �ð����� �����ϴµ� �����ӿ� ���� �ٸ��� �ֱ⿡ �ڻ��� ��������Ʈ�� ����
    public void WarningNeckSound()
    {
        if(spriteRenderer.sprite.name == ("spr_factorysci_fall_17") && neckWarningSoundPlay == false)
        {
            neckWarningSoundPlay = true;
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, doctorCollider);
        }
    }

}   //NameSpace
