using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Text timing_log = null;
    [SerializeField] Text combo_log = null;

    // 以下メンバ変数定義.
    string ui_log = null;
    

    void Start()
    {

    }
    
    void Update()
    {
        ui_log = NotesContoller.TimingLog;
        StartCoroutine(Timing(ui_log));
    }
    

    //タイミングによって出すテキストを表示
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
        }
        
        yield return new WaitForSeconds(0.5f);

        timing_log.text = "";
    }

    //コンボ数を表示
    void ComboText()
    {
        combo_log.text = ((int)NotesContoller.Combo).ToString("0");
    }
    
}
