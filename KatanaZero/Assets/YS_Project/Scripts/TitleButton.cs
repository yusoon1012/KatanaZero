using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    SceneManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void BossRoom()
    {
        SceneManager.LoadScene("BossScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
