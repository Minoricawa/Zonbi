using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paneru : MonoBehaviour
{
    // 以下公開メンバ変数定義.
    public int id;

    // 以下メンバ変数定義(SerializeField).
    [SerializeField] string json_path = null;
    [SerializeField] Text author_text = null;
    [SerializeField] Text nando_text = null;
    [SerializeField] Text name_text = null;

    // 以下メンバ変数定義.
    CanvasGroup canvas = null;
    IEnumerator fadein = null;
    IEnumerator fadeout = null;
    string title_name = null;

    // 以下公開関数定義
    public System.Action<int> click_callback = null;


    // 以下プロパティ.
    public string TitleName
    {
        get { return title_name; }
    }




    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        LoadChart();
    }



    // パネルに記載する情報をjsonから拝借、表示
    void LoadChart()
    {
        string json_text = Resources.Load<TextAsset>(json_path).ToString();
        JsonNode json = JsonNode.Parse(json_text);

        title_name = json["title"].Get<string>();
        name_text.text = title_name;
        author_text.text = json["author"].Get<string>();
        nando_text.text = json["nando"].Get<string>();

    }


    
    // フェードをリセット
    void RsetFade()
    {
        if (fadein != null)
        {
            StopCoroutine(fadein);
            fadein = null;
        }

        if (fadeout != null)
        {
            StopCoroutine(fadeout);
            fadeout = null;
        }
    }


   // フェード
    public void FadeIn(float speed = 5)
    {
        if (canvas == null) return;
        RsetFade();
        fadein = FadeInTime(1, speed);
        StartCoroutine(fadein);
    }

    public void FadeOut(float alpha ,float speed = 5)
    {
        if (canvas == null) return;
        RsetFade();
        fadeout = FadeOutTime(alpha, speed);
        StartCoroutine(fadeout);
    }

    IEnumerator FadeInTime(float alpha ,float speed)
    {
        while(canvas.alpha < alpha)
        {
            canvas.alpha += Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeOutTime(float alpha, float speed)
    {
        while (canvas.alpha > alpha)
        {
            canvas.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }


    // SelectのClickPaneru発動
    public void Click()
    {
        if (click_callback != null)
        {
            click_callback(id);
        }
       
    }


}
