using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using static PlayerMove;
using Unity.VisualScripting;
using TMPro;


public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
       Intro,Idle,Run,Jump,Attack
    }
    public GameObject slash;
    public Transform wallCheck;
    public float wallCheckDis;
    public LayerMask wall_mask;
    public PlayerState state;
    Player player;
    int playerId = 0;
    public float moveSpeed = 3f;
    private float jumpForce = 8f;
    private bool isRun;
    private bool isJump;
    private bool isStair;
    private bool isWallJump;
    private bool isWall;
    private float playerScale;
    private float direction;
    Rigidbody2D playerRigid;
    Animator playerAni;
    Ghost ghost;
    bool isAttacking;
    public float attackDuration = 0.2f; // 공격 지속 시간
    public float attackSpeed = 5f; // 공격 시 움직임 속도

    public float attackCooldown = 1f; // 공격 쿨다운
    private int attackCount = 0;
    private float lastAttackTime;
    Vector2 targetPosition;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        player = ReInput.players.GetPlayer(playerId);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        ghost = FindAnyObjectByType<Ghost>();
       
        if(state==PlayerState.Intro)
        {
       StartCoroutine(Intro());

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        playerScale = transform.localScale.x;
       isWall= Physics2D.Raycast(wallCheck.position, Vector2.right * playerScale, wallCheckDis, wall_mask);
       
        //if (IntroCanvas.isIntroOver == false)
        //{
        //    if (state == PlayerState.Intro)
        //    {
        //        ghost.isGhostMake = false;

        //    }
        //    return;
        //}
       
        if (player.GetButton("MoveLeft"))
        {
            isRun = true;
            ghost.isGhostMake = true;
            Vector3 movement = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            state = PlayerState.Run;

            playerRigid.velocity = movement;
           


        }
        else if (player.GetButton("MoveRight"))
        {
            ghost.isGhostMake = true;

            isRun = true;
            Vector3 movement = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            state = PlayerState.Run;

            playerRigid.velocity = movement;
            

        }
        else
        {
            ghost.isGhostMake = false;

            isRun = false;
            // ghost.isGhostMake = false;
           
            state = PlayerState.Idle;

            



        }
        playerAni.SetBool("Run", isRun);
        if (player.GetButtonDown("Jump"))
        {
           if(isJump == false)
            {
                
                isJump = true;
            }
           else
            {
                return;
            }
            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
       
        
        if(player.GetButtonDown("Attack")&&attackCount<4)
        {
            attackCount += 1;
            playerAni.Play("PlayerAttack");
             state = PlayerState.Attack;
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;
            
            Vector2 modifiedForce = new Vector2(attackDirection.x * 10f, attackDirection.y * (20f/attackCount));
            playerRigid.velocity = Vector2.zero;
            playerRigid.angularVelocity =0;
            slash.transform.position = this.transform.position;
            slash.SetActive(true);
            playerRigid.AddForce(modifiedForce, ForceMode2D.Impulse);
            if(attackDirection.x>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(attackDirection.x<0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        
        
        
        //if(isWallJump)
        //{
        //    Vector3 wallJump = new Vector3(-10f, jumpForce, 0f);
        //    playerRigid.AddForce(wallJump, ForceMode2D.Impulse);
        //    transform.localScale = new Vector3(-1, 1, 1);
        //}
        if(isJump==true)
        {
            state = PlayerState.Jump;
            ghost.isGhostMake = true;
        }
         
        if(state==PlayerState.Idle&&isStair==true)
        {
            playerRigid.velocity = new Vector2(0,0);
            playerRigid.gravityScale = 0;

        }
        else
        {
            playerRigid.gravityScale = 1.2f;

        }
        //Debug.LogFormat("isWall{0}", isWall);
    }//Update()

    private void FixedUpdate()
    {
        if(isWall)
        {
            isWallJump = false;
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, playerRigid.velocity.y * 0.9f);
            if(isWallJump==false)
            if(player.GetButton("Jump"))
            {
                isWallJump = true;
                Invoke("FreezeX", 0.3f);
                playerRigid.velocity = new Vector2(-playerScale * 5f, 0.9f * 5f);
                if(transform.localScale.x==1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (transform.localScale.x == -1)
                {
                    transform.localScale = new Vector3(1, 1, 1);

                }
            }
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Floor")||collision.collider.tag.Equals("Platform"))
        {
            attackCount = 0;
            isJump = false;
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            attackCount = 0;
            isJump = false;
            isStair = true;
        }
        if(collision.collider.tag.Equals("Wall"))
        {
            attackCount = 0;
            //isJump = false;
            if (player.GetButtonDown("Jump"))
            {
                isWallJump = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            attackCount = 0;
           
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            attackCount = 0;
           
        }
        if (collision.collider.tag.Equals("Wall"))
        {
            attackCount = 0;
            //isJump = false;
            if (player.GetButtonDown("Jump"))
            {
                isWallJump = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Stair"))
        {
            
            isStair = false;
        }
    }
    private IEnumerator Intro()
    {
        GameManager manager = FindAnyObjectByType<GameManager>();
        manager.IntroAction();
        playerAni.SetTrigger("PlaySong");
        yield return new WaitForSeconds(3);
        state = PlayerState.Idle;
    }
   

}
