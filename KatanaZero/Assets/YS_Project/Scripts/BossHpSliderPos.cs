using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpSliderPos : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    Vector3 setPosition = new Vector3(target.position.x,target.position.y+ 0.5f, 0);
        transform.position = setPosition;
    }
}
