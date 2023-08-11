using Rewired;
using Rewired.ComponentControls.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
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

    // TEST BOOL
    private bool PresentWallGrab = false;
    // TEST BOOL

    // �÷��̾ �׾����� ��Ҵ��� ������ ���� �ٸ������� ���������� public ���� �����
    public bool isPlayerDie = false;

    private bool isJump = false;



    //���߿� �÷��̾� ����ó���� Bool����
    public bool playerIsAir = false;
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
    public bool isWallGrab = false;

    // ������ �ִ� ���� �ٰų� �������� ����� ����
    private bool exitWallGrab = false;


    // ���� ������ �밢�� �����Ҷ� �ʿ��� ����
    private bool wallJump = false;
    public bool isleftWall = false;
    public bool isRightWall = false;

    // ���� ������ ���ݽ� ������ ���⶧���� ���� ����
    private bool isAttackClingWallCoolTimeBool = false;


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
    Animator playerAni;
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
        playerAni = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();

        leftAttackObj.SetActive(false);


    }
    public void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {

        LeftMove();
        RightMove();
        IsDownKey();
        IsRoll();
        JumpMove();
        WallGrabAtInput();
        AttackClick();

        if(isPlayerDie == true)
        {
            Debug.Log("�÷��̾� Die == true");
        }


    }

    //========================================�ݶ��̴�=======================================

    #region        �ݶ��̴�



    // --------------------------------------Collision Enter------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {
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

        //Debug.LogFormat("Enter -> {0}", collision.gameObject.tag);



        ////���⼭ �ٽ� ��������
        //isJump = false;
        //playerAni.SetBool("IsJumpBool", isJump);

        //�Ʒ������� �÷��̾ ���߿� �ֳ�? �� �ǹ�
        // �ִϸ��̼� ��Ʈ�ѷ� ���� ���� �ٽ� Ȯ���ҿ���
        playerIsAir = false;



        // ================================= �±װ� ������ �ִ� ���϶� SG_ClingWall =================================
        #region
        // �Ʒ��� ������ �ִ� ���� �پ������� if (Collider)
        //if (collision.gameObject.CompareTag("SG_ClingWall"))
        //{
        //    Debug.LogFormat("���� 1: {0} / ���� 2: {1}", isAttackClingWallCoolTimeBool, playerPresentFloor);
        //}

        if (collision.gameObject.CompareTag("SG_ClingWall") && isAttackClingWallCoolTimeBool == false &&
            playerPresentFloor == false)
        {
            //Debug.Log("Enter���� �ٴº� �ν�");

            
            playerAni.SetTrigger("WallGrabTrigger");
            //Debug.LogFormat("Trigger�����Ŵ");

            // �����ؼ� ���� ������ �޸��� �ʴ´�.
            readyRun = false;
            //playerAni.SetBool("ReadyRun", readyRun);



            //// TEST
            //PresentWallGrab = true;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST


            exitWallGrab = false;
            isWallGrab = true;
            wallGrabCount = 1;

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

            //Debug.Log("CollisionEnter ��� �ֱ�� üũ?");
            if (playerRigid.gravityScale == 1 && playerRigid.mass > 0)
            {

                playerRigid.gravityScale = 0.3f;
                playerRigid.mass = 0.3f;

            }
            else { /*PASS*/ }

            //playerAni.SetTrigger("WallGrabTrigger");

            //Debug.LogFormat("���� �ٿ� �پ�����? PresentWallGrab: {0}, isWallGrab: {1}, Jump: {2}, isRun: {3}", 
            //    PresentWallGrab, isWallGrab, playerAni.GetBool("IsJump"), playerAni.GetBool("ReadyRun"));

            //if (wallGrabCount == 1)
            //{
            //playerAni.SetTrigger("WallGrabTrigger");

            //}

        }


        #endregion
        // ���� �� �ٴڿ� ��������� if

        // ================================= �±װ� �ٴ��϶� Floor ================================================
        #region

        if (collision.gameObject.CompareTag("Floor"))
        {
            //���⼭ �ٽ� ��������
            isJump = false;

            playerPresentFloor = true;

            //// TEST
            //PresentWallGrab = false;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST


            playerAni.SetBool("IsJumpBool", isJump);

            playerAni.SetTrigger("GrabwallToIdleTrigger");

        }


        if (collision.gameObject.CompareTag("Floor") && wallJump == true)
        {
            wallJump = false;
            playerAni.SetBool("WallJumpBool", wallJump);

        }

        if(collision.gameObject.CompareTag("Floor") && isWallGrab == true)
        {
            
        }
        #endregion
        // !�÷��̾ ������ �ƴҶ�
        if (isDodge == false)
        {


        }
        // !�÷��̾ ������ �ƴҶ�



    }   //OnCollisionEnter


    //------------------------------------------- Collision Stay ---------------------------------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SG_ClingWall"))
        {
            //Debug.Log("CollisionStay ��� �ֱ�� üũ?");

            // õõ�� �������� �ϱ� ���� �߷� 0���� ����

            this.gameObject.transform.position = this.gameObject.transform.position - new Vector3(0f, 0.001f, 0f);

        }

        //Debug.LogFormat("Stay -> {0}", collision.gameObject.tag);

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

            //// TEST
            //PresentWallGrab = false;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST

        }
        else { /*PASS*/ }

        if(collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("Exit���� False�� �ٲ�");
            playerPresentFloor = false;
        }

        //Debug.LogFormat("Exit -> {0}", collision.gameObject.tag);
        

        ////  �ٴڿ��� ������ ���� ���� ������ �ٽ� ���� �پ����� ������ ���� �ϴ� ����
        //if (collision.gameObject.CompareTag("SG_ClingWall"))
        //{
        //    isJump = false;
        //}


        //  ���߿� �̻��Ѱ����� false�� �ȴٸ� ���� mass,gravityScale 1�� ����� ���ǹ� �ȿ� ������ �ɰŰ���
        isleftWall = false;
        isRightWall = false;


    }

    #endregion �ݶ��̴�


    //=======================================Ŀ���� �Լ�=========================================


    #region �̵����� Ŀ�����Լ�
    public void LeftMove()
    {

        if (isAttacking == false && isRolling == false)
        {
            //if (player.GetButtonDown("MoveLeft") && isDownKeyInput == false)
            //{
            //    playerAni.SetTrigger("ReadyRunTrigger");
            //}

            if (player.GetButton("MoveLeft"))
            {
                isLeftMove = true;
                // Debug.LogFormat("L�̵� -> {0}", isLeftMove);
                //gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.gameObject.transform.localScale = backScale;
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

        if (isAttacking == false && isRolling == false)
        {

            //if (player.GetButtonDown("MoveRight") && isDownKeyInput == false)
            //{
            //    playerAni.SetTrigger("ReadyRunTrigger");
            //}

            if (player.GetButton("MoveRight"))
            {
                //Debug.LogFormat("R�̵� -> {0}", isRightMove);
                isRightMove = true;
                //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                this.gameObject.transform.localScale = frontScale;
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

        if (player.GetButtonDown("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = true;
            playerAni.SetBool("PrecrouchBool", isDownKeyInput);
            //Debug.LogFormat("SŰ -> {0}", isDownKeyInput);

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
            playerAni.SetBool("PrecrouchBool", isDownKeyInput);
            //Debug.LogFormat("SŰ -> {0}", isDownKeyInput);

            playerBoxCollider.offset = new Vector2(0.07f, -0.03f);
            playerBoxCollider.size = new Vector2(0.55f, 1.1f);
        }
        else { /*PASS*/ }
    }

    public void IsRoll()
    {
        if (isRollRock == false && playerIsAir == false && isJump == false)
        {
            if (isRightMove == true && isDownKeyInput == true
                && isLeftMove == false && isRollRock == false)
            {
                isDodge = true;
                //Debug.Log("������ ����");
                //

                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                // �޸��� ���� ����
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

                Debug.LogFormat("������ ������ �� / isRollRock: {0}, readyRun: {1}",
                    isRollRock, readyRun);

            }       // if: ������ ����������?
            else { /*PASS*/ }

            if (isLeftMove == true && isDownKeyInput == true &&
                isRightMove == false && isRollRock == false)
            {
                isDodge = true;

                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                // �޸��� ���� ����
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

                //Debug.LogFormat("������ ������ �� / isRollRock: {0}, readyRun: {1}",
                    //isRollRock, readyRun);
            }       // if: ���� ����������?
            else { /*PASS*/ }


        }

        //else { Debug.Log("������ ���"); } 
    }

    public void JumpMove()
    {
        //Debug.LogFormat("���� ���� ����?? Ani jump: {0}, current jump: {1}", 
        //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == false)
        {
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }

            playerIsAir = true;

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //���� �������� �� ���� ���ϰ� bool ó��
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);
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
                wallJump = true;
                playerAni.SetBool("WallJumpBool", wallJump);

                this.gameObject.transform.localScale = backScale;
                playerRigid.AddForce(new Vector2(8f, 9f), ForceMode2D.Impulse);

                playerAni.SetTrigger("PlayerFlipTrigger");
                this.gameObject.transform.localScale = frontScale;
                playerAni.SetTrigger("FlipAfterTrigger");

                isJump = true;
                isWallGrab = false;
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
                playerAni.SetBool("WallJumpBool", wallJump);

                this.gameObject.transform.localScale = frontScale;
                playerRigid.AddForce(new Vector2(-8f, 9f), ForceMode2D.Impulse);

                playerAni.SetTrigger("PlayerFlipTrigger");
                this.gameObject.transform.localScale = backScale;
                playerAni.SetTrigger("FlipAfterTrigger");

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
        //if (isWallGrab == true && !player.GetButtonDown("MoveJump") && exitWallGrab == true)
        //{
           
        //    isWallGrab = false;
        //    //exitWallGrab = false;
        //}
        //else { /*PASS*/ }


        //  { if : �����پ������� WŰ���ƴ� A,S,D Ű�� �ϳ��� ��������� 
        if(isWallGrab == true && 
            (player.GetButtonDown("MoveLeft") || player.GetButtonDown("MoveRight") || player.GetButtonDown("DownKey")))
        {
            // ��Ÿ���� �־ ������ ������������ ���� ���ϰ� ����
            exitWallGrab = true;
            playerAni.SetTrigger("WallGrabToFallTriger");
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

                playerAni.SetTrigger("LeftClickAttack");
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

                // Ư���� �������� ������ �޸��� ������ �޸��� ���� ���� �߰�
                // �޸��� ���� ����
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

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

        if (isRightMove == true && playerIsAir == false && isJump == false) // ������ ������
        {
            playerRigid.AddForce(new Vector3(8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isRightMove = false;
            isDodge = false;
        }   // ������ ������

        if (isLeftMove == true && playerIsAir == false && isJump == false) // ���� ������
        {
            playerRigid.AddForce(new Vector3(-8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isLeftMove = false;
            isDodge = false;
        }   // ���� ������
        isRollRock = false;


        //Debug.LogFormat("readyRun -> {0}", readyRun);
        //Debug.Log("�������� ����");
    }
    // } ������ �ѵڿ� ��Ÿ���� �ٿ���

    private IEnumerator IsAttackClingWallCoolTime()
    {

        yield return waitSecond;
        isAttackClingWallCoolTimeBool = false;
        exitWallGrab = false;
    }



}   // NameSpace
