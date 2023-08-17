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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        kissyfaceCollider = GetComponent<BoxCollider2D>();
        throwAttack = GetComponent<Kissyface_Throw>();
        jumpAttack = GetComponent<KissyFace_JumpAttack>();
        lunge = GetComponent<Kissyface_Lunge>();
        StartCoroutine(SelectAction());
        Physics2D.IgnoreCollision(kissyfaceCollider, playerCollider);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAction)
        {
            
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
    }
    private IEnumerator SelectAction()
    {
        int waitSecond;
        if (lastPattern == LUNGE)
        {
            waitSecond = 2;
        }
        else
        {
            waitSecond = 1;
        }
        isAction = true;
        while(lastPattern==pattern)
        {
        pattern = Random.Range(1, 4);

        }

        
        lastPattern = pattern;
       
        yield return new WaitForSeconds(waitSecond);
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
