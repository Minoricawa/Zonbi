using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameScene : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameUI game_ui = null;
    [SerializeField] NotesContoller notes_controller = null;
    [SerializeField] Select select = null;
    //  [SerializeField] Transition transition = null;
  //  [SerializeField] GameObject fadein_black = null;
    [SerializeField] LaserController laser_controller_l = null;
    
    [SerializeField] LaserController laser_controller_r = null;
    //   [SerializeField] HandMenu hand_menu_l = null;
    //  [SerializeField] HandMenu hand_menu_r = null;
    [SerializeField] PopUp pop_up_l = null;
    [SerializeField] PopUp pop_up_r = null;
 //   [SerializeField] NoteBase note_base = null;

    // 以下メンバ変数定義.
    int id_ = 0;
    Paneru paneru;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        notes_controller.MissCallback = OnMiss;
        notes_controller.TimingCallback = OnTiming;
        notes_controller.GoodCallback = OnGood;
        notes_controller.BadCallback = OnBad;
        notes_controller.MusicFinishCallback = OnMusicFinish;
        select.SetPaneruCallback = OnSetPaneru;
        game_ui.GameoverCallback = OnGameOver;
        game_ui.ReplayCallback = OnReaplay;
        laser_controller_l.HitCallback = OnHitL;
        laser_controller_r.HitCallback = OnHitR;
        /*
        hand_menu_l.PaneruOpenCallback = OnPaneruOpen;
        hand_menu_r.PaneruOpenCallback = OnPaneruOpen;
        hand_menu_l.PaneruCloseCallback = OnPaneruClose;
        hand_menu_r.PaneruCloseCallback = OnPaneruClose;
        */
        pop_up_l.PaneruOpenCallback = OnPaneruOpen;
        pop_up_l.PaneruCloseCallback = OnPaneruClose;
        pop_up_r.PaneruOpenCallback = OnPaneruOpen;
        pop_up_r.PaneruCloseCallback = OnPaneruClose;
        //  transition.RetryCallback = OnRetry;



        // fadein_black.SetActive(true);
        //  StartCoroutine("DeleteFadeIn");
    }

    /*
    IEnumerator DeleteFadeIn()
    {
        yield return new WaitForSeconds(1.8f);
        fadein_black.SetActive(false);
    }
    */

    void Update()
    {

     
    }



    // ノーツに当たればHit
    void OnHitL()
    {
        notes_controller.HitNote = laser_controller_l.HitNote;
        notes_controller.Hit();
    }
    void OnHitR()
    {
        notes_controller.HitNote = laser_controller_r.HitNote;
        notes_controller.Hit();
    }

    // ミスをした場合の処理
    void OnMiss()
    {
        Debug.Log("GameScene OnMiss");
        game_ui.Damage();
        game_ui.ComboText();
    }

    // GOODの場合の処理
    void OnGood()
    {
        game_ui.ComboText();
        game_ui.ScoreText();
    }

    // BADの場合の処理
    void OnBad()
    {
        game_ui.ScoreText();
    }


    // 選曲パネルを決定した時の処理
    void OnSetPaneru(int id)
    {
        id_ = id;
        notes_controller.Play(id);
        game_ui.gameObject.SetActive(true);
        game_ui.SetUp(select.GetMusicTitle(id));
        GameInfo.NowGameStatus = GameInfo.GameStatus.Play;

        Debug.Log("OnSetPaneru");
        Debug.Log(game_ui.gameObject);
        Debug.Log(game_ui.gameObject.activeSelf);

    }
    

    // タイミング表示
    void OnTiming(string str)
    {
        game_ui.SetTiming(str);
    }


    // ゲームオーバー時の処理
    void OnGameOver()
    {
        Time.timeScale = 0;
        notes_controller.Music.Pause();
        GameInfo.NowGameStatus = GameInfo.GameStatus.GamgeOver;
    }

    // リプレイ時の処理
    void OnReaplay()
    {
        notes_controller.Replay();
        game_ui.SetUp();
        GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
    }


    // 曲が終わったらパネル表示、最大スコア・コンボ書き換え
    void OnMusicFinish()
    {
        select.OpenPaneruList();
        select.GameUISet();

        int high_combo = 0;
        if (PlayerPrefs.HasKey("HighCombo" + id_))
        {
            high_combo = PlayerPrefs.GetInt("HighCombo" + id_);
        }
        if (notes_controller.MaxCombo > high_combo)
        {
            PlayerPrefs.SetInt("HighCombo" + id_, notes_controller.MaxCombo);
        }

        int high_score = 0;
        if (PlayerPrefs.HasKey("HighScore" + id_))
        {
            high_score = PlayerPrefs.GetInt("HighScore" + id_);
        }
        if (notes_controller.Score > high_score)
        {
            PlayerPrefs.SetInt("HighScore" + id_, notes_controller.Score);
        }

        PlayerPrefs.Save();
        select.UpdateScore();
        GameInfo.NowGameStatus = GameInfo.GameStatus.Select;
    }


    // オプションパネルを開く・閉じるときに、プレイ中であれば曲等を止める・流す
    void OnPaneruOpen()
    {
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause)
        {
            notes_controller.Pause();
            Time.timeScale = 0;
        }
    }
    void OnPaneruClose()
    {
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause)
        {
            notes_controller.Resume();
            Time.timeScale = 1;
        }
    }
}
