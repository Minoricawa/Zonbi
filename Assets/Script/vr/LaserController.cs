using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.XR;


public class LaserController : MonoBehaviour
{
    
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] AudioClip se = null;
   
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
    int start_cnt = 0;

    // 以下プロパティ.
    public GameObject HitNote
    {
        get { return hit_note; }
    }
    public System.Action HitCallback
    {
        get { return hit_callback; }
        set { hit_callback = value; }
    }



    void Start()
    {
        start_cnt = 60;
        ve = GetComponent<VelocityEstimator>();
        audio_source = GetComponent<AudioSource>();
        

        //レーザーポインタを作成する
        CreateLaserPointer();
    }
    

    protected void CreateLaserPointer()
    {

        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.SetParent(transform, false);
        laser.transform.localScale = new Vector3(thickness, thickness, 2.0f);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        laser.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(laser.GetComponent<BoxCollider>());
    }

    public void AdjustLaserDistance(float distance)
    {

        if (laser == null) { return; }
        distance += 0.01f;

        //レーザーの長さを調整
        laser.transform.localScale = new Vector3(thickness, thickness, distance);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, distance * 0.5f);
    }



    void Update()
    {

        start_cnt--;
        if (start_cnt > 0) return;//最初の１秒は音を鳴らさない


        
        speed = ve.GetVelocityEstimate().magnitude;
        if (ve.GetAccelerationEstimate().magnitude > 400)
        {
            Swing();
        }
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
        if (audio_source.isPlaying) return;
        audio_source.PlayOneShot(se);
        
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        XRDevice.DisableAutoXRCameraTracking(cam, false);
        cam.gameObject.transform.localPosition = Vector3.zero;
        cam.gameObject.transform.localRotation = Quaternion.identity;

    }


    
}
