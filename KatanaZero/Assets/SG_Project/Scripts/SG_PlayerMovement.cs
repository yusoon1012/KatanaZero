using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;


    private bool isJump = false;
    private bool playerIsAir = false;
    private bool readyRun = false;
    private bool leftClickAttackCoolTime = false;
    private Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

    // ================= �ڷ�ƾ ======================

    private Coroutine leftAttackCoroutine;    // �ڷ�ƾ ĳ��

    // �Ʒ� fixedUpdate�� 0.02�� �ֱ��̱⿡ ��� ���ؾ��ҰŰ���
    private WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdateĳ��
    private WaitForSeconds waitSecond = new WaitForSeconds(0.3f);
    

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
        JumpMove();
        AttackClick();


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //���⼭ �ٽ� ��������
        isJump = false;
        //�Ʒ������� �÷��̾ ���߿� �ֳ�? �� �ǹ�
        // �ִϸ��̼� ��Ʈ�ѷ� ���� ���� �ٽ� Ȯ���ҿ���
        //playerIsAir = false;
        playerAni.SetBool("IsJumpBool", isJump);

        //Debug.LogFormat("��򰡿� �΋H�ƴ�. Jump: {0}", isJump);
    }


    //=======================================Ŀ���� �Լ�=========================================
    public void LeftMove()
    {
        if (player.GetButton("MoveLeft"))
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            readyRun = true;
            Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            playerRigid.velocity = move;
            playerAni.SetBool("ReadyRun", readyRun);

        }

        if (player.GetButtonUp("MoveLeft"))
        {
            readyRun = false;
            playerAni.SetBool("ReadyRun", readyRun);
        }
    }

    public void RightMove()
    {
        if (player.GetButton("MoveRight"))
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            readyRun = true;
            Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
            playerRigid.velocity = move;
            playerAni.SetBool("ReadyRun", readyRun);

        }
        if (player.GetButtonUp("MoveRight"))
        {
            readyRun = false;
            playerAni.SetBool("ReadyRun", readyRun);
        }
    }

    public void JumpMove()
    {
        //Debug.LogFormat("���� ���� ����?? Ani jump: {0}, current jump: {1}", 
            //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false)
        {

        

            playerRigid.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);

            //���� �������� �� ���� ���ϰ� bool ó��
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);

        }
    }

    public void AttackClick()
    {

        
        //  ��Ŭ����
        if (Input.GetMouseButtonDown(0))
        {

            if (leftClickAttackCoolTime == false) // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����
            {
                
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());

                playerAni.SetTrigger("LeftClickAttack");
                Debug.LogFormat("���콺����Ŭ������");


                //  ���콺�� ��ǥ�� ī�޶��� WorldPoint�� ���� 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                //  Ŭ���� X��ǥ ����
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(9f, 0f), ForceMode2D.Impulse);
                    this.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // ��� ������ AddForce ����ȭ�Ǳ⶧���� �߰��� �ʱ�ȭ
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-9f, 0f), ForceMode2D.Impulse);
                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else { /*PASS*/ }

                // Ŭ���� Y��ǥ ����
                if (mousePosition.y > gameObject.transform.position.y)
                {
                  
                    playerRigid.AddForce(new Vector2(0f, 3f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {
                 
                    playerRigid.AddForce(new Vector2(0f, -3f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }

            }   // ��Ŭ�� ��Ÿ�� �ƴҶ��� �����

            else { Debug.LogFormat("true = ��Ÿ���� �������� ->{0}", leftClickAttackCoolTime); }

            //Debug.LogFormat("�̰��� ���� ���콺�� ��ǥ? -> {0}", mousePosition);
        }
        
    }   // AttackClick()


    // ==========================================�ڷ�ƾ============================================

    // { ���� ���� ��Ÿ�� �ڷ�ƾ
    private IEnumerator LeftClickAttack()
    {
        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3��

        leftClickAttackCoolTime = false;
    }
    // } ���� ���� ��Ÿ�� �ڷ�ƾ


}   // NameSpace
