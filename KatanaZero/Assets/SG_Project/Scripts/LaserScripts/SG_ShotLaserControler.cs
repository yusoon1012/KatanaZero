using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SG_ShotLaserControler : MonoBehaviour
{
    // �÷��̾�� ��������� �÷��̾� ���ó���� ���� Class����
    public SG_PlayerMovement player = default;

    public GameObject dottedLine;

    private Switch switchClass;

    private SpriteRenderer spriteRenderer = null;

    // �÷� ��,��,��,�� �� �ٲٴ� �ڷ�ƾ ����Ϸ��� �Ʒ� �ּ� ����ϸ��
    private Coroutine changeColor = default;
    // changeColor = StartCoroutine(ColorChange());

    private WaitForFixedUpdate waitForFixedUpdate = default;

    private Color32 blue = new Color32(40, 130, 220, 255);
    private Color32 yellow = new Color32(255, 180, 0, 255);


    //  ���� Sclae��
    private Vector3 defaultScale = default;
    // �ٿ����ϴ� Sclae��
    private Vector3 shrinkageScale = default;

    //  ����sclae - shrinkageScale �ϰ� ������ ��
    private Vector3 reductionScale;

    private bool shotLaserIsbuttonOn = true;

    private AudioSource audioSource;
    public AudioClip[] audioclip;

    // Start is called before the first frame update
    public void Awake()
    {
        Initialization();
    }
    void Start()
    {
        //Debug.LogFormat("R : {0} / G:{1} / B : {2}", blue.r, blue.g, blue.b);
        //spriteRenderer.material.color = blue;
        Initialization();
        switchClass = FindObjectOfType<Switch>();
        switchClass.switchButtionboolChanged += ShotLaserControlerIsButtonOn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
            // SetActive == true �� �� ������ �۾�

        if (shotLaserIsbuttonOn == true)
        {
            Initialization();
            audioSource.Play();
            changeColor = StartCoroutine(ColorChange());
        }
    }

    void OnDisable()
    {
            // SetActive == flase �� �� ������ �۾�

        if (shotLaserIsbuttonOn == true)
        {
            // �ڷ�ƾ���� �پ�� ũ�� False�ɋ��� ����
            this.gameObject.transform.localScale = defaultScale;
            dottedLine.SetActive(true);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //  �������� �������� �÷��̾���
        if (collision.gameObject.CompareTag("Player"))
        {
            // �����ؿ;��� ������ ��������� �׋��� ������
            if (player == default || player == null)
            {
                player = FindObjectOfType<SG_PlayerMovement>();
            }
            else { /*PASS*/ }

     

        }
    }

    // �ʱ�ȭ ���ִ� �Լ�
    public void Initialization()
    {
        // ��������Ʈ �������� ����ִٸ� GetComponent �Ⱥ�������� Pass
        if (spriteRenderer == default || spriteRenderer == null)
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        else { /*PASS*/ }


        // { LEGACY : Color32 �� �̷��� ���� �Ұ�
        //// �Ķ��� RGB�� ������ ���������� �� RGB ����
        //if (blue == default || blue == null)
        //{
        //    blue = new Color32(40, 130, 220,255);
        //}
        //else { /*PASS*/ }

        //// ����� RGB�� �� ���� ���������� �� RGB ����
        //if (yellow == default || yellow == null)
        //{
        //    yellow = new Color32(255, 180, 0,255);
        //}
        //else { /*PASS*/ }
        // } LEGACY : Color32 �� �̷��� ���� �Ұ�


        //  waitForFixedUpdate �� ����������� ĳ��
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }

        // ������ ��鼭 �پ�� �� ������ ����ϱ⿡ ĳ��
        if (shrinkageScale == default || shrinkageScale == null)
        {
            shrinkageScale = new Vector3(0.02f, 0f, 0f);
        }
        else { /*PASS*/ }

        // Scale X �� ���̰� Active = false �Ҷ��� ���� ũ��� ���ƿ����� ����
        if (defaultScale == default || defaultScale == null)
        {
            defaultScale = new Vector3(0.25f, 4.93f, 1f);
        }
        else { /*PASS*/ }

        // ����� �ҽ��� ����������� �Ҵ�
        if (audioSource == default || audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else { /*PASS*/ }
        if (audioSource != null || audioSource != default)
        {
            audioSource.clip = audioclip[0];
        }
        else { /*PASS*/ }
    }

    private void ShotLaserControlerIsButtonOn(bool buttonSwitch)
    {
        shotLaserIsbuttonOn = buttonSwitch;
    }

    public void LaserAttack()
    {

    }

    IEnumerator ColorChange()
    {

        //  {��,��,��,�� ����
        spriteRenderer.color = blue;
        for (int i = 0; i <= 4; i++)
        {

            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 4; j++)
        {
            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = blue;

        for (int j = 0; j <= 4; j++)
        {
            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }
        //  }��,��,��,�� ����

        spriteRenderer.color = yellow;

        this.gameObject.SetActive(false);


    }
}
