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
    


    void Start()
    {
        
    }


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