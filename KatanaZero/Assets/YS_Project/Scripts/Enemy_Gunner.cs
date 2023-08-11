using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gunner : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooldown;
    private bool onPlatform;
    private bool onStair;
    private bool isDie = false;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
        

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            if (isDie)
            {
                target = null;
                return;
            }
            if (!onStair)
            {
                target = collision.transform;

            }
            inRange = true;
            Debug.Log("player가 트리거에 들어옴");
        }
        Flip();
    }
    public void Die()
    {

    }
    public void Flip()
    {
        if (target == null)
        {
            return;
        }
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0;
        }
        transform.eulerAngles = rotation;
    }
}
