using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SG_ShotLaserControler : MonoBehaviour
{

    public SG_PlayerMovement player;
    private SpriteRenderer spriteRenderer = null;

    // �÷� ��,��,��,�� �� �ٲٴ� �ڷ�ƾ ����Ϸ��� �Ʒ� �ּ� ����ϸ��
    private Coroutine changeColor = default;
    // changeColor = StartCoroutine(ColorChange());

    private WaitForFixedUpdate waitForFixedUpdate = default;

    private Color blue = default;
    private Color yellow = default;


    private Vector3 defaultScale = default;
    private Vector3 ShrinkageScale = default;

    // Start is called before the first frame update
    public void Awake()
    {
        // ��������Ʈ �������� ����ִٸ� GetComponent �Ⱥ�������� Pass
        if (spriteRenderer == default || spriteRenderer == null)
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        else { /*PASS*/ }

        // �Ķ��� RGB�� ������ ���������� �� RGB ����
        if (blue == default || blue == null)
        {
            blue = new Color(40, 130, 220);
        }
        else { /*PASS*/ }

        // ����� RGB�� �� ���� ���������� �� RGB ����
        if (yellow == default || yellow == null)
        {
            yellow = new Color(255, 180, 0);
        }
        else { /*PASS*/ }

        //  waitForFixedUpdate �� ����������� ĳ��
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }

        // ������ ��鼭 �پ�� �� ������ ����ϱ⿡ ĳ��
        if (ShrinkageScale == default || ShrinkageScale == null)
        {
            ShrinkageScale = new Vector3(-0.2f, 0f, 0f);
        }
        else { /*PASS*/ }

   

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        // SetActive == true �� �� ������ �۾�

        changeColor = StartCoroutine(ColorChange());
    }

    void OnDisable()
    {
        // SetActive == flase �� �� ������ �۾�
        // �ڷ�ƾ���� �پ�� ũ�� False�ɋ��� ����
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void LaserAttack()
    {

    }

    IEnumerator ColorChange()
    {

        //  {��,��,��,�� ����
        spriteRenderer.color = blue;
        for (int i = 0; i <= 3; i++)
        {
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 3; j++)
        {
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = blue;

        for (int j = 0; j <= 3; j++)
        {
            yield return waitForFixedUpdate;
        }
        //  }��,��,��,�� ����

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 3; j++)
        {
            //this.gameObject.transform.localScale
            yield return waitForFixedUpdate;
        }



    }
}
