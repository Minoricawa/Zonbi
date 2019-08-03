using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameObject pop_up = null;
    [SerializeField] GameObject options = null;
    [SerializeField] AudioClip se = null;
    [SerializeField] GameObject pause = null;
    [SerializeField] GameObject now_option = null;

    // 以下メンバ変数定義.
    AudioSource audio_source = null;
    bool pause_flag = false;


    void Start()
    {
        pop_up.SetActive(false);
        options.SetActive(false);
        pause.SetActive(false);
        now_option.SetActive(false);
        audio_source = GetComponent<AudioSource>();
        pause_flag = false;
    }

    void Update()
    {
        // スペースを押すとポーズ画面表示
        if (Input.GetKeyDown(KeyCode.Space) && !pause_flag)
        {
            PushPause();
        }
    }



    // ポップアップ開閉
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


    // オプション画面開閉
    public void PushOption()
    {
        options.SetActive(true);
    }
    public void CloseOption()
    {
        options.SetActive(false);
    }


    // ポーズ画面開閉
    public void PushPause()
    {
        pause.SetActive(true);

        GameObject go = GameObject.Find("SelectPaneru");
        if (go != null)
        {
            NotesContoller notes_controller = go.GetComponent<NotesContoller>();
            notes_controller.Pause();
            Time.timeScale = 0;
        }
        
    }
    public void ClosePause()
    {
        pause.SetActive(false);
        GameObject go = GameObject.Find("SelectPaneru");
        if (go != null)
        {
            NotesContoller notes_controller = go.GetComponent<NotesContoller>();
            notes_controller.Resume();
            Time.timeScale = 1;
        }
    }

    // ゲーム中のオプション画面開閉
    public void PushGameNowOption()
    {
        now_option.SetActive(true);
        pause_flag = true;
    }
    public void CloseGameNowOption()
    {
        now_option.SetActive(false);
        pause_flag = false;
    }
}
