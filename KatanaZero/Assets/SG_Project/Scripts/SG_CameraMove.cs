using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SG_CameraMove : MonoBehaviour
{


    public Transform target;
    public Vector2 offset;
    public Vector2 minPosition;
    public Vector2 maxPosition;


    //public Transform target; // �÷��̾��� Transform ������Ʈ�� ���⿡ �Ҵ��մϴ�.
    //public Vector2 offset; // ī�޶�� �÷��̾� ������ �Ÿ��� �����ϴ� ������ ���Դϴ�.

    void Awake()
    {
        //this.transform.position = new Vector3(0f, 0.75f, -10f);

        offset = new Vector2(0f, 0.75f);
        minPosition = new Vector2(-1.5f, -2f);
        maxPosition = new Vector2(105f, 30f);

    }

    public void Start()
    {
        
    }

    public void Update()
    {

    }





    void LateUpdate()
    {
        if (target == null)
            return;

        // �÷��̾��� ���� ��ġ�� �������� ���� ��ġ�� ī�޶� �̵���ŵ�ϴ�.
        Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

    

        // ī�޶��� ��ġ�� ������ ���� ������ �����մϴ�.
        float clampedX = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        // ���ѵ� ���� ī�޶� ��ġ�� �����մϴ�.
        
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);


    }
}


