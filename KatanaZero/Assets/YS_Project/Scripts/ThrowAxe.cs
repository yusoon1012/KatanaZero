using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour
{
    public float speed = 10f;
    public float spinSpeed = 1000f;
    Rigidbody2D rb;
    private Vector3 rotate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = spinSpeed * Time.deltaTime;
        transform.Rotate(Vector3.back, rotationAmount);
    }
}
