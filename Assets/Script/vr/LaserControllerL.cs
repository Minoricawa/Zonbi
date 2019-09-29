using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.XR;


public class LaserControllerL : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] AudioClip se = null;
 //   [SerializeField] GameObject paneru_l = null;
    [SerializeField] LaserControllerR laser_controller_r = null;
    [SerializeField] PopUp popup = null;

    // 以下公開メンバ変数定義.
    public float thickness = 0.002f;
    public float cursorSize = 0.04f;
    public Color laserColor = new Color(1, 1, 0);
    public SteamVR_Input_Sources HandType;

    // 以下メンバ変数定義.
    protected GameObject laser;
    protected GameObject cursor;
    GameObject hit_note = null;
    VelocityEstimator ve = null;
    AudioSource audio_source = null;
    float speed = 0;
    System.Action hit_callback = null;
    System.Action paneru_open_callback = null;
    System.Action paneru_close_callback = null;
    bool paneru_open_l = false;


    // 以下プロパティ.
    public System.Action HitCallback
    {
        get { return hit_callback; }
        set { hit_callback = value; }
    }
    public GameObject HitNote
    {
        get { return hit_note; }
    }
    public System.Action PaneruOpenCallback
    {
        get { return paneru_open_callback; }
        set { paneru_open_callback = value; }
    }
    public System.Action PaneruCloseCallback
    {
        get { return paneru_close_callback; }
        set { paneru_close_callback = value; }
    }
    public bool PaneruOpenL
    {
        get { return paneru_open_l; }
    }



    void Start()
    {
        ve = GetComponent<VelocityEstimator>();
        audio_source = GetComponent<AudioSource>();

     //   paneru_l.SetActive(false);
        paneru_open_l = false;

        //レーザーポインタを作成する
        CreateLaserPointer();
    }

    //------------------------------------------------------------------------------------------------------------------------------//
    protected void CreateLaserPointer()
    {

        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.SetParent(transform, false);
        laser.transform.localScale = new Vector3(thickness, thickness, 2.0f);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        laser.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(laser.GetComponent<BoxCollider>());

        
        cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cursor.transform.SetParent(transform, false);
        cursor.transform.localScale = new Vector3(cursorSize, cursorSize, cursorSize);
        cursor.transform.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
        cursor.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(cursor.GetComponent<SphereCollider>());
        
    }

    public void AdjustLaserDistance(float distance)
    {

        if (laser == null) { return; }
        distance += 0.01f;

        //レーザーの長さを調整
        laser.transform.localScale = new Vector3(thickness, thickness, distance);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, distance * 0.5f);
           cursor.transform.localPosition = new Vector3(0.0f, 0.0f, distance);
    }



    void Update()
    {
        // メニューボタンを押すとメニュー開閉
        if (GameInfo.NowGameStatus != GameInfo.GameStatus.GamgeOver && !laser_controller_r.PaneruOpenR)
        {
            if (!paneru_open_l && SteamVR_Actions.default_Teleport.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                PushPause();
            }

            else if (paneru_open_l && SteamVR_Actions.default_Teleport.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                ClosePause();
            }
        }




        //speed = ve.GetVelocityEstimate().magnitude;
        speed = ve.GetVelocityEstimate().magnitude;
        if (ve.GetAccelerationEstimate().magnitude > 400)
        {
            Swing();
        }
        //Debug.LogFormat("speed {0}", speed);
        //Debug.LogFormat("GetAccelerationEstimate {0}", ve.GetAccelerationEstimate().magnitude);
    }

    // ノーツに触れたときHit
    void OnTriggerEnter(Collider other)
    {
        if (ve.GetAccelerationEstimate().magnitude < 300) return;
        if (hit_note == other.gameObject) return;
        if (other.tag != "enemy") return;
        hit_note = other.gameObject;
        if (hit_callback != null) hit_callback();
    }

    // 音を鳴らす
    void Swing()
    {
      //  Debug.LogFormat("Swing1");
        if (audio_source.isPlaying) return;
      //  Debug.LogFormat("Swing2");
        audio_source.PlayOneShot(se);

        //sXRSettings.enabled = false;
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        XRDevice.DisableAutoXRCameraTracking(cam, false);
        cam.gameObject.transform.localPosition = Vector3.zero;
        cam.gameObject.transform.localRotation = Quaternion.identity;

    }


    // ポーズ画面開閉
    public void PushPause()
    {
        /*
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause) return;
        paneru_l.SetActive(true);
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Play && select_paneru != null)
        {
            NotesContoller notes_controller = select_paneru.GetComponent<NotesContoller>();
            notes_controller.Pause();
            Time.timeScale = 0;
        }
        GameInfo.NowGameStatus = GameInfo.GameStatus.Pause;
        */

        if (paneru_open_l) return;
        paneru_open_l = true;
        //  paneru_l.SetActive(true);
        popup.Open();
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Play) GameInfo.NowGameStatus = GameInfo.GameStatus.Pause;
        if (paneru_open_callback != null) paneru_open_callback();
    }
    public void ClosePause()
    {
        /*
        paneru_l.SetActive(false);
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause && select_paneru != null)
        {
            NotesContoller notes_controller = select_paneru.GetComponent<NotesContoller>();
            notes_controller.Resume();
            Time.timeScale = 1;
        }
        GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
        */

        paneru_open_l = false;
        //  paneru_l.SetActive(false);
        popup.Close();
        if (paneru_close_callback != null) paneru_close_callback();
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause) GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
    }
}
