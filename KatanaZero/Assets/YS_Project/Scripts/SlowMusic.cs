using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowMusic : MonoBehaviour
{
    AudioSource bgm;
    TimeManager timeManager;
    Kissyface_manager kissyface;
    private static SlowMusic _instance;

    // �ٸ� ��ũ��Ʈ���� TimeManager�� ������ �� ����� ���� �ν��Ͻ�
    public static SlowMusic Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SlowMusic>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // �̹� �ٸ� ������ ������ �ν��Ͻ��� ���� ��� �� �ν��Ͻ��� �ı��մϴ�.
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;
    }
    void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
        bgm = GetComponent<AudioSource>();
        kissyface = FindAnyObjectByType<Kissyface_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(kissyface.isDie)
        {
            Destroy(gameObject);
        }
        if(timeManager.isTimeSlow==true)
        {
            bgm.pitch = 0.5f;
        }
        else if(timeManager.isTimeSlow==false)
        {
            bgm.pitch = 1;
        }

    }
}
