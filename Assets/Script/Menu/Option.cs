using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Option : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
   // [SerializeField] Text speed = null;
   // [SerializeField] Text buki = null;
  //  [SerializeField] int sp = 0;
    [SerializeField] AudioMixer mixer = null;
    //[SerializeField] Image button_lr = null;
    // [SerializeField] Image button_ud = null;
    [SerializeField] GameObject camera_rig = null;

    // 以下静的メンバ変数定義.
  //  static public bool eyes_lr = false;
  //  static public bool eyes_ud = false;

    // 以下メンバ変数定義.
  //  string te = "";
  //  bool buki_ = false;
    
    // 以下プロパティ.
 /*   public int Sp
    {
        get { return sp; }
    }
*/    


    void Start()
    {
        //  sp = 5;
        //  buki_ = true;
        //   eyes_lr = true;
        //  eyes_ud = true;


        
    }

    /*
    //ノーツスピード変更
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
    */

   /*
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
    */

    // BGM音量調節
    public void BGMVolume(float bgmValue)
    {
        mixer.SetFloat("MyExposedParam", bgmValue);
    }
    // SE音量調節
    public void SEVolume(float volume)
    {
        mixer.SetFloat("SEVol", volume);
    }

    /*
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
    */

    // カメラの位置変更
    public void XUp()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.x += 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }
    public void XDown()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.x -= 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }
    public void YUp()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.y += 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }
    public void YDown()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.y -= 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }
    public void ZUp()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.z += 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }
    public void ZDown()
    {
        Vector3 pos = camera_rig.transform.localPosition;
        pos.z -= 0.2f;
        camera_rig.transform.localPosition = pos;
        SetCamPosPrefs(pos);
    }

    // カメラの位置初期化
    public void CameraReset()
    {
        Vector3 pos = new Vector3(0, -3.59f, 2.57f); //ローカル？
        camera_rig.transform.localPosition = pos;

        SetCamPosPrefs(pos);


        //  PlayerPrefs.Save();
    }

    void SetCamPosPrefs(Vector3 pos)
    {
        PlayerPrefs.SetFloat("camera_pos_x", pos.x);
        PlayerPrefs.SetFloat("camera_pos_y", pos.y);
        PlayerPrefs.SetFloat("camera_pos_z", pos.z);
        PlayerPrefs.Save();
    }
}