using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float spinSpeed = 1000f;
    public bool isStop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStop)
        {
        float rotationAmount = spinSpeed * Time.deltaTime;
        transform.Rotate(Vector3.back, rotationAmount);

        }
    }
    public void StopRotate()
    {
        isStop = true;
    }
}
