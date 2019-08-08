using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Text timing_log = null;
    [SerializeField] Text combo_log = null;
    [SerializeField] Text title = null;
    [SerializeField] Slider hp_slider = null;
    [SerializeField] Text score_log = null;
    [SerializeField] GameObject gameover = null;
    [SerializeField] NotesContoller notes_contoller = null;
    [SerializeField] GameObject black = null;

    // 以下メンバ変数定義.
    // string ui_log = null;
    System.Action gameover_callback = null;


    // 以下プロパティ.
    public System.Action GameoverCallback
    {
        set { gameover_callback = value; }
    }
    


    void Start()
    {
        gameover.SetActive(false);
        black.SetActive(false);
    }
    
    void Update()
    {

    }

    // 初期設定
    public void SeUp(string title_str)
    {
        combo_log.text = "Combo:000";
        title.text = title_str;
        hp_slider.value = 1;
        score_log.text = "Score:000000";
    }

    // タイミングによって出すテキストを表示 
    public void SetTiming(string str)
    {
        StartCoroutine("Timing", str);
    }
    
    IEnumerator Timing(string timing_log_text)
    {
        switch (timing_log_text)
        {
            case "good":
                timing_log.text = "GOOD!!";
                break;

            case "bad":
                timing_log.text = "BAD!!";
                break;

            case "miss":
                timing_log.text = "MISS!!";
                break;

            default:
                timing_log.text = "MISS!!";
                break;
        }
        
        yield return new WaitForSeconds(0.5f);

        timing_log.text = "";
    }


    // コンボ数を表示
    public void ComboText()
    {
        combo_log.text = "Combo:" + ((int)notes_contoller.Combo).ToString("000");
    }

    // コンボ数リセット
    public void ComboReset()
    {
        notes_contoller.Combo = 0;
    }


    // HPバーを減らす
    public void Damage()
    {
        Debug.LogFormat("hp_slider.value {0}", hp_slider.value);
        hp_slider.value = hp_slider.value - 0.2f;
        if (hp_slider.value <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    // スコア数を表示
    public void ScoreText()
    {
        score_log.text = "Score:" + ((int)notes_contoller.Score).ToString("000000");
    }

    // スコア数リセット
    public void ScoreReset()
    {
        notes_contoller.Score = 0;
    }

    // 体力0になるとゲームオーバー画面へ
    IEnumerator GameOver()
    {
        black.SetActive(true);

        yield return new WaitForSeconds(2.8f);
        gameover.SetActive(true);
        if (gameover_callback != null) gameover_callback();
    }
    
}
