using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kissyface_manager : MonoBehaviour
{
    public bool isAction = false;
    public int pattern=0;
    public int lastPattern=0;
    public BoxCollider2D playerCollider;
    public Transform playerTransform;
    public bool isAttackable = false;
    public bool isHit = false;
    private bool isBlock = false;  
    private KissyFace_JumpAttack jumpAttack;
    private Kissyface_Lunge lunge;
    private Kissyface_Throw throwAttack;
    private BoxCollider2D kissyfaceCollider;
    const int IDLE = 0;
    const int JUMP_ATTACK = 1;
    const int LUNGE = 2;
    const int THROW = 3;
    Animator anim;
    Vector3 leftAngle = new Vector3(0, 180, 0);
    Vector3 rightAngle = new Vector3(0, 0, 0);
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        kissyfaceCollider = GetComponent<BoxCollider2D>();
        throwAttack = GetComponent<Kissyface_Throw>();
        jumpAttack = GetComponent<KissyFace_JumpAttack>();
        lunge = GetComponent<Kissyface_Lunge>();
        StartCoroutine(SelectAction());
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(kissyfaceCollider, playerCollider);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAction)
        {
            rb.gravityScale = 1;
            isAttackable = false;
            if(playerTransform.position.x<transform.position.x&&transform.eulerAngles!=leftAngle)
            {
                transform.eulerAngles = leftAngle;
            }
            else if(playerTransform.position.x > transform.position.x && transform.eulerAngles != rightAngle)
            {
                transform.eulerAngles = rightAngle;
            }
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            StartCoroutine(SelectAction());
        }
        if(isHit)
        {
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            isAction = true;
        }
        if(!isHit&& isBlock)
        {
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            isAction = true;
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("공격에 맞았다.");
            if (isAttackable)
            {
                anim.Play("Kissyface_hurt");
                isHit = true;
                isAction = true;
                rb.gravityScale = 2;
                StartCoroutine(RecoverRoutine());

            }
            else
            {
                anim.Play("Kissyface_block");
                isAction = true;
                isBlock = true;
                StartCoroutine(BlockRoutine());
            }
        }
    }
    private IEnumerator SelectAction()
    {
        if(!isAction&&!isBlock)
        {
        isAction = true;
        int waitSecond;
        if (lastPattern == LUNGE)
        {
            waitSecond = 2;
        }
        else
        {
            waitSecond = 1;
        }
        while(lastPattern==pattern)
        {
        pattern = Random.Range(1, 4);

        }

        
        lastPattern = pattern;
       
        yield return new WaitForSeconds(waitSecond);
            if(isBlock)
            {
                yield break;
            }
        if(pattern==JUMP_ATTACK)
        {
            jumpAttack.enabled = true;
        }
        else if(pattern==LUNGE)
        {
            lunge.enabled = true;
        }
        else if(pattern==THROW)
        {
            throwAttack.enabled = true;
        }
        }
    }
    private IEnumerator RecoverRoutine()
    {
        yield return new WaitForSeconds(1);
        anim.Play("Kissyface_recover");
        isAction = false;
        isHit = false;

    }
    private IEnumerator BlockRoutine()
    {
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
        isAction = false;
        isBlock = false;
    }
}
