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
        notes_controller.GoodCalloback = OnGood;
        select.SetPaneruCallback = OnSetPaneru;
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
    }


    // 選曲パネルを決定した時の処理
    void OnSetPaneru(int id)
    {
        notes_controller.Play(id);
        game_ui.gameObject.SetActive(true);
        game_ui.SeUp(select.GetMusicTitle(id));
    }

    // タイミング表示
    void OnTiming(string str)
    {
        game_ui.SetTiming(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
