using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] List<Paneru> sound_paneru_list = new List<Paneru>();    // パネルの配列
    [SerializeField] List<AudioSource> audio_list = new List<AudioSource>();    // サウンドの配列
    [SerializeField] Camera cam = null;
    [SerializeField] Canvas canvas = null;
    [SerializeField] GameObject paneru_container = null;
    [SerializeField] GameObject game_ui = null;

    // 以下メンバ変数定義.
    int max_rot = 180;  // パネルを配置する最大角度
    int sound_length;   // 曲数（パネル数）
    int active_rot = 30;    // カメラの角度とパネルの角度に対してアクティブにする範囲
    int distance = 40;
    int paneru_y = 4;
    float prev_cam_rot_y = 0;
    System.Action<int> set_paneru_callback = null;   // 決定した時に呼ぶ

    // 以下プロパティ.
    public System.Action<int> SetPaneruCallback
    {
        set { set_paneru_callback = value; }
    }

    public string GetMusicTitle(int id)
    {
        Paneru paneru = sound_paneru_list[id];
        if (paneru != null)
        {
            return paneru.TitleName;
        }
        return "";
    }

    void Start()
    {
        sound_length = sound_paneru_list.Count; 
        SetPosition();
        SetData();
        game_ui.SetActive(false);
    }

    private void Update() // 毎フレーム実行
    {
        // プレイ中は処理しない
        if (canvas.GetComponent<NotesContoller>().IsPlaying == true)
        {
            return;
        }
        
        float cam_rot_y = cam.gameObject.transform.rotation.eulerAngles.y + 90;
        float cam_rot_x = cam.gameObject.transform.rotation.eulerAngles.x;
        cam_rot_y = cam_rot_y % 360;
       

        // 前フレームとカメラの角度が変わってない場合は処理しない
        if (cam_rot_y == prev_cam_rot_y)
        {
            return;
        }

        
        // 出現させる範囲設定
        int rot = max_rot / sound_length;  // 60;
        for (var i = 0; i < sound_length; i++)
        {
            Paneru paneru = sound_paneru_list[i];
            CanvasGroup canvas = paneru.GetComponentInChildren<CanvasGroup>();

            int trot = rot * (i) + 30;   // 45 - 75
            float minrot = trot - active_rot / 2;
            float maxrot = trot + active_rot / 2;
            

            // カメラの向きにより出現
            if (minrot < cam_rot_y && maxrot > cam_rot_y && (cam_rot_x > 360 - 30 || cam_rot_x < 30))
            {
                paneru.FadeIn();
                SetActiveSound(i, true);
            }
            else
            {
                paneru.FadeOut(0.2f);
                SetActiveSound(i, false);
            }
        }

        prev_cam_rot_y = cam_rot_y;

    }



    // パネルを扇状に配置
    void SetPosition()
    {
        int rot = max_rot / sound_paneru_list.Count;  // 60;
        for (var i = 0; i < sound_paneru_list.Count; i++)
        {
            GameObject paneru = sound_paneru_list[i].gameObject;


            float x = Mathf.Cos((-rot * i + 150) * Mathf.PI / 180) * distance - 57;
            float z = Mathf.Sin((-rot * i + 150) * Mathf.PI / 180) * distance - 220;


            paneru.transform.localPosition = new Vector3(x, paneru_y, z);

            paneru.transform.localRotation = Quaternion.Euler(0, rot * i - 60, 0);
            
        }
    }
    

    // パネルにID付与
    void SetData()
    {
        for (var i = 0; i < sound_paneru_list.Count; i++)
        {
            Paneru paneru = sound_paneru_list[i];
            paneru.id = i;
            paneru.click_callback = ClickPaneru;
        }
    }

    // クリックしたパネルのidをプレイ、ゲームUI表示
    public void ClickPaneru(int id)
    {
        HidePaneruList();
        if (set_paneru_callback != null) set_paneru_callback(id);
        
    }
    


    // すべての曲パネルを消す
    public void HidePaneruList()
    {
        paneru_container.SetActive(false);
    }

    // すべての曲パネルを出す
    public void OpenPaneruList()
    {
        paneru_container.SetActive(true);
    }

  

    // パネルがアクティブかつプレイ開始していなければ、任意のaudioリストを再生
    void SetActiveSound(int index, bool active)
    {
        GameObject sound_paneru = sound_paneru_list[index].gameObject;
        

        AudioSource audio = audio_list[index];
        if (active == true)
        {
            if (audio.isPlaying != true)
            {
                audio.Play();
            }

        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }
    }
    
    // ゲームUI非表示
    public void GameUISet()
    {
        game_ui.SetActive(false);
      //  canvas.GetComponent<NotesContoller>.Combo = 0;
    }

}
