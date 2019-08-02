using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] GameUI game_ui = null;
    [SerializeField] NotesContoller notes_controller = null;
    [SerializeField] Select select = null;


    // Start is called before the first frame update
    void Start()
    {
        notes_controller.MissCallback = OnMiss;
        notes_controller.TimingCallback = OnTiming;
        notes_controller.GoodCallback = OnGood;
        notes_controller.BadCallback = OnBad;
        select.SetPaneruCallback = OnSetPaneru;
        game_ui.GameoverCallback = OnGameOver;
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




    // Update is called once per frame
    void Update()
    {
        
    }
}
