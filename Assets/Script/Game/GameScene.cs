using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameUI game_ui = null;
    [SerializeField] NotesContoller notes_controller = null;
    [SerializeField] Select select = null;
    //  [SerializeField] Transition transition = null;
    [SerializeField] GameObject fadein_black = null;

    // 以下静的メンバ変数定義.
    static int high_combo = 0;
    static int high_score = 0;
    int id_ = 0;

    // 以下プロパティ.
    public static int HighCombo
    {
        get { return high_combo; }
    }
    public static int HighScore
    {
        get { return high_score; }
    }
    


    // Start is called before the first frame update
    void Start()
    {
        notes_controller.MissCallback = OnMiss;
        notes_controller.TimingCallback = OnTiming;
        notes_controller.GoodCallback = OnGood;
        notes_controller.BadCallback = OnBad;
        notes_controller.MusicFinishCallback = OnMusicFinish;
        select.SetPaneruCallback = OnSetPaneru;
        game_ui.GameoverCallback = OnGameOver;
        //  transition.RetryCallback = OnRetry;


        //   high_combo = 0;
        //   high_score = 0;


        fadein_black.SetActive(true);
        StartCoroutine("DeleteFadeIn");
    }


    IEnumerator DeleteFadeIn()
    {
        yield return new WaitForSeconds(2.8f);
        fadein_black.SetActive(false);
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
        id = id_;
        notes_controller.Play(id);
        game_ui.gameObject.SetActive(true);
        game_ui.SeUp(select.GetMusicTitle(id));
        game_ui.ComboReset();
        game_ui.ScoreReset();
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
    }



    // 曲が終わったらパネル表示、最大スコア・コンボ書き換え
    void OnMusicFinish()
    {
        select.OpenPaneruList();
        select.GameUISet();
        //  paneru.MaxCombo();
        //  paneru.MaxScore();

        if (notes_controller.Combo > high_combo)
        {
            high_combo = notes_controller.Combo;
        }

        if (notes_controller.Score > high_score)
        {
            high_score = notes_controller.Score;
        }
    }

    /*
    // リトライ
    void OnRetry(int id)
    {
        OnSetPaneru(id);
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
