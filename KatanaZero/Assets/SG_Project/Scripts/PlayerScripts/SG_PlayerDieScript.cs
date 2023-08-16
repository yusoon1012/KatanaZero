using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerDieScript : MonoBehaviour
{
    // �÷��̾ �׾����� ��Ҵ��� ������ ���� �ٸ������� ���������� public ���� �����
    public bool isPlayerDie = false;

    private Rigidbody2D rigid;

    public Sprite[] dieSprite = new Sprite[2];
    private SpriteRenderer spriteRenderer;
    private Animator playerAni;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isPlayerDie = true;
            playerAni.enabled = false;
            WhenPlayerDie();

            spriteRenderer.sprite = dieSprite[0];
            Invoke("DieRenderer", 0.5f);
            // �÷��̾� ������
            if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                rigid.AddForce(new Vector2(-8f, -5f), ForceMode2D.Impulse);
            }
            else if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                rigid.AddForce(new Vector2(8f, 5f), ForceMode2D.Impulse);
            }

            // 0.5 �ʵ� RigidBody��ȣ�ۿ� ����
            Invoke("StopRigid", 0.5f);


        }
    }

    public void StopRigid()
    {
        rigid.simulated = false;
    }
    public void DieRenderer()
    {
        spriteRenderer.sprite = dieSprite[1];
    }


    public void WhenPlayerDie()
    {
        // ������ ����
        this.gameObject.GetComponent<SG_PlayerMovement>().enabled = false;
    }
}
