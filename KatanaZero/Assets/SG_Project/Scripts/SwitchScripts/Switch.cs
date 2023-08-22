using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Switch : MonoBehaviour
{
    // �ٸ�Ŭ�������� �˾ƾ��ϴ� bool�� �Ű������� �޴� �̺�Ʈ����
    public event Action<bool> switchButtionboolChanged;

    // ������ ����ġ On,Off �������� bool
    private bool isSwitchOn = true;

    // ����ġ �ٲ܋� ��ø� true�� �ؼ� ������ �̺�Ʈ �߻� �Ƚ�Ű������ bool ����
    public bool isTouchButton = false;

    public bool IsSwitchOn
    {        
        get {  return isSwitchOn; }
        set
        {
            //Debug.Log("Set�� ����?");
            if (isSwitchOn != value) // && isTouchButton == true && switchControlNum == 1
            {
                //Debug.Log("�̺�Ʈ�� ������?");
                isSwitchOn = value;
                // �̺�Ʈ ȣ�� �� �Ű����� ����
                switchButtionboolChanged?.Invoke(isSwitchOn);
            }
            else { /*PASS*/ }
        }
    }

    private int switchControlNum = 0;

    public SpriteRenderer switchSpriteRenderer;

    /// <summary>
    /// sprite [1] == ��  sprite [0] == ����
    /// </summary>
    public Sprite[] switchSprite = new Sprite[2];

    // �ڷ�ƾ ĳ��
    private Coroutine touchButton;

    private WaitForFixedUpdate waitForFixedUpdate = default;


    // �����
    private AudioSource audioSource;

    /// <summary>
    /// [0] = Switch On [1] = Switch Off
    /// </summary>
    [SerializeField] AudioClip[] audioClip;

    public void Awake()
    {
        //isSwitchOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //switchSprite = new Sprite[direction];
        switchSpriteRenderer = GetComponent<SpriteRenderer>();

        if(waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }
        if (audioSource == default || audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else { /*PASS*/ }



    }

    // Update is called once per frame
    void Update()
    {
        ChangeSwitch();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switchControlNum = 1;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switchControlNum = 0;
        }
    }

    public void ChangeSwitch()
    {
        if (switchControlNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                
                //  { �����̽� �Է��ϸ� ����ġ ��,���� ����
                if (IsSwitchOn == true && isTouchButton == false)
                {
                    //����ġ ������ �Ҹ�
                    audioSource.clip = audioClip[1];
                    audioSource.Play();
                    // sprite [0] == ����
                    switchSpriteRenderer.sprite = switchSprite[0];
                    //Debug.LogFormat("�� : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = false;
                    touchButton = StartCoroutine(ChangeTouchSwitch());

                }
                else if (IsSwitchOn == false && isTouchButton == false)
                {
                    //����ġ ������ �Ҹ�
                    audioSource.clip = audioClip[0];
                    audioSource.Play();

                    // sprite [1] == ��
                    switchSpriteRenderer.sprite = switchSprite[1];
                    //Debug.LogFormat("�� : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = true;
                    touchButton = StartCoroutine(ChangeTouchSwitch());
                }
                //Debug.LogFormat("IsSwitchOn �� : {0}", IsSwitchOn);
                //  { �����̽� �Է��ϸ� ����ġ ��,���� ����
            }
            else { /*PASS*/ }
        }
    }


    // ��ġ�������� ��� True�� �ΰ� false�� �ٲ��� �ڷ�ƾ
    IEnumerator ChangeTouchSwitch()
    {
        isTouchButton = true;
        for(int i =0; i <= 5; i++) { yield return waitForFixedUpdate; }
        isTouchButton = false;
    }


}   // NameSpace
