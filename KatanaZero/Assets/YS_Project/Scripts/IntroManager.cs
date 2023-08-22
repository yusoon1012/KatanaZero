using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public AudioClip cassetClip;
    public AudioClip backgroundClip;
    public AudioSource bgm;
    public TMP_Text songName;
    public GameObject introUi;
    private bool isSlow = false;
    public IntroCanvas introCanvas;
    public bool introOver = false;
    GameObject targetObject ;
    private static IntroManager _instance;

    // ...

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // 이미 다른 씬에서 생성된 인스턴스가 있을 경우 이 인스턴스를 파괴합니다.
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = backgroundClip;
        targetObject = GameObject.Find("TimeManager");
    }

    // Update is called once per frame
    void Update()
    {


        if (introCanvas.isIntroOver == false)
        {
            return;
        }
        //if(introOver)
        //{
        //    targetObject.SetActive(true);
        //}


        
        if (TimeManager.Instance.isTimeSlow == true)
        {
            if (isSlow == false)
            {

                isSlow = true;
            }
            bgm.pitch = 0.4f;
        }
        else
        {
            bgm.pitch = 1f;
            isSlow = false;
        }
    }
    public void IntroAction()
    {
        bgm.Stop();
        bgm.PlayOneShot(cassetClip);
        StartCoroutine(IntroTimer());
    }
    private IEnumerator IntroTimer()
    {
        yield return new WaitForSeconds(3);
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
        songName.text = string.Format("{0}", backgroundClip.name);
        introUi.SetActive(true);
        introOver = true;

    }
}
