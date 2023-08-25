using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        // GameManager �ν��Ͻ��� �̹� �ִ� ��� ���� �������� �ʰ� ���� �ν��Ͻ��� ����մϴ�.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �����ǵ��� ����
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� ���� ���� ������Ʈ�� �ı��մϴ�.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
            UnityEngine.Cursor.visible = false;
        
    }
}
