using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Timers;

public class NoteBase : MonoBehaviour
{
    // 以下メンバ変数定義.
    float timing_       = 0.0f;
    float distance_     = 0.0f;
    float during_       = 0.0f;
    Vector3 first_pos_;
    Vector3 now_pos_;
    bool is_go_          = false;
    bool is_show_       = false;
//    bool is_start_pos_  = false;
//    bool is_die_ = false;
    float go_time_       = 0.0f;
    int pre_time_       = 500;
    Timer pre_timer_    = null;
    Vector3 start_pos_   = Vector3.zero;    
    float now_z_         = 0;
    float frame_         = 0;
    Animator note_ = null;
    AudioSource audio_source_ = null;
    int id_ = 0;
    System.Action miss_callback_ = null;
    bool is_miss_ = false;

    // 以下プロパティ定義.
    void StartNotes()
    {
        is_go_ = true;
    }

    public void SetParameter(float timing)
    {
        timing_ = timing;
    }

    public float GetTiming()
    {
        return timing_;
    }

    public System.Action MissCallback
    {
        set { miss_callback_ = value; }
    }
    

    void OnEnable()
    {
        is_go_ = false;
        first_pos_ = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        
        note_ = GetComponent<Animator>();
        audio_source_ = GetComponent<AudioSource>();


        
        // オブジェクト配置
        this.UpdateAsObservable()
            .Where(_ => is_go_)
            //.Where(_ => !is_die_)
            .Where(_ => 0 <= now_z_)
            .Subscribe(_ => {

                float time_ratio = (Time.time * 1000 - go_time_) / during_;
                float rot_y = this.gameObject.transform.localRotation.eulerAngles.y - 180;
                float radian2 = GetAim(new Vector2(-3, 0), new Vector2(this.transform.localPosition.z, this.transform.localPosition.x));
                
                rot_y = radian2;
                float radius = distance_ * time_ratio;
                double radian = rot_y * Math.PI / 180;
                var x = Math.Sin(radian) * radius;
                var z = Math.Cos(radian) * radius;
                
                this.gameObject.transform.localPosition = new Vector3(first_pos_.x - (float)x, first_pos_.y, first_pos_.z - (float)z);

                now_pos_ = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
                now_z_ = now_pos_.z;
                
            });


        // beat_pointでとどまらせる
        this.UpdateAsObservable()
            .Where(_ => now_z_ < 0)
            .Where(_=> frame_ <= 59)
            .Subscribe(_=> {
                this.gameObject.transform.localPosition = new Vector3(now_pos_.x, now_pos_.y, now_pos_.z);
                frame_++;
            });

        var cnt = 0;

        //「MISS」とログ、自身を消す
        this.UpdateAsObservable()
            .Where(_ => !is_miss_)
            .Where(_ => frame_ == 60)
            .Subscribe(_ =>
            {
                cnt++;
                Debug.LogFormat("cnt {0}", cnt);
                is_miss_ = true;
                this.gameObject.SetActive(false);
                Debug.Log("MISS");
                if (miss_callback_ != null) miss_callback_();
            });

        // 時間によりマテリアル変更
        this.UpdateAsObservable()
            .Where(_ => Time.time * 1000 - go_time_ > 0.7 * 1000)
            .Subscribe(_ =>
            {
                MaterialChange(Color.blue);
            });
        this.UpdateAsObservable()
            .Where(_ => Time.time * 1000 - go_time_ > 1.7 * 1000)
            .Subscribe(_ =>
            {
                MaterialChange(Color.red);
            });
        this.UpdateAsObservable()
            .Where(_ => Time.time * 1000 - go_time_ > 2.7 * 1000)
            .Subscribe(_ =>
            {
                MaterialChange(Color.white);
            });
    }
    
    // p2からp1への角度を求める
    // @param p1 自分の座標
    // @param p2 相手の座標
    // @return 2点の角度(Degree)
    public float GetAim(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
    
    // 値管理とタイマー開始
    public void Go(float distance, float during,int id)
    {
        id_ = id;

        distance_ = distance;
        during_ = during;
        go_time_ = Time.time * 1000 + pre_time_;

      //  Debug.LogFormat("id {0} ", id);

        pre_timer_ = new Timer(pre_time_);

        // Elapsedイベントにタイマー発生時の処理を設定する
        pre_timer_.Elapsed += (sender, e) =>
        {
            pre_timer_.Stop();
            StartNotes();
        };

        // タイマーを開始する
        pre_timer_.Start();

        Show();
        
    }


    // ノーツの初期地点とフラグ管理
    void Show()
    {
        is_show_ = true;
        start_pos_ = first_pos_;

        transform.localPosition = new Vector3(start_pos_.x, 6, start_pos_.z);
    }
    
    float dy = 0;
    float py = 0.1f;
    void Update()
    {
        // ノーツが降ってくる
        if (is_show_ )
        {
            dy += py;
            float y = transform.localPosition.y - dy;
            if (y < start_pos_.y)
            {
                y = start_pos_.y;
              //  is_start_pos_ = true;
            }
            transform.localPosition = new Vector3(start_pos_.x, y, start_pos_.z);
        }
    }

    // 倒されたら実行
    void OnHitBullet()
    {
        StartCoroutine(DaiAnim());
    }
    
    // 倒れるアニメーション＆音声再生、自身を破壊
    IEnumerator DaiAnim()
    {
        note_.SetBool("dai", true);
        audio_source_.Play();

      //  is_die_ = true;

        //1.0秒待つ
        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);
    }

    
    // マテリアルの色変更
    void MaterialChange(Color color)
    {
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
    
}
