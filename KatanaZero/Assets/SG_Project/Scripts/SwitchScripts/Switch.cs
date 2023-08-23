using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Switch : MonoBehaviour
{
    public AudioClip laserOnClip;
    public AudioClip laserOffClip;
    AudioSource laserOnOffSource;
    public GameObject spaceBar;
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

    public Sprite[] switchSprite = new Sprite[2];

    // �ڷ�ƾ ĳ��
    private Coroutine touchButton;

    private WaitForFixedUpdate waitForFixedUpdate = default;

    public void Awake()
    {
        laserOnOffSource = GetComponent<AudioSource>();
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
            spaceBar.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switchControlNum = 0;
            spaceBar.SetActive(false);

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
                    switchSpriteRenderer.sprite = switchSprite[0];
                    //Debug.LogFormat("�� : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = false;
                    touchButton = StartCoroutine(ChangeTouchSwitch());
                    laserOnOffSource.clip = laserOffClip;
                    laserOnOffSource.Play();

                }
                else if (IsSwitchOn == false && isTouchButton == false)
                {
                    
                    switchSpriteRenderer.sprite = switchSprite[1];
                    //Debug.LogFormat("�� : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = true;
                    touchButton = StartCoroutine(ChangeTouchSwitch());
                    laserOnOffSource.clip = laserOnClip;
                    laserOnOffSource.Play();
                }
                Debug.LogFormat("IsSwitchOn �� : {0}", IsSwitchOn);
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
