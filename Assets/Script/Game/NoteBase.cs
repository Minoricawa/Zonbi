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
    bool is_die_ = false;
    float go_time_       = 0.0f;
    int pre_time_       = 500;
    Timer pre_timer_    = null;
    Vector3 start_pos_   = Vector3.zero;
    Animator note_ = null;
    AudioSource audio_source_ = null;
    int id_ = 0;
    System.Action miss_callback_ = null;
    Vector3 target_point_;


    // 以下プロパティ定義.
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
    
    void StartNotes()
    {
        is_go_ = true;
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
            .Where(_ => !is_die_)
            .Subscribe(_ => {

                // 時間の割合
                float time_ratio = (Time.time * 1000 - go_time_) / during_;
                // 残り時間（ミリ秒）
                float remaining_time = during_ - Time.time * 1000 - go_time_;

                // 角度
                float radian2 = GetAim(target_point_, new Vector2(this.transform.localPosition.z, this.transform.localPosition.x));
                if (time_ratio < 0.1)
                {
                    this.gameObject.transform.localRotation = Quaternion.Euler(0, radian2 - 180, 0);
                }

                // 位置
                Vector3 move = Vector3.Lerp(start_pos_, target_point_, time_ratio * 0.9f);
                if (time_ratio < 1.15f)
                {
                    this.gameObject.transform.localPosition = move;
                }

                // 「MISS」とログ、自身を消す
                if (time_ratio > 1.20f)
                {
                    if (miss_callback_ != null) miss_callback_();
                    this.gameObject.SetActive(false);
                }

                // 時間によりマテリアル変更
                if (time_ratio > 0.95f && time_ratio < 1.10f)
                {
                    MaterialChange(Color.white);
                }
                else if (time_ratio > 0.74f && time_ratio < 1.0f)
                {
                    MaterialChange(Color.red);
                }
                else if (time_ratio > 0.58f && time_ratio < 1.0f)
                {
                    MaterialChange(Color.blue);
                }
                else if (time_ratio > 1.10f)
                {
                    MaterialChange(Color.clear);
                }
                
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
    public void Go(float distance, float during,int id, Vector3 target)
    {
        target_point_ = target;

        id_ = id;

        distance_ = distance;
        during_ = during;
        go_time_ = Time.time * 1000 + pre_time_;
        
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
        start_pos_ = this.gameObject.transform.localPosition;

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
                is_show_ = false;
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
        is_die_ = true;
        note_.SetBool("dai", true);
        audio_source_.Play();
    
        //1.0秒待つ
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }

    
    // マテリアルの色変更
    public void MaterialChange(Color color)
    {
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
    
}
