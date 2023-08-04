using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 3f;
    public float jumpForce = 3f;
    private bool isJump = false;
    private bool readyRun = false;


    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

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
        if(player.GetButton("MoveLeft"))
        {
            Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            playerRigid.velocity = move;
        }        
        if (player.GetButton("MoveRight"))
        {
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

        if (player.GetButtonDown("Jump") && isJump == false)
        {
            playerRigid.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);
            //���� �������� �� ���� ���ϰ� bool ó��
            isJump = true;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //���⼭ �ٽ� ��������
        isJump = false;
    }

}
