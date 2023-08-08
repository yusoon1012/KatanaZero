using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SG_AttackImfact : MonoBehaviour
{

    public Transform player;
    private Animator empectAni;
    private SG_PlayerMovement playerMovementClass;

    private float pitagorasC;
    private float playerZ;
    private float mouseZ;


    // Start is called before the first frame update
    void Start()
    {
        empectAni = GetComponent<Animator>();
        playerMovementClass = FindObjectOfType<SG_PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsClick();
    }
    public void LateUpdate()
    {
       
    }

    private void PlayerIsClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (playerMovementClass.leftClickAttackCoolTime == false && playerMovementClass.attackCount < 4)
            {
                /*      LEGACY
                //playerZ = this.transform.position.x + this.transform.position.y;
                //mouseZ = playerMovementClass.mousePosition.x + playerMovementClass.mousePosition.y;

                //pitagorasC = playerZ + mouseZ;

                //// ��Ÿ��󽺾� ����� ���
                //Debug.LogFormat("��Ÿ��󽺾� -> {0}", pitagorasC);
                ////this.gameObject.transform.LookAt(playerMovementClass.mousePosition);
                ////this.gameObject.transform.localRotation = Quaternion.Euler(this.gameObject.transform.localRotation.x,0f, this.gameObject.transform.localRotation.z);

                //this.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, pitagorasC);
                ////this.gameObject.transform.Rotate(new Vector3(0f, 0f, playerMovementClass.mousePosition.z));
                         LEGACY */


                var dir = playerMovementClass.mousePosition - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Debug.LogFormat("mouse -> {0}", playerMovementClass.mousePosition);
                Debug.LogFormat("player -> {0}", transform.position);
                Debug.LogFormat("dir -> {0}", dir);

                transform.localScale = player.localScale;
                

                empectAni.SetTrigger("Slash");
            }
        }

    }
}
