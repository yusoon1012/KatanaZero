using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{
    // ============== 마우스 좌표를 위한 테스트 변수들=================
    //public GameObject testObj;
    //private GameObject cloneObj;
    // ============== 마우스 좌표를 위한 테스트 변수들=================

    public float moveSpeed = 7f;
    public float jumpForce = 7f;

   
    private bool isJump = false;


    //나중에 플레이어 공중처리할 Bool변수
    public bool playerIsAir = false;
    public bool isAttacking = false;
    public bool readyRun = false;
    public bool leftClickAttackCoolTime = false;

    // 구르기 제작에 사용할 bool 변수들
    public bool isRightMove = false;
    public bool isLeftMove = false;
    public bool isDownKeyInput = false;
    public bool isRollRock = false;

    //플레이어가 3번이상 연속으로 공격하지 못하게 할 변수 (collider에서 적이 아닐경우 초기화 되게 해두었음)
    public int attackCount = 0;


    public Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

    // ================= 코루틴 ======================

    private Coroutine leftAttackCoroutine;    // 코루틴 캐싱
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;

    // 아래 fixedUpdate는 0.02초 주기이기에 계산 잘해야할거같음
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdate캐싱
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= 코루틴 ======================


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

        //여기서 다시 점프가능
        isJump = false;

        //아래변수는 플레이어가 공중에 있나? 를 의미
        // 애니메이션 컨트롤러 수정 이후 다시 확인할예정
        playerIsAir = false;

        playerAni.SetBool("IsJumpBool", isJump);

        //Debug.LogFormat("어딘가에 부딫쳤다. Jump: {0}", isJump);
    }


    //=======================================커스텀 함수=========================================


    #region 이동관련 커스텀함수
    public void LeftMove()
    {

        if (isAttacking == false)
        {

            if (player.GetButton("MoveLeft"))
            {
                isLeftMove = true;
               // Debug.LogFormat("L이동 -> {0}", isLeftMove);
                gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                playerAni.SetBool("ReadyRun", readyRun);

            }

            if (player.GetButtonUp("MoveLeft"))
            {
                isLeftMove = false;
                //Debug.LogFormat("L이동 -> {0}", isLeftMove);
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
                //Debug.LogFormat("R이동 -> {0}", isRightMove);
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
                //Debug.LogFormat("R이동 -> {0}", isRightMove);
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
            //Debug.LogFormat("S키 -> {0}", isDownKeyInput);
        }
        else { /*PASS*/ }

        if (player.GetButtonUp("DownKey"))
        {
            isDownKeyInput = false;
            //Debug.LogFormat("S키 -> {0}", isDownKeyInput);
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
                Debug.Log("구르기 들어옴");
                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                Debug.Log("구르기 마지막");

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
        else { Debug.Log("구르기 잠김"); }
    }

    public void JumpMove()
    {
        //Debug.LogFormat("현재 점프 상태?? Ani jump: {0}, current jump: {1}", 
        //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false)
        {

            playerIsAir = true;

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //점프 했을때에 또 점프 못하게 bool 처리
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);

        }
    }



    #endregion

    public void AttackClick()
    {


        //  좌클릭시
        if (Input.GetMouseButtonDown(0))
        {


            if (leftClickAttackCoolTime == false && attackCount < 3) // 좌클릭 쿨타임 아닐때에 실행됨                        
            {

                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

                playerAni.SetTrigger("LeftClickAttack");
                //Debug.LogFormat("마우스왼쪽클릭했음");


                //  마우스의 좌표를 카메라의 WorldPoint로 구함 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                //아래 Instantiate 는 마우스찍는곳이 정상인지 확인하기 위한 박스 제작기
                //cloneObj = Instantiate(testObj, mousePosition, testObj.transform.rotation);



                //  아래 조건 사용시 좌우로 움직이질 않기 때문에 잠시 주석
                // 플레이어 y포지션 + 0.2 하면 어꺠 쪽
                //if (this.gameObject.transform.position.y + 0.2 < mousePosition.y && 
                //    this.gameObject.transform.position.x + 0.2 > mousePosition.x ||
                //    this.gameObject.transform.position.x + -0.01 > mousePosition.x)
                //{
                //    // 윗 공격 조건 달성시 좌우 이동없이 위로 이동
                //    //  PASS
                //}


                //  ==================== 아래는 조건에따라 AddForce로 힘을 주는 코드 =========================
                //  클릭시 X좌표 조건
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;



                    playerRigid.AddForce(new Vector2(12f, 0f), ForceMode2D.Impulse);
                    //힘을 실어준다음에 시간지나면 초기화시켰고 상,하 는 어차피 좌우도 같이 움직일수 밖에 없기 때문에 삽입X
                    addforceReset = StartCoroutine(AddforceReset());
                    this.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-12f, 0f), ForceMode2D.Impulse);
                    //힘을 실어준다음에 시간지나면 초기화시켰고 상,하 는 어차피 좌우도 같이 움직일수 밖에 없기 때문에 삽입X
                    addforceReset = StartCoroutine(AddforceReset());

                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else { /*PASS*/ }

                // 클릭시 Y좌표 조건
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



            }   // 좌클릭 쿨타임 아닐때에 실행됨

            else { /*PASS*/ }
            //{ /*Debug.LogFormat("true = 쿨타임이 돌고있음 ->{0}", leftClickAttackCoolTime);*/ }

            //Debug.LogFormat("이것이 현재 마우스의 좌표? -> {0}", mousePosition);
        }

    }   // AttackClick()


    

    // ==========================================코루틴============================================

    // { 왼쪽 공격 쿨타임 코루틴
    private IEnumerator LeftClickAttack()
    {
        for(int i =0; i < 5; i++)
        {
            yield return fixedUpdate;
        }
        
        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3초

        leftClickAttackCoolTime = false;

    }
    // } 왼쪽 공격 쿨타임 코루틴


    // { 공격시 일정시간 움직임 잠금
    private IEnumerator MoveRock()
    {
        for (int i = 0; i <= 25; i++)
        {
            //Debug.LogFormat("현재 fixedUpdate 돈 횟수 {0}", i);
            // 1초동안 움직임 멈춤
            yield return fixedUpdate;
        }

        isAttacking = false;
    }
    // } 공격시 일정시간 움직임 잠금

    // { 공격후 AddForce의 힘값을 일정시간 이후 초기화
    private IEnumerator AddforceReset()
    {
        for (int i = 0; i <= 15; i++)
        {
            yield return fixedUpdate;
        }

        //  플레이어보다 앞을 눌렀을때에
        if (mousePosition.x > gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one;
        }
        else if (mousePosition.x < gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one * -1;
        }
    }
    // } 공격후 AddForce의 힘값을 일정시간 이후 초기화


    // { 구르기 한뒤에 쿨타임을 줄예정
    private IEnumerator RollCoolTime()
    {
        isRollRock = true;
        for(int i = 0; i <= 40; i++)
        {
            yield return fixedUpdate;
        }
        isRollRock = false;
        Debug.Log("구르기쿨 끝남");
    }
    // } 구르기 한뒤에 쿨타임을 줄예정


}   // NameSpace
