using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRay : MonoBehaviour
{
    public  enum EnemyState
    {
        Idle,Patrol,Chase
    }
  //  public Transform rayCast;
    //public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector]public Transform target;
    [HideInInspector]public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public EnemyState state;
    //private RaycastHit2D hit;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooldown;
    private float intTimer;
    private bool onPlatform;
    private bool onStair;
    private bool isDie=false;
    private bool isGrounded;
    EnemyPlatformPass platformPass;
    Rigidbody2D enemyRigid;
    BoxCollider2D enemyCollider;
    Vector3 initPosition;
    PlayerMove playerMove;
    TimeBody timeBody;
    // Start is called before the first frame update
    void Awake()
    {
        SelecTarget();
        initPosition = transform.position;
        intTimer = timer;
        anim = GetComponent<Animator>();
        platformPass = GetComponent<EnemyPlatformPass>();
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        playerMove = FindAnyObjectByType<PlayerMove>();
        timeBody = GetComponent<TimeBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBody.isRewindin)
        {
            return;
        }
        if(isDie)
        {
            return;
        }
       if(!attackMode)
        {
            Move();

        }
       if(!InsideofLisits()&&!inRange&&!anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack"))
        {
            SelecTarget();
        }
        if (inRange)
        {
            if(onPlatform&&target.position.y<transform.position.y-0.5f)
            {
                platformPass.isPass = true;
            }
            else
            {
                platformPass.isPass = false;

            }
            if(target.position.y>transform.position.y+0.5f&& onStair==true)
            {
                target = leftLimit;
               
            }
            
            anim.SetBool("Run", true);
            //! LEGACY
            //    hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, rayCastMask);

            //    Debug.Log("�þ߿� �÷��̾ ������");

            //    RaycastDebugger();
            //}
            //if(hit.collider!=null)
            //{
            //    EnemyLogic();
            //}
            //else if(hit.collider==null)
            //{
            //    Debug.Log("hit.collider==null");
            //    inRange = false;
        }
       
        if (inRange)
        {
            EnemyLogic();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag=="Wall")
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            if(isDie)
            {
                target = null;
                return;
            }
            if(!onStair)
            {
            target = collision.transform;

            }
            inRange = true;
            Debug.Log("player�� Ʈ���ſ� ����");
        }
        Flip();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Platform"))
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack"))
            {
            onPlatform = true;

            }
        }
        if(collision.collider.tag.Equals("Stair"))
        {
            onStair = true;
        }
        if(collision.collider.tag.Equals("Floor")|| collision.collider.tag.Equals("Platform"))
        {
            isGrounded = true;
        }
        if(collision.collider.CompareTag("Player")||collision.collider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(enemyCollider, collision.collider);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Platform"))
        {
            onPlatform = false;
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            onStair = false;
        }
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            isGrounded = false;
        }
    }

    void EnemyLogic()
    {
        if(playerMove.isDie)
        {
            StopAttack();
            return;
        }
        distance = Vector2.Distance(transform.position, target.position);
        if(distance>attackDistance)
        {
            StopAttack();
        }
        else if(attackDistance>distance)
        {
            Attack();
        }
        if(cooldown)
        {
            anim.SetBool("Attack", false);
        }
    }
    void Move()
    {
        if(target!=null)
        {

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack")&&target.gameObject.tag!="Player")
        {
            anim.SetBool("Run", false);

            anim.SetBool("Walk", true);
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            enemyRigid.velocity = new Vector2(moveDirection.x*moveSpeed,enemyRigid.velocity.y);
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack") && target.gameObject.tag == "Player")
        {
            anim.SetBool("Walk", false);

            anim.SetBool("Run", true);
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            enemyRigid.velocity = new Vector2(moveDirection.x * (moveSpeed*2), enemyRigid.velocity.y);
        }
        }
        


    }
    public void Init()
    {
        isDie = false;
        inRange = false;
        target = null;
        SelecTarget();

    }
    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);

        anim.SetBool("Attack", true);

    }    
    void StopAttack()
    {
        cooldown = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }
    //void RaycastDebugger()
    //{
    //    if(distance>attackDistance)
    //    {
    //        Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.blue);
    //    }
    //    else if(attackDistance>distance)
    //    {
    //        Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);

    //    }
    //}
    private bool InsideofLisits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }
    public void SelecTarget()
    {
        if (state == EnemyState.Patrol)
        {

        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);
        if(distanceToLeft>distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }
        Flip();
        }
    }
   public void Flip()
    {
        if(target==null)
        {
            return;
        }
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x>target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0;
        }
        transform.eulerAngles = rotation;
    }

    public void Die()
    {
        anim.Play("Grunt_Die_Ground");
        isDie = true;
        EnemyCountManager.Instance.currentCount += 1;

        if (isGrounded)
        {
        StartCoroutine(DieRoutine());

        }
    }
    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        enemyRigid.velocity = Vector3.zero;
        enemyRigid.gravityScale = 0;
        
    }
}
