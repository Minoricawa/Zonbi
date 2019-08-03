using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPaneruContainer : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Text max_combo_text = null;
    [SerializeField] Text max_score_text = null;

    // 以下メンバ変数定義.
    int high_combo = 0;
    int high_score = 0;

    // Start is called before the first frame update
    void Start()
    {
        max_combo_text.text = "0000000";
        max_score_text.text = "0000000";
        high_combo = 0;
        high_score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    // 最大コンボなら書き換え
    public void MaxCombo()
    {
        if (NotesContoller.Combo > high_combo)
        {
            max_combo_text.text = ((int)NotesContoller.Combo).ToString("0000000");
            high_combo = NotesContoller.Combo;
        }
    }

    // 最大スコアなら書き換え
    public void MaxScore()
    {
        if (NotesContoller.Score > high_score)
        {
            max_score_text.text = ((int)NotesContoller.Score).ToString("0000000");
            high_score = NotesContoller.Score;
        }
    }
}
