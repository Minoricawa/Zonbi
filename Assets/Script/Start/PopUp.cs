using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameObject pop_up = null;
    [SerializeField] GameObject options = null;
   // [SerializeField] GameObject pauses = null;
    [SerializeField]AudioClip se = null;

    // 以下メンバ変数定義.
    AudioSource audio_source = null;
    


    void Start()
    {
        pop_up.SetActive(false);
        options.SetActive(false);
        //pauses.SetActive(false);
        audio_source = GetComponent<AudioSource>();
    }


    //ポップアップ開閉
    public void Open()
    {
        pop_up.SetActive(true);
        audio_source.PlayOneShot(se);
    }
    public void Close()
    {
        pop_up.SetActive(false);
        audio_source.PlayOneShot(se);
    }


    //オプション画面開閉
    public void PushOption()
    {
        options.SetActive(true);
    }
    public void CloseOption()
    {
        options.SetActive(false);
    }


    //ポーズ画面開閉
    public void PushPause()
    {
        if (Input.GetButtonDown("Q"))
        {
            //pauses.SetActive(true);
        }
        
    }
    public void ClosePause()
    {
        //pauses.SetActive(false);
    }
}
