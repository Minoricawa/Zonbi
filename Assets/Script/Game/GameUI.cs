using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameUI : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Text timing_log = null;
    [SerializeField] Text combo_log = null;
    [SerializeField] Text title = null;
    [SerializeField] Slider hp_slider = null;
    [SerializeField] Text score_log = null;    
    [SerializeField] NotesContoller notes_contoller = null;
    [SerializeField] GameOver game_over = null;
    
    
    // 以下プロパティ.
    public System.Action GameoverCallback
    {
        set { game_over.GameoverCallback = value; }
    }

    public System.Action ReplayCallback
    {
        set { game_over.ReplayCallback = value; }
    }
    



    void Start()
    {

    }
    
    void Update()
    {

    }

    // 初期設定
    public void SetUp(string title_str = "")
    {
        combo_log.text = "Combo:000";
        if (title_str != "") title.text = title_str;
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
                timing_log.color = Color.yellow;
                break;

            case "bad":
                timing_log.text = "BAD!!";
                timing_log.color = Color.blue;
                break;

            case "miss":
                timing_log.text = "MISS!!";
                timing_log.color = Color.red;
                break;

            default:
                timing_log.text = "MISS!!";
                timing_log.color = Color.red;
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
    

    // HPバーを減らす
    public void Damage()
    {
        hp_slider.value = hp_slider.value - 0.1f;
        if (hp_slider.value <= 0)
        {
            game_over.Show();
        }
    }

    // スコア数を表示
    public void ScoreText()
    {
        score_log.text = "Score:" + ((int)notes_contoller.Score).ToString("000000");
    }
}
