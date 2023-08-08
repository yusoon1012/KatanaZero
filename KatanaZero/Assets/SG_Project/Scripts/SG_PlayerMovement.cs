using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{
    // ============== ���콺 ��ǥ�� ���� �׽�Ʈ ������=================
    //public GameObject testObj;
    //private GameObject cloneObj;
    // ============== ���콺 ��ǥ�� ���� �׽�Ʈ ������=================

    public float moveSpeed = 7f;
    public float jumpForce = 7f;

   
    private bool isJump = false;


    //���߿� �÷��̾� ����ó���� Bool����
    public bool playerIsAir = false;
    public bool isAttacking = false;
    public bool readyRun = false;
    public bool leftClickAttackCoolTime = false;

    // ������ ���ۿ� ����� bool ������
    public bool isRightMove = false;
    public bool isLeftMove = false;
    public bool isDownKeyInput = false;
    public bool isRollRock = false;

    //�÷��̾ 3���̻� �������� �������� ���ϰ� �� ���� (collider���� ���� �ƴҰ�� �ʱ�ȭ �ǰ� �صξ���)
    public int attackCount = 0;


    public Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

    // ================= �ڷ�ƾ ======================

    private Coroutine leftAttackCoroutine;    // �ڷ�ƾ ĳ��
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;

    // �Ʒ� fixedUpdate�� 0.02�� �ֱ��̱⿡ ��� ���ؾ��ҰŰ���
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdateĳ��
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= �ڷ�ƾ ======================


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        LeftMove();
        RightMove();
        IsDownKey();
        IsRoll();
        JumpMove();
        AttackClick();
      


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {

        }
        else
        {
            attackCount = 0;
            isJump = false;
        }

        //���⼭ �ٽ� ��������
        isJump = false;

        //�Ʒ������� �÷��̾ ���߿� �ֳ�? �� �ǹ�
        // �ִϸ��̼� ��Ʈ�ѷ� ���� ���� �ٽ� Ȯ���ҿ���
        playerIsAir = false;

        playerAni.SetBool("IsJumpBool", isJump);

        //Debug.LogFormat("��򰡿� �΋H�ƴ�. Jump: {0}", isJump);
    }


    //=======================================Ŀ���� �Լ�=========================================


    #region �̵����� Ŀ�����Լ�
    public void LeftMove()
    {

        if (isAttacking == false)
        {

            if (player.GetButton("MoveLeft"))
            {
                isLeftMove = true;
               // Debug.LogFormat("L�̵� -> {0}", isLeftMove);
                gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                playerAni.SetBool("ReadyRun", readyRun);

            }

            if (player.GetButtonUp("MoveLeft"))
            {
                isLeftMove = false;
                //Debug.LogFormat("L�̵� -> {0}", isLeftMove);
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);
            }

        }

    }

    public void RightMove()
    {

        if (isAttacking == false)
        {



            if (player.GetButton("MoveRight"))
            {
                //Debug.LogFormat("R�̵� -> {0}", isRightMove);
                isRightMove = true;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                readyRun = true;
                Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                playerAni.SetBool("ReadyRun", readyRun);

            }
            if (player.GetButtonUp("MoveRight"))
            {
                isRightMove = false;
                //Debug.LogFormat("R�̵� -> {0}", isRightMove);
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);
            }
        }


    }
    public void IsDownKey()
    {
        if (player.GetButton("DownKey"))
        {
            isDownKeyInput = true;
            //Debug.LogFormat("SŰ -> {0}", isDownKeyInput);
        }
        else { /*PASS*/ }

        if (player.GetButtonUp("DownKey"))
        {
            isDownKeyInput = false;
            //Debug.LogFormat("SŰ -> {0}", isDownKeyInput);
        }
        else { /*PASS*/ }
    }

    public void IsRoll()
    {
        if (isRollRock == false)
        {
            if (isRightMove == true && isDownKeyInput == true
                && isLeftMove == false && isRollRock == false)
            {
                Debug.Log("������ ����");
                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                Debug.Log("������ ������");

            }
            else { /*PASS*/ }

            if (isLeftMove == true && isDownKeyInput == true &&
                isRightMove == false && isRollRock == false)
            {
                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
            }
            else { /*PASS*/ }
        }
        else { Debug.Log("������ ���"); }
    }

    public void JumpMove()
    {
        //Debug.LogFormat("���� ���� ����?? Ani jump: {0}, current jump: {1}", 
        //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false)
        {

            playerIsAir = true;

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //���� �������� �� ���� ���ϰ� bool ó��
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);

        }
    }



    #endregion

    public void AttackClick()
    {


        //  ��Ŭ����
        if (Input.GetMouseButtonDown(0))
        {


            if (leftClickAttackCoolTime == false && attackCount < 3) // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����                        
            {

                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

                playerAni.SetTrigger("LeftClickAttack");
                //Debug.LogFormat("���콺����Ŭ������");


                //  ���콺�� ��ǥ�� ī�޶��� WorldPoint�� ���� 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                //�Ʒ� Instantiate �� ���콺��°��� �������� Ȯ���ϱ� ���� �ڽ� ���۱�
                //cloneObj = Instantiate(testObj, mousePosition, testObj.transform.rotation);



                //  �Ʒ� ���� ���� �¿�� �������� �ʱ� ������ ��� �ּ�
                // �÷��̾� y������ + 0.2 �ϸ� ��� ��
                //if (this.gameObject.transform.position.y + 0.2 < mousePosition.y && 
                //    this.gameObject.transform.position.x + 0.2 > mousePosition.x ||
                //    this.gameObject.transform.position.x + -0.01 > mousePosition.x)
                //{
                //    // �� ���� ���� �޼��� �¿� �̵����� ���� �̵�
                //    //  PASS
                //}


                //  ==================== �Ʒ��� ���ǿ����� AddForce�� ���� �ִ� �ڵ� =========================
                //  Ŭ���� X��ǥ ����
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;



                    playerRigid.AddForce(new Vector2(12f, 0f), ForceMode2D.Impulse);
                    //���� �Ǿ��ش����� �ð������� �ʱ�ȭ���װ� ��,�� �� ������ �¿쵵 ���� �����ϼ� �ۿ� ���� ������ ����X
                    addforceReset = StartCoroutine(AddforceReset());
                    this.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-12f, 0f), ForceMode2D.Impulse);
                    //���� �Ǿ��ش����� �ð������� �ʱ�ȭ���װ� ��,�� �� ������ �¿쵵 ���� �����ϼ� �ۿ� ���� ������ ����X
                    addforceReset = StartCoroutine(AddforceReset());

                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else { /*PASS*/ }

                // Ŭ���� Y��ǥ ����
                if (mousePosition.y > gameObject.transform.position.y)
                {
                    
                    playerAni.SetBool("IsJumpBool", true);
                    playerIsAir = true;
                    playerRigid.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {
                   
                    playerAni.SetBool("IsJumpBool", true);
                    playerIsAir = true;
                    playerRigid.AddForce(new Vector2(0f, -5f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }

                //Debug.LogFormat("Now AirBool -> {0}", playerIsAir);
                playerAni.SetBool("PlayerIsAir", playerIsAir);



            }   // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����

            else { /*PASS*/ }
            //{ /*Debug.LogFormat("true = ��Ÿ���� �������� ->{0}", leftClickAttackCoolTime);*/ }

            //Debug.LogFormat("�̰��� ���� ���콺�� ��ǥ? -> {0}", mousePosition);
        }

    }   // AttackClick()


    

    // ==========================================�ڷ�ƾ============================================

    // { ���� ���� ��Ÿ�� �ڷ�ƾ
    private IEnumerator LeftClickAttack()
    {
        for(int i =0; i < 5; i++)
        {
            yield return fixedUpdate;
        }
        
        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3��

        leftClickAttackCoolTime = false;

    }
    // } ���� ���� ��Ÿ�� �ڷ�ƾ


    // { ���ݽ� �����ð� ������ ���
    private IEnumerator MoveRock()
    {
        for (int i = 0; i <= 25; i++)
        {
            //Debug.LogFormat("���� fixedUpdate �� Ƚ�� {0}", i);
            // 1�ʵ��� ������ ����
            yield return fixedUpdate;
        }

        isAttacking = false;
    }
    // } ���ݽ� �����ð� ������ ���

    // { ������ AddForce�� ������ �����ð� ���� �ʱ�ȭ
    private IEnumerator AddforceReset()
    {
        for (int i = 0; i <= 15; i++)
        {
            yield return fixedUpdate;
        }

        //  �÷��̾�� ���� ����������
        if (mousePosition.x > gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one;
        }
        else if (mousePosition.x < gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one * -1;
        }
    }
    // } ������ AddForce�� ������ �����ð� ���� �ʱ�ȭ


    // { ������ �ѵڿ� ��Ÿ���� �ٿ���
    private IEnumerator RollCoolTime()
    {
        isRollRock = true;
        for(int i = 0; i <= 40; i++)
        {
            yield return fixedUpdate;
        }
        isRollRock = false;
        Debug.Log("�������� ����");
    }
    // } ������ �ѵڿ� ��Ÿ���� �ٿ���


}   // NameSpace
