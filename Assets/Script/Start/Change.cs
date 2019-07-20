using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //セレクト画面へ
    public void Push()
    {
        SceneManager.LoadScene("Scene_kyokai");
    }
}
