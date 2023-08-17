using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAxe : MonoBehaviour
{
    public Transform center;    // 원운동 중심점
    public float radius; // 반지름
    public float speed = 20f;  // 속도
    bool isMaxSize = false;
    private Vector3 initialCenterPosition; // 초기 중심점 위치
    private float angle = 0;
    private void Start()
    {
        radius = 0.1f;
        Destroy(gameObject, 1f);
        initialCenterPosition = transform.position;
    }
    void Update()
    {
        if(radius<0)
        {
            return;
        }
        if (radius >=3f)
        {
            isMaxSize = true;

        }
        
        if(!isMaxSize)
        {
            radius += 5f*Time.deltaTime;

        }
        else
        {
            radius -= 5f * Time.deltaTime;

        }
        angle += speed * Time.deltaTime;
        //transform.position = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        Vector3 newPosition = initialCenterPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        transform.position = newPosition;
        Vector3 lookDirection = initialCenterPosition - newPosition;
        float rotationAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle+90f);

    }
}
