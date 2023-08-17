using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour
{
    public float speed = 10f;
    Rotate rotateClass;
    Rigidbody2D rb;
    private Vector3 rotate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        rotateClass = GetComponentInChildren<Rotate>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Wall"))
        {
            rb.velocity = Vector2.zero;
            rotateClass.isStop = true;
        }
    }
}
