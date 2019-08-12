using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameObject gameover = null;
    [SerializeField] GameObject black_out = null;

    // 以下メンバ変数定義.
    // string ui_log = null;
    System.Action replay_callback = null;
    System.Action gameover_callback = null;
    bool is_show = false;

    // 以下プロパティ.
    public System.Action ReplayCallback
    {
        set { replay_callback = value; }
    }
    public System.Action GameoverCallback
    {
        set { gameover_callback = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameover.SetActive(false);
        black_out.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        if (is_show) return;
        is_show = true;
        StartCoroutine(GameOverCoroutine());
    }

    public void Replay()
    {
        if (replay_callback != null) replay_callback();
        black_out.SetActive(false);
        gameover.SetActive(false);
        is_show = false;
    }

    // 体力0になるとゲームオーバー画面へ
    IEnumerator GameOverCoroutine()
    {
        black_out.SetActive(true);

        yield return new WaitForSeconds(2.8f);
        gameover.SetActive(true);
        if (gameover_callback != null) gameover_callback();
    }

    



}
