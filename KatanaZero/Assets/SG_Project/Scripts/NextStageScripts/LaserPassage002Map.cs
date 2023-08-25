using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;
using Rewired;

public class LaserPassage002Map : MonoBehaviour
{
    // Ŭ����� ���� ������Ʈ
    public GameObject[] lights;
    public GameObject playerUI;

    // { �۷ι� ����Ʈ ���� ����
    public GameObject globalLightObj;
    private Light2D globalLight;

    // Start Color = 1 -> 0
    private Color globalLightStartColor;
    private Color globalLightEndColor;

    // ������ ���� ������
    private float lightStartTime = 0;
    private float lightEndTime = 2;

    // } �۷ι� ����Ʈ ���� ����

    // { �ؽ�Ʈ ���� ����
    public TextMeshProUGUI clearText;

    // A�� 0 -> 1
    private Color clearTextStartColor;
    private Color clearTextEndColor;

    private float textStartTime = 0;
    private float textEndTime = 2;
    // } �ؽ�Ʈ ���� ����

    Coroutine lightCoroutine;
    Coroutine textCoroutine;




    // �������̵��� �ѹ��� �����ֱ� ���� ����
    private bool laserPassageClear = false;

    private int sceneIdx;



    void Start()
    {
        globalLight = globalLightObj.GetComponent<Light2D>();
        //clearText = GetComponent<TextMeshProUGUI>();

        // ó���� �ؽ�Ʈ �����ϰ� �ؼ� Ŭ���� �Ҷ����� ���̰� ����
        clearText.color = clearTextStartColor;

        globalLightStartColor = new Color(1, 1, 1, 1);
        globalLightEndColor = new Color(0, 0, 0, 1);

        clearTextStartColor = new Color(1, 1, 1, 0);
        clearTextEndColor = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && laserPassageClear == false)
        {
            laserPassageClear = true;

            lightCoroutine = StartCoroutine(GlobalLightOff());
            textCoroutine = StartCoroutine(ClearTextOn());

        }
    }

    // �۷ι�����Ʈ�� �̿��ؼ� ȭ�� ���� ���������� ���ϴ� �ڷ�ƾ
    IEnumerator GlobalLightOff()
    {
        // ȭ�� �������Ǵµ� ���صǴ� ������Ʈ�� ����
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        playerUI.SetActive(false);

        while (lightStartTime < lightEndTime)
        {
            lightStartTime += Time.deltaTime;

            float time = Mathf.Clamp01(lightStartTime / lightEndTime);

            globalLight.color = Color.Lerp(globalLightStartColor, globalLightEndColor, time);

            yield return null;
        }

    }

    IEnumerator ClearTextOn()
    {
        while (textStartTime < textEndTime)
        {
            textStartTime += Time.deltaTime;

            float textTime = Mathf.Clamp01(textStartTime / textEndTime);

            clearText.color = Color.Lerp(clearTextStartColor, clearTextEndColor, textTime);

            yield return null;
        }

        yield return new WaitForSeconds(3);

        // ����� Index�� ��� �ű⿡ 1�� ���� ���� �θ��� �ϴ� ����
        if (EnemyCountManager.Instance.isAllClear)
        {
            sceneIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIdx + 1);
        }


    }


    //if (collision.tag.Equals("Player"))
    //{        
    //    if (EnemyCountManager.Instance.isAllClear)
    //    {
    //        sceneIdx = SceneManager.GetActiveScene().buildIndex;
    //        SceneManager.LoadScene(sceneIdx + 1);
    //    }
    //}
}
