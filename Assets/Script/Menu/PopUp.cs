using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameObject pop_up = null;
    [SerializeField] GameObject options = null;
    [SerializeField] AudioClip se = null;

    // 以下メンバ変数定義.
    AudioSource audio_source = null;
    System.Action paneru_open_callback = null;
    System.Action paneru_close_callback = null;


    // 以下プロパティ.
    public System.Action PaneruOpenCallback
    {
        get { return paneru_open_callback; }
        set { paneru_open_callback = value; }
    }
    public System.Action PaneruCloseCallback
    {
        get { return paneru_close_callback; }
        set { paneru_close_callback = value; }
    }



    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        options.SetActive(false);
        pop_up.SetActive(false);
    }

    void Update()
    {

    }



    // ポップアップ開閉
    public void Open()
    {
        pop_up.SetActive(true);
        audio_source.PlayOneShot(se);
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Play) GameInfo.NowGameStatus = GameInfo.GameStatus.Pause;
        if (paneru_open_callback != null) paneru_open_callback();
    }
    public void Close()
    {
        audio_source.PlayOneShot(se);
        StartCoroutine(Wait());
        if (paneru_close_callback != null) paneru_close_callback();
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause) GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
    }
    IEnumerator Wait()
    {
        //0.17秒待つ
        yield return new WaitForSeconds(0.17f);
        pop_up.SetActive(false);
        if (options) options.SetActive(false);
    }


    // オプション画面開閉
    public void PushOption()
    {
        audio_source.PlayOneShot(se);
        options.SetActive(true);
    }
    public void CloseOption()
    {
        audio_source.PlayOneShot(se);
        options.SetActive(false);
    }

}
