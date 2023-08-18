    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    Vector3 initialPosition;
    bool isReflected = false;
    public bool dieTrigger = false;
    TimeManager timeManager;
    TrailRenderer trail;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        timeManager = FindAnyObjectByType<TimeManager>();
        trail = GetComponent<TrailRenderer>();
        initialPosition = transform.position;
    }
    private void Update()
    {
        if (timeManager != null)
        {

        if(timeManager.isTimeSlow)
        {
            trail.enabled = false;
        }
        else
        {
            trail.enabled = true;
        }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall") || collision.tag.Equals("Floor"))
        {
            Destroy(gameObject);
        }
        else if (collision.tag.Equals("Reflect") && !isReflected)
        {
            dieTrigger = true;
            ReflectBullet();
        }
        if(collision.tag.Equals("Enemy")&&dieTrigger==true)
        {
            EnemyRay enemyRay = collision.GetComponent<EnemyRay>();
            if (enemyRay != null)
            {
                enemyRay.Die();
            }

            Enemy_Gunner enemyGunner = collision.GetComponent<Enemy_Gunner>();
            if (enemyGunner != null)
            {
                enemyGunner.Die();
            }
        }
    }

    private void ReflectBullet()
    {
        Vector2 normal = new Vector2(-rb.velocity.x, -rb.velocity.y).normalized;  // ���� �̵� ������ �ݴ� ���� ���
        Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, normal);  // ������ ���� �ݻ� ���� ���
        rb.velocity = reflectedDirection * speed;
        isReflected = true;
    }
}
