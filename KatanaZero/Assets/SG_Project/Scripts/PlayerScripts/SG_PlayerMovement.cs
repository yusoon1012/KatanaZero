using Rewired;
using Rewired.ComponentControls.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{

    // �Ʒ��� ����Ʈ������ ���� int�� ����
    // one ������ AttackEmpect ��ũ��Ʈ���� ���� �ϱ� ������ public
    public int one = 0;


    // ============== ���콺 ��ǥ�� ���� �׽�Ʈ ������=================

    public float moveSpeed = 10f;
    public float jumpForce = 7f;



    #region Bool ����


    // private
    private bool isJump = false;



    //���߿� �÷��̾� ����ó���� Bool����

    public bool isAttacking = false;

    //private
    private bool readyRun = false;
    private bool leftClickAttackCoolTime = false;

    //private
    // ������ ���ۿ� ����� ������
    private bool isRightMove = false;
    private bool isLeftMove = false;
    private bool isDownKeyInput = false;
    private bool isRollRock = false;
    private bool isRolling = false;


    //private
    // ���ٱ� ���ۿ� ����� ������
    // 23.08.18 �̰ɷ� ���� �پ����� �Ⱥپ����� üũ �Ұ���
    private bool isWallGrab = false;

    //private
    // ������ �ִ� ���� �ٰų� �������� ����� ����
    private bool exitWallGrab = false;


    //private
    // ���� ������ �밢�� �����Ҷ� �ʿ��� ����
    private bool wallJump = false;
    private bool isleftWall = false;
    private bool isRightWall = false;

    //private
    // ���� ������ ���ݽ� ������ ���⶧���� ���� ����
    private bool isAttackClingWallCoolTimeBool = false;

    //private
    // 23.08.20 Flip �ߴ��� ó���� ����
    private bool isFlipJump = false;

    //private
    // 23.08.21 WallGrab������ ASD �������� Ȯ�� ���� ����
    private bool wallGrabTouch = false;

    //private
    // ������ ���� ���ϰ� ó���� ����
    //  true�����϶� ���ϰ� �Ұ���
    private bool headOffPrecrouch = false;


    // �÷��̾� ������ ������ �ִ� �������� üũ�ϴ� ����
    public bool isDodge = false;

    //private
    // �����÷��̾Floor ��  ���� ��� �ִ��� �˷��� Bool����
    private bool playerPresentFloor = false;

    public bool isDie = false;

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
    Vector3 diePosition;

    #region �ڷ�ƾ ĳ�� �κ�

    // ================= �ڷ�ƾ ======================

    private Coroutine leftAttackCoroutine;    // �ڷ�ƾ ĳ��
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;
    private Coroutine isAttackClingWallCoolTime;

    // 23.08.22�߰� �׾ AddForce�� ���� �ְ� �״�� ���� ��Ű�� �ϱ� ���ؼ��ڷ�ƾ �̿� ����
    private Coroutine playerDiePositionPick;

    // 23.08.21     10 : 55 �����丵 �ϸ鼭 �ڷ�ƾ �߰� ���� ������ �߰�����
    private Coroutine jumpCoroutine;
    private Coroutine exitWallGrabCoroutine;
    private Coroutine wallGrabToJumpCoroutine;
    private Coroutine flipExitCoroutine;


    // �Ʒ� fixedUpdate�� 0.02�� �ֱ��̱⿡ ��� ���ؾ��ҰŰ���
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdateĳ��
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= �ڷ�ƾ ======================

    #endregion �ڷ�ƾ ĳ��

    #region ����� ����
    AudioSource audioSource;

    // [0] = Jump001 [1] = Jump002    [2] = Roll
    // [3] = Run001  [4] = Run002     [5] = Run003
    // [6] = Die     [7] = Landing001 [8] = FlipJump001

    [SerializeField] private AudioClip[] audioClip;
    #endregion ����� ���� 

    public void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        // clip �� ����ִ»��¿��� ���ϰų� �������� ������ �߱⶧���� ó���� clip�� �������ְ� Play���� ����
        if (audioSource.clip == default || audioSource.clip == null)
        {
            audioSource.clip = audioClip[5];
        }

        leftAttackObj.SetActive(false);
        FirstBoolFalse();


    }
    public void FixedUpdate()
    {

    }
    // Update is called once per frame

    //�ֱ������� �θ��� ������Ʈ
    void Update()
    {
        // ���� �ʾ������� ���� ����
        if (isDie == false)
        {
            LeftMove();
            RightMove();
            JumpMove();
            IsDownKey();
            IsRoll();
            WallGrabAtInput();
            AttackClick();
        }
        //Debug.LogFormat("playerPresentFloor -> {0}", playerPresentFloor);

    }

    //========================================�ݶ��̴�=======================================

    #region        �ݶ��̴�



    // --------------------------------------Collision Enter------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDie == false)
        {
            // !�÷��̾ ������ �ƴҶ�
            if (isDodge == false)
            {
                // �������� �¾Ƽ� �׾�������
                if (collision.gameObject.CompareTag("SG_LaserShot"))
                {
                    isDie = true;

                    // �״� �ִϸ��̼� ���
                    animator.SetTrigger("DieTrigger");

                    // X�������� ����� ���ư�����
                    if (this.gameObject.transform.position.x > collision.gameObject.transform.position.x)
                    {
                        playerRigid.AddForce(new Vector2(3f, 3f));

                        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                        this.transform.localScale = backScale;

                    }
                    // X �������� ������ ���ư�����
                    else if (this.gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    {
                        playerRigid.AddForce(new Vector2(-3f, 3f));

                        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                        this.transform.localScale = frontScale;
                    }

                }
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
                //attackCount = 0;
                // 23.08.21 09:36 ���� ������ ���� �ӽ� �ּ�ó��
                //isJump = false;
            }
            #endregion






            // ================================= �±װ� ������ �ִ� ���϶� SG_ClingWall =================================
            #region       

            if (collision.gameObject.CompareTag("SG_ClingWall") && isAttackClingWallCoolTimeBool == false)
            {
                // 23.08.21     09 : 40  Jump ��ģ�� WallGrab���¿��� Flip ���� �����°Ͷ����� false�� �ִ°� �߰�
                isJump = false;

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

            //Debug.LogFormat("�����̵� �ݶ��̴� Enter �϶� {0}", isJump);
            //  !���� if
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && isJump == true)
            {
                //���⼭ �ٽ� ��������            
                isJump = false;

                //!����� �����Ҹ�
                // ���� ������ Y���� �÷��̾��� ������ Y �� �� Ŭ���� �Ҹ� �÷���
                if (collision.gameObject.transform.position.y < this.transform.position.y)
                {
                    audioSource.clip = audioClip[7];
                    audioSource.Play();
                }
                else { /*PASS*/ }

                animator.SetTrigger("Landing");
                playerPresentFloor = true;
                jumpCoroutine = StartCoroutine(LandingTriggerReset());

            }   //  !���� if
            else { /*PASS*/ }

            // WallGrab ���¿��� ASD Ű �������� ������ �ϱ� ���� if
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isJump == false && wallGrabTouch == true)
            {
                animator.SetTrigger("Landing");
                wallGrabTouch = false;
                // �������� Ʈ���� �ʱ�ȭ
                jumpCoroutine = StartCoroutine(LandingTriggerReset());

                // ���ٱ� Ʈ���� �ʱ�ȭ
                exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());

                // ���ٱ� -> ���� �� ���� Ʈ���� �ʱ�ȭ
                wallGrabToJumpCoroutine = StartCoroutine(WallGrabToJumpTriggerReset());

                //animator.ResetTrigger("ExitWallGrab");
            }
            else { /*PASS*/ }

            // FlipJump �� �ѵڿ� ���� ������ �ִϸ����� Exit ���ֱ�����
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isFlipJump == true)
            {
                //!����� �����Ҹ�
                audioSource.clip = audioClip[7];
                audioSource.Play();

                animator.SetTrigger("FlipExit");
                isFlipJump = false;
            }
            else { /*PASS*/ }

            // ������ �귯�����´��� ���� ��´ٸ�
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isWallGrab == true)
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
            else { /*PASS*/ }

            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && attackCount > 0)
            {
                attackCount = 0;
                animator.SetTrigger("Landing");

                // �������� ���� �پ����� ������ �ȵŴ� Trigger�� �ʱ�ȭ
                // ���ٱ� Ʈ���� �ʱ�ȭ
                exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());
                // �������� Ʈ���� �ʱ�ȭ
                jumpCoroutine = StartCoroutine(LandingTriggerReset());
                // FlipExit Ʈ���� �ʱ�ȭ
                flipExitCoroutine = StartCoroutine(FlipExitTriggerReset());

            }

            #endregion



        }
    }   //OnCollisionEnter


    //------------------------------------------- Collision Stay ---------------------------------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (isDie == false)
        {
            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                // õõ�� �������� �ϱ� ���� �߷� 0���� ����
                this.gameObject.transform.position = this.gameObject.transform.position - new Vector3(0f, 0.001f, 0f);
            }
            else { /*PASS*/ }

            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                // Floor ��� �ٴڿ� ��� �ִµ����� true
                playerPresentFloor = true;
            }


            //  ���� ����ִµ� ���ݻ����̰� Jump �ִϸ��̼��� �������̶�� ������ Idle �ִϸ��̼� ���� ����
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && isAttacking == true &&
               animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.Jump"))
            {
                //! �÷��̾ ���� ������ �׳� Idle�ִϸ��̼����� ���������� �÷����̶��
                //  ������ Y�� ���ؼ� �÷��̾���  Y���� �� �������� Idle�� �������ؼ� �Ʒ��� �΋Hĥ���� Idle�� ���°� ����
                if (collision.gameObject.CompareTag("Floor"))
                {
                    animator.Play("IdleAnimation");
                }

                else if (collision.gameObject.CompareTag("Platform"))
                {
                    if (collision.gameObject.transform.position.y < this.gameObject.transform.position.y)
                    {
                        animator.Play("IdleAnimation");
                    }
                }
                else { /*PASS*/ }

            }
            else { /*PASS*/ }
        }
    }   // OnCollisionStay

    //------------------------------------------ Collision Exit --------------------------------------------
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (isDie == false)
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

            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                //Debug.Log("Exit���� False�� �ٲ�");
                playerPresentFloor = false;
            }

            //  ���߿� �̻��Ѱ����� false�� �ȴٸ� ���� mass,gravityScale 1�� ����� ���ǹ� �ȿ� ������ �ɰŰ���
            isleftWall = false;
            isRightWall = false;


        }
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

        // ���� �������� üũ�� ����
        isDodge = false;

        // ���� �÷��̾ ���� ��� �ִ��� üũ�� ����
        // CollisionEnter = True CollisionExit = false
        playerPresentFloor = false;

        // ������ �ߴ��� �˷��� ����
        isAttacking = false;
        // ���� ������ ��Ÿ������ üũ�� ���� ��Ÿ���� true�Ͻ� Ŭ���ص� ������ ������ ����
        leftClickAttackCoolTime = false;

        // �������� �ߴ��� �˷��� ����
        wallJump = false;

        // ���� ���� ���� ���ʿ� �ִ� ������ �����ʿ� �ִº����� �˷��� ����
        isleftWall = false;
        isRightWall = false;

        // ���� �پ ������ �ٽ� ���� ���� �ʰ� ���� ����
        isAttackClingWallCoolTimeBool = false;


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

                // �̵� ����� Ŀ���� �Լ�
                MoveAudio();

                this.gameObject.transform.localScale = backScale;
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                animator.SetBool("ReadyRun", readyRun);
            }

            if (player.GetButtonUp("MoveLeft") && isRightMove == true)
            {
                isLeftMove = false;
            }
            else if (player.GetButtonUp("MoveLeft") && isRightMove == false)
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

                // �̵� ����� Ŀ���� �Լ�
                MoveAudio();

                this.gameObject.transform.localScale = frontScale;
                readyRun = true;
                Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;

                animator.SetBool("ReadyRun", readyRun);

            }
            if (player.GetButtonUp("MoveRight") && isLeftMove == true)
            {
                isRightMove = false;
            }

            else if (player.GetButtonUp("MoveRight") && isLeftMove == false)
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
        if (isJump == false && isRollRock == false && isWallGrab == false)
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
            animator.Play("Jump");
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }
            //���� �������� �� ���� ���ϰ� bool ó��
            isJump = true;

            // !����� (����)
            audioSource.clip = audioClip[2];
            audioSource.Play();

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            headOffPrecrouch = true;

        }
        else { /*PASS*/ }
        //  �ٴ� ���� �޶���� ���¿��� ������ �������
        if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == true)
        {
            FlipJump();
        }
        else { /*PASS*/ }
    }

    // �밢�� ����
    public void FlipJump()
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

            //!����� (������)
            audioSource.clip = audioClip[8];
            audioSource.Play();

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

            //!����� (������)
            audioSource.clip = audioClip[8];
            audioSource.Play();

            animator.SetTrigger("FlipStart");
            this.gameObject.transform.localScale = frontScale;
            playerRigid.AddForce(new Vector2(-8f, 9f), ForceMode2D.Impulse);


            this.gameObject.transform.localScale = backScale;


            isJump = true;
            isWallGrab = false;
            headOffPrecrouch = true;

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

            // 23.08.21     11:04 �ٽ� �õ�

            //�Ʒ� ����� �� ���°� Ȯ�ε� W ���������� �ȵ���
            //Debug.Log("���� ��� �������� ASD Ű�� ������ ������?");
            animator.SetTrigger("WallGrabToJump");
            wallGrabTouch = true;

            // ���⿡ �������� �� Landing Trigger �� ���� �۵� ���� ����

            // ���⿡ �ڷ�ƾ ���� ReSetTrigger �־��� ����
            //jumpCoroutine = null;
            // jumpCoroutine = StartCoroutine()

            // !TODO : ĳ���Ѱ� ����ϰ� �����߰���
            StartCoroutine(IsAttackClingWallCoolTime());
            //exitWallGrab = false;

        }   //  } if : �����پ������� WŰ���ƴ� A,S,D Ű�� �ϳ��� ��������� 

    }



    #endregion

    #region ����� ���� Ŀ���� �Լ�
    private void MoveAudio()
    {

        // if : (1)�÷��̾ A or D Ű�� ���� ���¿����ϰ� (2)audio�� �÷��� ������ �ʾƾ��ϰ� 
        //      (3)�÷��̾ ���� ��� �־�� ��
        if (!audioSource.isPlaying && playerPresentFloor == true &&
            (isRightMove == true || isLeftMove == true))
        {
            //Debug.LogFormat("audioSource.clip �� ���� �ϴ���?? {0}", audioSource.clip != null);
            //Debug.LogFormat("audioSource.clip�� null�̶� �̸��� ��ã��. -> {0}", audioSource.clip.name);


            // if : ������� ���������� ��� �ǵ��� �ϰ����            
            if (audioSource.clip.name == ("Run002"))
            {
                // audioClip[5] = Run003
                audioSource.clip = audioClip[5];
                audioSource.Play();
            }
            else if (audioSource.clip.name != ("Run002"))
            {
                audioSource.clip = audioClip[4];
                audioSource.Play();
            }
            else { /*PASS*/ }
            //Debug.LogFormat("Clip Name -> {0}", audioSource.clip.name);
        }
        else { /*PASS*/ }
    }

    #endregion ����� ���� Ŀ���� �Լ�

    public void AttackClick()
    {
        //  ��Ŭ����
        if (Input.GetMouseButtonDown(0))
        {

            if (leftClickAttackCoolTime == false && attackCount < 3) // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����                        
            {
                //// ���ٱ� Ʈ���� �ʱ�ȭ
                //exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());

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

                // 23.08.21 ���뿡�� ���� �ִϸ��̼� ��� �ϸ� �ɰŰ���
                animator.Play("AttackAnimaition001");

                leftAttackObj.SetActive(true);
                one = 1;
                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

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
            // !����� (������)
            audioSource.clip = audioClip[2];
            audioSource.Play();

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
            // !����� (������)
            audioSource.clip = audioClip[2];
            audioSource.Play();

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

    // �׾������� �������� ���߿� ���� ��Ű������ �ڷ�ƾ
    private IEnumerator PlayerDiePositionPick()
    {
        // ���⿡ ��ǥ�� �����ϰ� ��� �������� ���� ���������
        for (int i = 0; i <= 25; i++)
        {
            yield return fixedUpdate;
        }
        diePosition = this.transform.position;

        // !23.08.22 �ӽ÷� �ص� ���� ���ľ��Ҽ��� ����

        playerRigid.gravityScale = 0f;
        playerRigid.mass = 0f;

        
        for (int j = 0; j <= 100; j++)
        {
            this.transform.position = diePosition;
            yield return waitSecond;
        }
        // !23.08.22 �ӽ÷� �ص� ���� ���ľ��Ҽ��� ����

    }
    // �Ʒ��� Ʈ���� �ʱ�ȭ ���� �ڷ�ƾ

    private IEnumerator LandingTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("Landing");
    }

    private IEnumerator ExitWallGrabTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("ExitWallGrab");
    }

    private IEnumerator WallGrabToJumpTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("WallGrabToJump");
    }

    private IEnumerator FlipExitTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("FlipExit");
    }

}   // NameSpace
