using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_HandEnemy : MonoBehaviour
{

    // Ray�� �÷��̾� Ȯ���ϰ� Collider�� �ǰ� ���� ���� �� ����
   


    // �÷��̾ �׾����� Ȯ���ϱ� ���� ����
    SG_PlayerMovement playerMovementClass;

    Animator animator;


    // �÷��̾� �߰��ߴ���
    private bool discoverPlayer = false;    

    private bool handEnemyDie = false;



    // Start is called before the first frame update
    void Start()
    {
        playerMovementClass = FindAnyObjectByType<SG_PlayerMovement>();
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("SG_PlayerAttack"))
        {
            handEnemyDie = true;
            animator.SetTrigger("IsDieTrigger");
        }

    }

    // �÷��̾ ���� ���������� ������
    public void NonDiscoverPlayerMove()
    {
        if(discoverPlayer == false)
        {
            //animator.SetTrigger()
        }
    }
}
