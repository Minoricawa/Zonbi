using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    //  [SerializeField] Select select_ = null;

    // 以下メンバ変数定義.
    int id_ = 0;
    System.Action retry_callback = null;

    // 以下プロパティ.
    public System.Action RetryCallback
    {
        set { retry_callback = value; }
    }





    void Start()
    {
       
    }

    
    void Update()
    {
        
    }

    public void GetId(int id)
    {
        id_ = id;
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
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    /*
    // 選択した曲をリトライ
    public void Retry(int id)
    {
       if (retry_callback != null) retry_callback(id);
    }
    */
    
}
