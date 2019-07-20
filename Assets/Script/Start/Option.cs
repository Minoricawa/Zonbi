using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Option : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Text speed = null;
    [SerializeField] Text buki = null;
    [SerializeField] int sp = 0;
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] Image button_lr = null;
    [SerializeField] Image button_ud = null;

    // 以下静的メンバ変数定義.
    static public bool eyes_lr = false;
    static public bool eyes_ud = false;

    // 以下メンバ変数定義.
    string te = "";
    bool buki_ = false;
    
    // 以下プロパティ.
    public int Sp
    {
        get { return sp; }
    }
    


    void Start()
    {
        sp = 5;
        buki_ = true;
        eyes_lr = true;
        eyes_ud = true;
        
    }

    
    //ノーツスピード変更(現在は表示のみ変わる)
    public void SpeedUp()
    {
        if (sp < 10)
        {
            sp++;
            te = sp.ToString("0");
            speed.text = te;
        }
        else
        {
            return;
        }

    }
    public void SpeedDown()
    {
        if (sp > 1)
        {
            sp--;
            te = ((int)sp).ToString("0");
            speed.text = te;
        }
        else
        {
            return;
        }

    }



    //視点左右反転
    public void EyesLR()
    {

        if (eyes_lr)
        {
            eyes_lr = false;
            button_lr.color = new Color(192f / 255f, 192f / 255f, 192f / 255f);
        }
        else
        {
            eyes_lr = true;
            button_lr.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
    }
    //視点上下反転
    public void EyesUD()
    {

        if (eyes_ud)
        {
            eyes_ud = false;
            button_ud.color = new Color(192f / 255f, 192f / 255f, 192f / 255f);
        }
        else
        {
            eyes_ud = true;
            button_ud.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
    }


    //BGM音量調節(現在はタイトルのみ反映)
    public void BGMVolume(float bgmValue)
    {
        mixer.SetFloat("MyExposedParam", bgmValue);
        

    }
    //SE音量調節(現在はタイトルのみ反映)
    public void SEVolume(float volume)
    {
        mixer.SetFloat("SEVol", volume);
        float v = 0;
        bool flg = mixer.GetFloat("SEVol", out v);
        Debug.LogFormat("SEVolume {0} {1} {2}", v, flg, volume);
        
    }

    //武器の左右反転
    public void BukiChange()
    {
        if (buki_)
        {
            buki_ = false;
            buki.text = "鉈　　スパナ";
        }
        else
        {
            buki_ = true;
            buki.text = "スパナ　　鉈";
        }
    }




}