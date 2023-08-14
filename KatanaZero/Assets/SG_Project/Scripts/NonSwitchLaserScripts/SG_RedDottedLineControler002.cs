using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SG_RedDottedLineControler002 : MonoBehaviour
{
    public GameObject shotLaser;

    public GameObject dotted001;
    public GameObject dotted002;
    public GameObject dotted003;
    public GameObject dotted004;

    private Switch switchClass;


    private bool redDottedIsButtonSwitch = true;

    #region StackOverflow
    //public event Action<bool> redDottedLineBoolEvent;

    //private bool isChangeEventBool = false;

    //public bool IsChangeEventBool
    //{
    //    get { return isChangeEventBool; }

    //    set 
    //    {
    //        if (isChangeEventBool != value)
    //        {
    //            IsChangeEventBool = value;
    //            redDottedLineBoolEvent?.Invoke(isChangeEventBool);
    //        }
    //    }
    //}
    #endregion

    //private bool getEventbool = false;

    private float onOffDotted = 0f;
    private float dottedSpeed = 2f;
    private int dottedcontrolNum = 0;
    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("redDottedIsButtonSwitch �� ->{0}", redDottedIsButtonSwitch);
      
            onOffDotted += dottedSpeed * Time.deltaTime;
            //Debug.LogFormat("Dotted -> {0}", onOffDotted);

            if (onOffDotted >= 0.08f)
            {
                DottedLineControls();
            }

            else { /*PASS*/ }
        
    }

    void OnEnable()
    {
        // SetActive == true �� �� ������ �۾�



    }

    void OnDisable()
    {
        // SetActive == flase �� �� ������ �۾�
       
            shotLaser.gameObject.SetActive(true);

        
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (redDottedIsButtonSwitch == true)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                this.gameObject.SetActive(false);
         
            }
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
        else if (dottedcontrolNum == 1)
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
