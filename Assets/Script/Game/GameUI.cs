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
    [SerializeField] GameObject now_option = null;
    [SerializeField] GameObject pause = null;


    // 以下メンバ変数定義.
    // string ui_log = null;
    bool pause_flag = false;

    // 以下プロパティ.
    public System.Action GameoverCallback
    {
        set { game_over.GameoverCallback = value; }
    }

    public System.Action ReplayCallback
    {
        set { game_over.ReplayCallback = value; }
    }


 //   var trackedObject = GetComponent<SteamVR_TrackedObject>();
 //   var device = SteamVR_Controller.Input((int)trackedObject.index);




    void Start()
    {
        pause_flag = false;
        pause.SetActive(false);
        now_option.SetActive(false);
    }
    
    void Update()
    {
        if (SteamVR_Actions.default_Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            PushPause();
        }
        if (SteamVR_Actions.default_Teleport.GetStateUp(SteamVR_Input_Sources.Any))
        {
            ClosePause();
        }
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
    

    // HPバーを減らす
    public void Damage()
    {
        Debug.LogFormat("hp_slider.value {0}", hp_slider.value);
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


    // ポーズ画面開閉
    public void PushPause()
    {
         pause_flag = true;
        
         pause.SetActive(true);

         GameObject go = GameObject.Find("SelectPaneru");
         if (go != null)
         {
             NotesContoller notes_controller = go.GetComponent<NotesContoller>();
             notes_controller.Pause();
             Time.timeScale = 0;
         }

         GameInfo.NowGameStatus = GameInfo.GameStatus.Pause;
        
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
            pause_flag = false;
        }
        GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
    }
    

    // ゲーム中のオプション画面開閉
    public void PushGameNowOption()
    {
        now_option.SetActive(true);
    }
    public void CloseGameNowOption()
    {
        now_option.SetActive(false);
    }
}
