using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Option : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] GameObject camera_rig = null;
    [SerializeField] Slider bgm_slider = null;
    [SerializeField] Slider se_slider = null;


    void Start()
    {
        SetVolume();
    }
    

    // BGM音量調節
    public void BGMVolume(float volume)
    {
        mixer.SetFloat("BGMVol", volume);
        SetBGMVolPrefs(volume);
    }
    // SE音量調節
    public void SEVolume(float volume)
    {
        mixer.SetFloat("SEVol", volume);
        SetSEVolPrefs(volume);
    }

    // 音量初期化
    public void VolumeReset()
    {
        bgm_slider.value = 0;
        mixer.SetFloat("BGMVol", 0);
        SetBGMVolPrefs(0);

        se_slider.value = 0;
        mixer.SetFloat("SEVol", 0);
        SetSEVolPrefs(0);
    }

    // BGM音量セーブ
    public void SetBGMVolPrefs(float bgm)
    {
        bgm_slider.value = bgm;
        PlayerPrefs.SetFloat("bgm_volume", bgm);
        PlayerPrefs.Save();
    }
    // SE音量セーブ
    public void SetSEVolPrefs(float se)
    {
        se_slider.value = se;
        PlayerPrefs.SetFloat("se_volume", se);
        PlayerPrefs.Save();
    }
    
    // 各音量を設定
    void SetVolume()
    {
        // 音量調整のデータがあれば
        if (PlayerPrefs.GetFloat("bgm_volume") != 0 || PlayerPrefs.GetFloat("se_volume") != 0)
        {
            bgm_slider.value = PlayerPrefs.GetFloat("bgm_volume");
            mixer.SetFloat("BGMVol", bgm_slider.value);
            se_slider.value = PlayerPrefs.GetFloat("se_volume");
            mixer.SetFloat("SEVol", se_slider.value);
        }
    }


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
        Vector3 pos = new Vector3(0, -3.59f, 2.57f);
        camera_rig.transform.localPosition = pos;

        SetCamPosPrefs(pos);
    }

    // カメラの位置セーブ
    void SetCamPosPrefs(Vector3 pos)
    {
        PlayerPrefs.SetFloat("camera_pos_x", pos.x);
        PlayerPrefs.SetFloat("camera_pos_y", pos.y);
        PlayerPrefs.SetFloat("camera_pos_z", pos.z);
        PlayerPrefs.Save();
    }

}