using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_MoveRedLaser : MonoBehaviour
{
    //--------------------------------------- !������ �ʿ� ���� ��������� �̰� ���ߵ�! ----------------------------------
    Switch switchClass;

    private float moveSpeed = 3f;

    public bool leftCrash = false;
    public bool rightCrash = false;
    public bool laserIsSwitchBottonOn = true;

    private float daltatime = 0;

    private Vector3 leftMove = default;
    private Vector3 rightMove = default;



    // Start is called before the first frame update
    void Start()
    {
        MoveRedLaserInitialization();
        switchClass = FindObjectOfType<Switch>();
        switchClass.switchButtionboolChanged += IsChangedBool;
    }

    // Update is called once per frame
    void Update()
    {
        daltatime += moveSpeed * Time.deltaTime;
        LaserMovement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ ��� ���� �ε�������
        if (collision.gameObject.CompareTag("SG_Laser"))
        {
            // { ���ʰ������ε� �ε����Ÿ� ���������� ���� �ϰ� ���������� ���ٰ� �ε����Ÿ� �������� ������
            if (leftCrash == true)
            {
                leftCrash = false;
                rightCrash = true;
            }
            else if (rightCrash == true)
            {
                leftCrash = true;
                rightCrash = false;
            }
            else { /*PASS*/ }
            // } ���ʰ������ε� �ε����Ÿ� ���������� ���� �ϰ� ���������� ���ٰ� �ε����Ÿ� �������� ������
        }
        else { /*PASS*/ }
    }

    public void LaserMovement()
    {
        if (laserIsSwitchBottonOn == true) // ����ġ�� On�϶��� �����̰�
        {

            // �����ʿ� �ε����� �������� �̵�
            if (leftCrash == false && rightCrash == true)
            {
                //���ǿ� ������ ������
                if (daltatime >= 0.1)
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + leftMove;
                    daltatime = 0;
                }


            }

            // ���ʿ� �ε����� ���������� �̵�
            else if (leftCrash == true && rightCrash == false)
            {
                // ���ǿ� ������ ������
                if (daltatime >= 0.1)
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + rightMove;
                    daltatime = 0;
                }


            }
            else
            {
                Debug.Log("���� �߸��Ǿ���");
            }

        }
    }

    public void IsChangedBool(bool buttonSwitch)
    {
        laserIsSwitchBottonOn = buttonSwitch;

        //Debug.Log("buttonSwitch ��ȭ�ؼ� �ҷ��Գ�?");
    }

    // ù ���� ����
    public void MoveRedLaserInitialization()
    {

        int firstMove = Random.Range(0, 1);

        //  ù�������� ��������
        if (firstMove == 0)
        {
            leftCrash = true;
        }
        else if (firstMove == 1)
        {
            rightCrash = true;
        }
        else { /*PASS*/ }

        // �������� �̵��ҋ� ���� ��
        if (leftMove == default || leftMove == null)
        {
            leftMove = new Vector3(-0.05f, 0f, 0f);
        }
        else { /*PASS*/ }

        // ���������� �̵��Ҷ� ���� ��
        if (rightMove == default || rightMove == null)
        {
            rightMove = new Vector3(0.05f, 0f, 0f);
        }
        else { /*PASS*/ }


    }
}   //  NameSpace    
