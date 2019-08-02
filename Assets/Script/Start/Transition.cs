using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // タイトル画面へ
    public void NextTitle()
    {
        SceneManager.LoadScene("Scene_title");
    }

    // セレクト画面へ
    public void NextSelect()
    {
        SceneManager.LoadScene("Scene_kyokai");
    }

    // ゲーム終了
    public void GameExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
