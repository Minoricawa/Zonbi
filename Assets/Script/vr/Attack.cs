using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.XR;


public class Attack : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] AudioClip se = null;

    // 以下メンバ変数定義.
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
