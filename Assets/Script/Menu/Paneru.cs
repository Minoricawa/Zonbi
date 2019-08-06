﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paneru : MonoBehaviour
{
    // 以下公開メンバ変数定義.
    public int id;

    // 以下メンバ変数定義(SerializeField).
    [SerializeField] AudioSource audio_souce = null;
    [SerializeField] Text author_text = null;
    [SerializeField] Text nando_text = null;
    [SerializeField] Text name_text = null;
    [SerializeField] Image image = null;
    [SerializeField] Text max_combo_text = null;
    [SerializeField] Text max_score_text = null;

    // 以下メンバ変数定義.
    CanvasGroup canvas = null;
    IEnumerator fadein = null;
    IEnumerator fadeout = null;
    string title_name = null;
    int high_combo = 0;
    int high_score = 0;



    // 以下公開関数定義
    public System.Action<int> click_callback = null;


    // 以下プロパティ.
    public string TitleName
    {
        get { return title_name; }
    }

    public AudioSource AudioSource
    {
        get { return audio_souce; }
    }

    public void MusicPlay()
    {
        if (audio_souce.isPlaying != true)
        {
            audio_souce.Play();
        }
    }

    public void MusicStop()
    {
        if (audio_souce.isPlaying)
        {
            audio_souce.Stop();
        }
    }


    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        max_combo_text.text = "0000000";
        max_score_text.text = "0000000";
        high_combo = 0;
        high_score = 0;
    }



    // パネルに記載する情報をjsonから拝借、表示
    public void Setup(string json_path , AudioClip audio_clip ,string image_path)
    {
        
        string json_text = Resources.Load<TextAsset>(json_path).ToString();
        JsonNode json = JsonNode.Parse(json_text);
        title_name = json["title"].Get<string>();
        name_text.text = title_name;
        author_text.text = json["author"].Get<string>();
        nando_text.text = json["nando"].Get<string>();
        image.sprite = Resources.Load<Sprite>(image_path);
        audio_souce.clip = audio_clip;
        
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


    // 最大コンボなら書き換え
    public void MaxCombo()
    {
        max_combo_text.text = ((int)NotesContoller.Combo).ToString("0000000");  // きてる、けど変わらない
        if (NotesContoller.Combo > high_combo)
        {
            max_combo_text.text = ((int)NotesContoller.Combo).ToString("0000000");  // きてない
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
