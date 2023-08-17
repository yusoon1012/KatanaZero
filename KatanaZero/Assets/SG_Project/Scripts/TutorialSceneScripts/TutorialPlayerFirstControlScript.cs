using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class TutorialPlayerFirstControlScript : MonoBehaviour
{
    public SG_PlayerMovement movement;
    public GameObject leftClickAttack;

    private Animator animator;

    private Coroutine playerStart;

    // waitForSecond = 0.25초
    private WaitForSeconds waitForSecond;

    private WaitForFixedUpdate waitForFixedUpdate;

    private Rigidbody2D rigid;

    private StateMachineBehaviour stateMachine;

    private AnimatorStateInfo stateInfo;
    float playbackTime;



    private bool isFall = false;

    // Idle = 0     Fall = 1     Roll = 2      Run = 3

    private float nowAni = 0;

    // 뛰어내려서 굴러야 할때 = 1 
    private float nowCutin = 0;

    private float moveSpeed = 9f;


    // Start is called before the first frame update
    public void Awake()
    {
        //stateMachine = GetComponent<StateMachineBehaviour>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        stateMachine = animator.GetBehaviour<AnimatorPlayerLayerScript>();

        waitForSecond = new WaitForSeconds(0.25f);
        waitForFixedUpdate = new WaitForFixedUpdate();

        movement.enabled = false;

    }

    void Start()
    {
        playerStart = StartCoroutine(StartTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0)
        {
            Debug.Log("TimeSclae 이 0 이여도 되나?");
            animator.Play("Attack");
            leftClickAttack.SetActive(true);
            Time.timeScale = 1;
        }
    }

    //  실행순서 2
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && nowAni == 1)
        {
           
            
            //isFall = false;
            animator.SetTrigger("ExitFall");
            animator.SetBool("FallBool", false);
            
            nowAni = 0;
            nowCutin = 1;

            rigid.velocity = Vector3.zero;
            this.rigid.AddForce(new Vector2(8f, 0f), ForceMode2D.Impulse);
            animator.Play("IsRoll");            

            playerStart = null;

            playerStart = StartCoroutine(Play3());
            //animator.Play("Run");




        }
    }

    //  실행순서 1
    private IEnumerator StartTutorial()
    {

        for (int i = 0; i <= 20; i++)
        {
            yield return waitForSecond;
        }

        this.rigid.AddForce(new Vector2(3f, 4f), ForceMode2D.Impulse);
        isFall = true;


        animator.SetBool("FallBool", isFall);
        //animator.Play("Fall");
        nowAni = 1;


    }

    // 실행순서3
    private IEnumerator Play3()
    {
        //while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        //{
        //    yield return waitForFixedUpdate;
        //}

        //while(stateInfo.IsName("IsRool") && playbackTime < 0.8f)
        //{
        //    yield return waitForFixedUpdate;
        //}
        rigid.velocity = Vector3.zero;
        yield return waitForFixedUpdate;

        for (int i = 0; i <= 55; i++)
        {
            
            //animator.Play("run");
            Vector3 move = new Vector3(moveSpeed, rigid.velocity.y, 0f);
            rigid.velocity = move;
            yield return waitForFixedUpdate;


        }
        rigid.velocity = Vector3.zero;
        this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);



    }

    private void MouseEvent()
    {
        
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



    }
}
