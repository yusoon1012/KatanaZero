using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SG_SceneManager : MonoBehaviour
{

    //Scene scene;
    Scene nowScene;

    ClearText001 clearText001Script;

    // LaserPassage ����
    private bool isLaserPassageSceneClear = false;
    private bool laserPassageGet = false;
    public GameObject laserPassageSceneTextObj;

    // Start is called before the first frame update
    public void Awake()
    {
        // ����� ����
        nowScene = SceneManager.GetActiveScene();




    }

    void Start()
    {        

    }

    // Update is called once per frame
    void Update()
    {
        GetInstance();
        ChangeScene();
    }

    // ----------------------- Ŭ����� �̵���ų �� ---------------------------

    public void ChangeScene()
    {
        if (nowScene.name == ("LaserPassage") && isLaserPassageSceneClear == true)
        {
            SceneManager.LoadScene("SG_LaserPassage002");
        }
        else { /*PASS*/ }
    }

    // ---------------------- �ؽ�Ʈ ������Ʈ�� ������ �ν��Ͻ� -------------------------

    public void GetInstance()
    {
        #region LaserPassage
        if (nowScene.name == ("LaserPassage")) // LaserPassage
        {
            if (laserPassageSceneTextObj.activeSelf == true && laserPassageGet == false)
            {
                laserPassageGet = true;
                // LaserPassageScene �� �̺�Ʈ ������������ Get
                clearText001Script = FindObjectOfType<ClearText001>();
                // LaserPassageScene Ŭ���� �̺�Ʈ ����                
                clearText001Script.endClearTextEvent += IsLaserPassageSceneClear;
            }
        }
        #endregion LaserPassage



    }


    // ---------------------- �̺�Ʈ ���� �Լ� --------------------------------
    public void IsLaserPassageSceneClear(bool eventValue)
    {
        isLaserPassageSceneClear = eventValue;
        //Debug.LogFormat("�̺�Ʈ�� �� �ҷ������� �ٲ����? -> {0}", isLaserPassageSceneClear);
    }


}
