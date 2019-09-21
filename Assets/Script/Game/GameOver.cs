using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameObject gameover = null;
    [SerializeField] GameObject black_out = null;
    [SerializeField] GameObject set = null;
    [SerializeField] GameObject point_light = null;
    [SerializeField] GameObject notes = null;
    [SerializeField] GameObject play_ui = null;

    // 以下メンバ変数定義.
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
        set.SetActive(true);
        point_light.SetActive(true);
        notes.SetActive(true);
        play_ui.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ゲームオーバー中
    public void Show()
    {
        if (is_show) return;
        is_show = true;
        StartCoroutine(GameOverCoroutine());
    }

    // 同じ曲でもう一度プレイ
    public void Replay()
    {
        if (replay_callback != null) replay_callback();
        black_out.SetActive(false);
        gameover.SetActive(false);
        is_show = false;
        set.SetActive(true);
        point_light.SetActive(true);
        notes.SetActive(true);
        play_ui.SetActive(true);
    }

    // 体力0になるとゲームオーバー画面へ
    IEnumerator GameOverCoroutine()
    {
        black_out.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        gameover.SetActive(true);
        if (gameover_callback != null) gameover_callback();
        set.SetActive(false);
        point_light.SetActive(false);
        notes.SetActive(false);
        black_out.SetActive(false);
        play_ui.SetActive(false);
    }
    
}
