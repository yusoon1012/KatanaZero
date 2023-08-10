using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_RedDottedLineControler : MonoBehaviour
{

    public GameObject dotted001;
    public GameObject dotted002;
    public GameObject dotted003;
    public GameObject dotted004;

    private float onOffDotted = 0f;
    private float dottedSpeed = 2f;
    private int dottedcontrolNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //dotted001 = GetComponent<GameObject>();
        //dotted002 = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        onOffDotted += dottedSpeed * Time.deltaTime;
        Debug.LogFormat("Dotted -> {0}", onOffDotted);

        if(onOffDotted >= 0.08f)
        {
            DottedLineControls();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {

        }

    }


    private void DottedLineControls()
    {
        if (dottedcontrolNum == 0)
        {
            onOffDotted = 0;
            dotted001.SetActive(true);
            dotted002.SetActive(false);
            dotted003.SetActive(false);
            dotted004.SetActive(false);
            dottedcontrolNum = 1;
        }
        else if(dottedcontrolNum == 1)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(true);
            dotted003.SetActive(false);
            dotted004.SetActive(false);
            dottedcontrolNum = 2;
        }
        else if (dottedcontrolNum == 2)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(false);
            dotted003.SetActive(true);
            dotted004.SetActive(false);
            dottedcontrolNum = 3;
        }
        else if (dottedcontrolNum == 3)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(false);
            dotted003.SetActive(false);
            dotted004.SetActive(true);
            dottedcontrolNum = 0;
        }


    }

}
