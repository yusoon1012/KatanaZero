using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialPlayerFirstControlScript : MonoBehaviour
{
    public SG_PlayerMovement movement;

    private Animator animator;

    private Coroutine playerStart;
    private WaitForSeconds waitForSecond;

    private Rigidbody2D rigid;

    private bool isFall = false;

    // Idle = 0     Fall = 1     Roll = 2      Run = 3

    private float nowAni = 0;

    // �پ���� ������ �Ҷ� = 1 
    private float nowCutin = 0;


    // Start is called before the first frame update
    public void Awake()
    {
        animator = GetComponent<Animator>();

        waitForSecond = new WaitForSeconds(0.25f);
        rigid = GetComponent<Rigidbody2D>();

        movement.enabled = false;

    }

    void Start()
    {
        playerStart = StartCoroutine(StartTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  ������� 2
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && nowAni == 1)
        {
            Debug.Log("�ݶ��̴� �ٴ��� �νĵ�?");
            isFall = false;
            animator.SetBool("Fallbool",isFall);
            nowAni = 0;
            nowCutin = 1;

            rigid.velocity = Vector3.zero;
            this.rigid.AddForce(new Vector2(7f, 0f), ForceMode2D.Impulse);
            animator.Play("IsRoll");

        }
    }

    //  ������� 1
    private IEnumerator StartTutorial()
    {

        for (int i = 0; i <= 20; i++)
        {
            yield return waitForSecond;
        }

        this.rigid.AddForce(new Vector2(3f, 4f), ForceMode2D.Impulse);
        isFall = true;


        animator.SetBool("FallBool", isFall);
        nowAni = 1;


    }
}
