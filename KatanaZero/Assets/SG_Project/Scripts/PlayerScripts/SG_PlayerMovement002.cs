using Rewired;
using Rewired.ComponentControls.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement002 : MonoBehaviour
{
    // ============== ���콺 ��ǥ�� ���� �׽�Ʈ ������=================
    //public GameObject testObj;
    //private GameObject cloneObj;

    // �Ʒ��� ����Ʈ������ ���� int�� ����
    public int one = 0;


    // ============== ���콺 ��ǥ�� ���� �׽�Ʈ ������=================

    public float moveSpeed = 10f;
    public float jumpForce = 7f;



    #region Bool ����





    private bool isJump = false;



    //���߿� �÷��̾� ����ó���� Bool����

    public bool isAttacking = false;
    public bool readyRun = false;
    public bool leftClickAttackCoolTime = false;

    // ������ ���ۿ� ����� ������
    public bool isRightMove = false;
    public bool isLeftMove = false;
    public bool isDownKeyInput = false;
    public bool isRollRock = false;
    public bool isRolling = false;


    // ���ٱ� ���ۿ� ����� ������
    // 23.08.18 �̰ɷ� ���� �پ����� �Ⱥپ����� üũ �Ұ���
    public bool isWallGrab = false;

    // ������ �ִ� ���� �ٰų� �������� ����� ����
    private bool exitWallGrab = false;


    // ���� ������ �밢�� �����Ҷ� �ʿ��� ����
    private bool wallJump = false;
    public bool isleftWall = false;
    public bool isRightWall = false;

    // ���� ������ ���ݽ� ������ ���⶧���� ���� ����
    private bool isAttackClingWallCoolTimeBool = false;


    // 23.08.20 Flip �ߴ��� ó���� ����
    private bool isFlipJump = false;

    // ������ ���� ���ϰ� ó���� ����
    //  true�����϶� ���ϰ� �Ұ���
    private bool headOffPrecrouch = false;


    // �÷��̾� ������ ������ �ִ� �������� üũ�ϴ� ����
    public bool isDodge = false;

    // �����÷��̾Floor ��  ���� ��� �ִ��� �˷��� Bool����
    private bool playerPresentFloor = false;

    #endregion

    #region int ����

    //�÷��̾ 3���̻� �������� �������� ���ϰ� �� ���� (collider���� ���� �ƴҰ�� �ʱ�ȭ �ǰ� �صξ���)
    public int attackCount = 0;

    // �ִϸ��̼� ������ ������� ���� ����
    public int wallGrabCount = 0;

    #endregion

    public Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator animator;
    BoxCollider2D playerBoxCollider;
    public GameObject leftAttackObj;

    Vector3 frontScale = Vector3.one;
    Vector3 backScale = new Vector3(-1f, 1f, 1f);

    // ================= �ڷ�ƾ ======================

    private Coroutine leftAttackCoroutine;    // �ڷ�ƾ ĳ��
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;
    private Coroutine isAttackClingWallCoolTime;


    // �Ʒ� fixedUpdate�� 0.02�� �ֱ��̱⿡ ��� ���ؾ��ҰŰ���
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdateĳ��
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= �ڷ�ƾ ======================


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        leftAttackObj.SetActive(false);
        FirstBoolFalse();


    }
    public void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        LeftMove();
        RightMove();
        JumpMove();
        IsDownKey();
        IsRoll();
        WallGrabAtInput();
        AttackClick();

    }

    //========================================�ݶ��̴�=======================================

    #region        �ݶ��̴�



    // --------------------------------------Collision Enter------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {

        // !�÷��̾ ������ �ƴҶ�
        if (isDodge == false)
        {

        }
        // !�÷��̾ ������ �ƴҶ�

        // �Ʒ� ������ ������ ���� �����ؾ� �� SG_ClingWall�̶�� ���� �ƴ϶�� ���� ���ǰɰ� �־����
        headOffPrecrouch = false;

        // ================================�±װ� ���϶� ========================================
        #region
        if (collision.gameObject.CompareTag("Enemy"))
        {

        }
        else
        {
            attackCount = 0;
            isJump = false;
        }
        #endregion






        // ================================= �±װ� ������ �ִ� ���϶� SG_ClingWall =================================
        #region       

        if (collision.gameObject.CompareTag("SG_ClingWall") && isAttackClingWallCoolTimeBool == false)
        {

            exitWallGrab = false;
            isWallGrab = true;
            wallGrabCount = 1;

            #region �ٴ� �������� �÷��̾��� Scale�� �����ϴ� �ڵ�
            // ����� bool �� ������ ���� �Ѱ��־����
            if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                isleftWall = true;
            }
            else if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                isRightWall = true;
            }
            else { /*PASS*/ }

            // �Ʒ��� �ٴ� �������� �÷��̾� ��,�� ���� if��
            if (isleftWall == true)
            {
                this.gameObject.transform.localScale = backScale;
            }
            else if (isRightWall == true)
            {
                this.gameObject.transform.localScale = frontScale;
            }
            else { /*PASS*/ }
            #endregion �ٴ� �������� �÷��̾��� Scale�� �����ϴ� �ڵ�

            animator.Play("WallGrab");
            // ?? �÷��̾ ���� �������� �ʰ��ϱ� ���� �ѰŰ��� ??
            if (playerRigid.gravityScale == 1 && playerRigid.mass > 0)
            {
                playerRigid.gravityScale = 0.3f;
                playerRigid.mass = 0.3f;
            }

            else { /*PASS*/ }


        }


        #endregion
        // ���� �� �ٴڿ� ��������� if

        // ================================= �±װ� �ٴ��϶� Floor ================================================
        #region

        //  !���� if
        if (collision.gameObject.CompareTag("Floor") && isJump == true)
        {
            //���⼭ �ٽ� ��������
            isJump = false;
            animator.SetTrigger("Landing");
            playerPresentFloor = true;
        }   //  !���� if

        // FlipJump �� �ѵڿ� ���� ������ �ִϸ����� Exit ���ֱ�����
        if (collision.gameObject.CompareTag("Floor") && isFlipJump == true)
        {
            animator.SetTrigger("FlipExit");
            isFlipJump = false;
        }

        // ������ �귯�����´��� ���� ��´ٸ�
        if (collision.gameObject.CompareTag("Floor") && isWallGrab == true)
        {
            animator.SetTrigger("ExitWallGrab");
            isWallGrab = false;

            // ���� ������ �귯������ ���� ������� �ݴ����� ������ �ٶ󺻹������� ���ϰ� �ϴ� ����
            if (this.transform.localScale == frontScale)
            {
                this.transform.localScale = backScale;
            }
            else if (this.transform.localScale == backScale)
            {
                this.transform.localScale = frontScale;
            }



        }
        #endregion




    }   //OnCollisionEnter


    //------------------------------------------- Collision Stay ---------------------------------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SG_ClingWall"))
        {
            // õõ�� �������� �ϱ� ���� �߷� 0���� ����
            this.gameObject.transform.position = this.gameObject.transform.position - new Vector3(0f, 0.001f, 0f);
        }

    }   // OnCollisionStay

    //------------------------------------------ Collision Exit --------------------------------------------
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (playerRigid.gravityScale == 0)
        {
            playerRigid.mass = 1;
            playerRigid.gravityScale = 1;
        }
        else { /*PASS*/ }

        if (collision.gameObject.CompareTag("SG_ClingWall"))
        {
            exitWallGrab = true;
        }
        else { /*PASS*/ }

        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("Exit���� False�� �ٲ�");
            playerPresentFloor = false;
        }

        //  ���߿� �̻��Ѱ����� false�� �ȴٸ� ���� mass,gravityScale 1�� ����� ���ǹ� �ȿ� ������ �ɰŰ���
        isleftWall = false;
        isRightWall = false;


    }

    #endregion �ݶ��̴�


    //=======================================Ŀ���� �Լ�=========================================

    private void FirstBoolFalse()
    {
        // ��,�� ������ ������ ���� ����Ǵ� ����
        isRightMove = false;
        isLeftMove = false;

        // �ִϸ����� Run�� ���� Bool ����
        readyRun = false;

        //���� �÷��̾ ������ �������� �ƴ��� �����ϱ� ���� ����
        // ������ Play�� �Ұ��̰� �����°��� ���� ��������� Trigger�� �ٰ���
        isJump = false;

        //���� �߿� ������ ���ϰ� �ϱ����� ����
        headOffPrecrouch = false;

        // S ��� Ű�� �������� Ȯ���� ����
        isDownKeyInput = false;

        // ������ ��Ÿ������ üũ�� ����
        isRollRock = false;
        // ������ �������� �˷��� ����
        isRolling = false;

        // ���� ���� �پ��ִ��� Ȯ���� ����
        isWallGrab = false;
        // �ٴº� collider Exit �ɋ��� true�� �� ����
        exitWallGrab = false;

        // 23.08.20 Flip �ߴ��� ó���� ����
        isFlipJump = false;


        isAttacking = false;
        leftClickAttackCoolTime = false;


        wallJump = false;
        isleftWall = false;
        isRightWall = false;

        isAttackClingWallCoolTimeBool = false;


        isDodge = false;

        playerPresentFloor = false;

    }

    #region �̵����� Ŀ�����Լ�
    public void LeftMove()
    {

        // �������� �ƴϰ� ������ ���� �ƴҶ���
        if (isAttacking == false && isRolling == false)
        {

            if (player.GetButton("MoveLeft"))
            {
                // { �÷��̾� �߷� ����ȭ ����
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.gravityScale = 1f;
                    playerRigid.mass = 1f;
                }
                else { /*PASS*/ }
                // } �÷��̾� �߷� ����ȭ ����

                isLeftMove = true;
                this.gameObject.transform.localScale = backScale;
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                animator.SetBool("ReadyRun", readyRun);
            }

            if (player.GetButtonUp("MoveLeft"))
            {
                isLeftMove = false;
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
            }

        }

    }

    public void RightMove()
    {

        if (isAttacking == false && isRolling == false)
        {

            if (player.GetButton("MoveRight"))
            {
                // { �÷��̾� �߷� ����ȭ ����
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.gravityScale = 1f;
                    playerRigid.mass = 1f;
                }
                else { /*PASS*/ }
                // } �÷��̾� �߷� ����ȭ ����

                isRightMove = true;
                this.gameObject.transform.localScale = frontScale;
                readyRun = true;
                Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;

                animator.SetBool("ReadyRun", readyRun);

            }
            if (player.GetButtonUp("MoveRight"))
            {
                isRightMove = false;
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
            }
        }

    }
    public void IsDownKey()
    {

        if (player.GetButtonDown("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = true;
            animator.SetBool("PrecrouchBool", isDownKeyInput);
            playerBoxCollider.offset = new Vector2(0.07f, -0.27f);
            playerBoxCollider.size = new Vector2(0.55f, 0.64f);

        }
        else { /*PASS*/ }

        if (player.GetButton("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = true;
        }

        if (player.GetButtonUp("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = false;
            animator.SetBool("PrecrouchBool", isDownKeyInput);

            playerBoxCollider.offset = new Vector2(0.07f, -0.03f);
            playerBoxCollider.size = new Vector2(0.55f, 1.1f);
        }
        else { /*PASS*/ }
    }

    public void IsRoll()
    {
        if (isJump == false && isRollRock == false)
        {
            if (isRightMove == true && isDownKeyInput == true
                && isLeftMove == false)
            {
                isDodge = true;

                rollRock = StartCoroutine(RollCoolTime());
                animator.Play("PlayerRoll");

                // �޸��� ���� ����
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);

            }       // if: ������ ����������?
            else { /*PASS*/ }

            if (isLeftMove == true && isDownKeyInput == true &&
                isRightMove == false)
            {
                isDodge = true;

                rollRock = StartCoroutine(RollCoolTime());
                animator.Play("PlayerRoll");

                // �޸��� ���� ����
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);

            }       // if: ���� ����������?
            else { /*PASS*/ }


        }

    }

    public void JumpMove()
    {

        if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == false)
        {
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }

            //���� �������� �� ���� ���ϰ� bool ó��
            isJump = true;
            animator.Play("Jump");
            Debug.Log("����");

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            headOffPrecrouch = true;

        }

        //  �ٴ� ���� �޶���� ���¿��� ������ �������
        else if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == true)
        {
            if (isleftWall == true)
            {
                //  �������� �߷� ����ȭ
                if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }

                // !Check 08.20 �̰� �ʿ����� Ȯ���ؾ���
                wallJump = true;

                isFlipJump = true;

                animator.SetTrigger("FlipStart");
                this.gameObject.transform.localScale = backScale;
                playerRigid.AddForce(new Vector2(8f, 9f), ForceMode2D.Impulse);

                this.gameObject.transform.localScale = frontScale;

                isJump = true;
                isWallGrab = false;

                // !Check 08.20 �̰� �ʿ����� Ȯ���ؾ���       
                headOffPrecrouch = true;
            }

            else if (isRightWall == true)
            {
                //  �������� �߷� ����ȭ
                if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }
                wallJump = true;

                isFlipJump = true;

                animator.SetTrigger("FlipStart");
                this.gameObject.transform.localScale = frontScale;
                playerRigid.AddForce(new Vector2(-8f, 9f), ForceMode2D.Impulse);


                this.gameObject.transform.localScale = backScale;


                isJump = true;
                isWallGrab = false;
                headOffPrecrouch = true;



            }
            //  TODO : �ݶ��̴����� ���� �پ� �ִ� ������ ��ǥ�� �˼� ������ �ڿ� Bool �� ���� LEFT,Right ���� ����� �Ѱ��ְ�
            //         �������϶� �ִ� �� �����ϋ� �ִ��� �ٸ��� �ؾ� ��
        }
    }

    // Wall Grab�����϶��� Ű�� �����ٸ�
    public void WallGrabAtInput()
    {

        //  { if : �����پ������� WŰ���ƴ� A,S,D Ű�� �ϳ��� ��������� 
        if (isWallGrab == true && isFlipJump == false &&
            (player.GetButtonDown("MoveLeft") || player.GetButtonDown("MoveRight") || player.GetButtonDown("DownKey")))
        {
            // ��Ÿ���� �־ ������ ������������ ���� ���ϰ� ����

            // 23.08.20 �ִϸ��̼� WallGrab���� Jump�� ������ �ؼ� �������� ���� ������ ��
            // Test �غ��Ҵµ� ���� �߸��ǵ��̸� �÷��̾ ������ �����°Ű���
            //isJump = true;
            //animator.SetTrigger("WallGrabToJump");


            exitWallGrab = true;
            StartCoroutine(IsAttackClingWallCoolTime());
            //exitWallGrab = false;

        }   //  } if : �����پ������� WŰ���ƴ� A,S,D Ű�� �ϳ��� ��������� 

    }



    #endregion

    public void AttackClick()
    {


        //  ��Ŭ����
        if (Input.GetMouseButtonDown(0))
        {


            if (leftClickAttackCoolTime == false && attackCount < 3) // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����                        
            {
                //Debug.Log("!����");
                isleftWall = true;

                if (isWallGrab == true)
                {
                    isAttackClingWallCoolTimeBool = true;
                    isAttackClingWallCoolTime = StartCoroutine(IsAttackClingWallCoolTime());
                }
                else { /*PASS*/ }

                // �÷��̾� �߷� ����ȭ ����
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }
                else { /*PASS*/ }

                if (isRightWall == true || isleftWall == true)
                {
                    isleftWall = false;
                    isRightWall = false;
                }
                else { /*PASS*/ }

                leftAttackObj.SetActive(true);
                one = 1;
                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

                //Debug.LogFormat("���콺����Ŭ������");


                //  ���콺�� ��ǥ�� ī�޶��� WorldPoint�� ���� 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                #region ��ǥ REGACY
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
                #endregion ��ǥ REGACY

                //  ==================== �Ʒ��� ���ǿ����� AddForce�� ���� �ִ� �ڵ� =========================
                //  Ŭ���� X��ǥ ����
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(12f, 0f), ForceMode2D.Impulse);
                    //���� �Ǿ��ش����� �ð������� �ʱ�ȭ���װ� ��,�� �� ������ �¿쵵 ���� �����ϼ� �ۿ� ���� ������ ����X
                    addforceReset = StartCoroutine(AddforceReset());
                    this.gameObject.transform.localScale = frontScale;
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-12f, 0f), ForceMode2D.Impulse);
                    //���� �Ǿ��ش����� �ð������� �ʱ�ȭ���װ� ��,�� �� ������ �¿쵵 ���� �����ϼ� �ۿ� ���� ������ ����X
                    addforceReset = StartCoroutine(AddforceReset());

                    this.gameObject.transform.localScale = backScale;
                }
                else { /*PASS*/ }

                // Ŭ���� Y��ǥ ����
                if (mousePosition.y > gameObject.transform.position.y)
                {


                    playerRigid.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {


                    playerRigid.AddForce(new Vector2(0f, -5f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }



                // Ư���� �������� ������ �޸��� ������ �޸��� ���� ���� �߰�
                // �޸��� ���� ����
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);

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
        for (int i = 0; i < 5; i++)
        {
            yield return fixedUpdate;
        }


        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3��
        leftAttackObj.SetActive(false);
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
        isRolling = true;
        // ���⼭ ���� ���� ������ ���� �����ָ� �ɰŰ�����?

        if (isRightMove == true && isJump == false) // ������ ������
        {
            playerRigid.AddForce(new Vector3(8f, 0f, 0f), ForceMode2D.Impulse);

            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }

            playerRigid.velocity = Vector3.zero;
            isRolling = false;
            animator.SetTrigger("RollEnd");


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isRightMove = false;
            isDodge = false;
        }   // ������ ������

        if (isLeftMove == true && isJump == false) // ���� ������
        {
            playerRigid.AddForce(new Vector3(-8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }

            playerRigid.velocity = Vector3.zero;
            isRolling = false;
            animator.SetTrigger("RollEnd");


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isLeftMove = false;
            isDodge = false;
        }   // ���� ������
        isRollRock = false;

    }
    // } ������ �ѵڿ� ��Ÿ���� �ٿ���

    private IEnumerator IsAttackClingWallCoolTime()
    {

        yield return waitSecond;
        isAttackClingWallCoolTimeBool = false;
        exitWallGrab = false;
    }



}   // NameSpace
