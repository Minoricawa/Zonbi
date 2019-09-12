using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class NotesContoller : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] List<string> bgm_filePath_list = new List<string>();
    [SerializeField] List<string> bgm_clipPath_list = new List<string>();
    [SerializeField] GameObject zombie_notes = null;
    [SerializeField] Transform spawn_point = null;
    [SerializeField] Transform beat_point = null;
    [SerializeField] Transform lean_center = null;
    [SerializeField] Transform lean_left = null;
    [SerializeField] Transform lean_right = null;

    // 以下静的メンバ変数定義.
    // static string log = null;

    // 以下メンバ変数定義.
    int id_ = 0;
    string title = null;
    // int BPM = 0;
    float end = 0;
    List<GameObject> notes_list;
    float distance = 0.0f;
    float during = 0.0f;
    float play_time = 0.0f;
    int go_index = 0;
    bool is_playing = false;    
    AudioSource music;    
    List<float> note_timings = new List<float>();
    float check_range = 0;
    float beat_range = 0;
    System.Action miss_callback_ = null;
    System.Action<string> timing_callback_ = null;
    System.Action good_callback_ = null;
    System.Action bad_callback_ = null;
    System.Action music_finish_callback_ = null;
    JsonNode music_list_json = null;
    int combo = 0;
    int max_combo = 0;
    int score = 0;


    // 以下プロパティ.
    public bool IsPlaying
    {
        get { return is_playing; }
    } 

    public AudioSource Music
    {
        get { return music; }
    }

    public int Combo
    {
        get { return combo; }
    }

    public int MaxCombo
    {
        get { return max_combo; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }
    
    public System.Action MissCallback
    {
        set { miss_callback_ = value; }
    }

    public System.Action<string> TimingCallback
    {
        set { timing_callback_ = value; }
    }

    public System.Action GoodCallback
    {
        set { good_callback_ = value; }
    }

    public System.Action BadCallback
    {
        set { bad_callback_ = value; }
    }

    public System.Action MusicFinishCallback
    {
        set { music_finish_callback_ = value; }
    }
    
    public void Pause()
    {
        music.Pause();
    }

    public void Resume()
    {
        music.Play();
    }

    void OnEnable()
    {

        LoadMusicListJson();

        music = this.GetComponent<AudioSource>();

        distance = Mathf.Abs(beat_point.localPosition.z - spawn_point.localPosition.z); // 距離
        distance = Mathf.Abs(beat_point.localPosition.z - spawn_point.localPosition.z); // 距離
        during = 3 * 1000;  // かかる時間
        is_playing = false;
        go_index = 0;
        check_range = 110;
        beat_range = 80;


        // ノーツを出現
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => notes_list.Count > go_index)
            .Where(_ => notes_list[go_index].GetComponent<NoteBase>().GetTiming() <= ((Time.time * 1000 - (play_time - 500)) + during))
            .Where(_ => GameInfo.NowGameStatus == GameInfo.GameStatus.Play)
            .Subscribe(_ => {
                NotesShow(notes_list[go_index]);
                notes_list[go_index].GetComponent<NoteBase>().Go(distance, during, go_index);
                go_index++;
            });

        // 曲が終わったらパネル表示のコールバック起動
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Time.time * 1000 - play_time >= end)
            .Where(_ => GameInfo.NowGameStatus == GameInfo.GameStatus.Play)
            .Subscribe(_ =>
            {
                is_playing = false;
                if (music_finish_callback_ != null)
                {
                    music_finish_callback_();
                }
            });

        // プレイ中、ボタンを押したタイムを見る
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Where(_ => GameInfo.NowGameStatus==GameInfo.GameStatus.Play)
            .Subscribe(_ => {
                Beat(Time.time * 1000 - play_time);
            });

        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Input.GetMouseButtonDown(1))
            .Where(_ => GameInfo.NowGameStatus == GameInfo.GameStatus.Play)
            .Subscribe(_ => {
                Beat(Time.time * 1000 - play_time);
            });
    }
    
    // 曲のリストのjsonをロード
    void LoadMusicListJson()
    {
        string json_text = Resources.Load<TextAsset>("Json/music_list").ToString();
        music_list_json = JsonNode.Parse(json_text);
    }


    // jsonからデータを拝借、ノーツを生成しリストへ
    void LoadChart(int id)
    {
        string json_file = "";
        string audio_file = "";

        foreach (var music in music_list_json["music_list"])
        {
            if (id == int.Parse(music["id"].Get<string>()))
            {
                json_file = music["json_file"].Get<string>();
                audio_file = music["audio_file"].Get<string>();
            }
        }

        go_index = 0;
        notes_list = new List<GameObject>();
        
        music.clip = (AudioClip)Resources.Load(audio_file);
        string json_text = Resources.Load<TextAsset>(json_file).ToString();
        JsonNode json = JsonNode.Parse(json_text);
        title = json["title"].Get<string>();
        //    BPM = int.Parse(json["BPM"].Get<string>());
        end = float.Parse(json["end"].Get<string>());

        note_timings = new List<float>();

        System.Random r = new System.Random();
        foreach (var note in json["notes"])
        {
            float timing = float.Parse(note["timing"].Get<string>());
            GameObject notes;
            float xrandam = (float)r.Next(-20, 20) / 10; // ブレ幅
            notes = Instantiate(zombie_notes, new Vector3(spawn_point.localPosition.x + xrandam, spawn_point.localPosition.y, spawn_point.localPosition.z), Quaternion.Euler(0, 180, 0));
            SetLean(notes, note["lean"].Get<string>());
            notes.GetComponent<NoteBase>().SetParameter(timing);
            notes.GetComponent<NoteBase>().MissCallback = OnMiss;

            notes_list.Add(notes);

            NotesHide(notes);

            note_timings.Add(timing);
        }

    }

    //　ノーツ削除
    void RemoveNotes()
    {
        if (notes_list == null) return;
        foreach(GameObject notes in notes_list)
        {
            notes.transform.SetParent(null);
            Destroy(notes);
        }
    }

    // ミスをした場合の処理
    void OnMiss()
    {
        combo = 0;
        Debug.Log("NotesContoller OnMiss");
        if(miss_callback_ != null) miss_callback_();
        if (timing_callback_ != null) timing_callback_("miss");
    }

    // Goodの場合の処理
    void OnGood()
    {
        combo++;
        if (max_combo < combo)
        {
            max_combo = combo;
        }
        score += 1000;
        if (timing_callback_ != null) timing_callback_("good");
        if (good_callback_ != null) good_callback_();
    }

    // Badの場合の処理
    void OnBad()
    {
        score += 100;
        if (timing_callback_ != null) timing_callback_("bad");
        if (bad_callback_ != null) bad_callback_();
    }
    
    // ノーツのレーン分け
    void SetLean(GameObject note, string lean)
    {
        switch (lean)
        {
            case "c":
                note.transform.SetParent(lean_center, false);
                break;
            case "l":
                note.transform.SetParent(lean_left, false);
                break;
            case "r":
                note.transform.SetParent(lean_right, false);
                break;
            default:
                note.transform.SetParent(lean_center, false);
                break;
        }
    }

    // リプレイ
    public void Replay()
    {
        RemoveNotes();
        Play(id_);
    }

    // 開始フラグを立て、値を管理。曲再生。
    public void Play(int id)
    {
        id_ = id;
        LoadChart(id);
        music.Stop();
        music.Play();
        play_time = Time.time * 1000;
        is_playing = true;
        max_combo = 0;
        combo = 0;
        score = 0;
        Time.timeScale = 1;
    }
    
    // ノーツ登場
    void NotesShow(GameObject notes)
    {
        notes.transform.localPosition = new Vector3(notes.transform.localPosition.x, 0, notes.transform.localPosition.z);
        Animator anim = notes.GetComponent<Animator>();
        anim.SetBool("advent", true);
    }

    // ノーツを見えないところに溜める
    void NotesHide(GameObject notes)
    {
        notes.transform.localPosition = new Vector3(notes.transform.localPosition.x, -1000, notes.transform.localPosition.z);
    }
    
    // timingと一番近いタイミングのノーツを探す
    void Beat(float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        for (int i = 0; i < note_timings.Count; i++)
        {
            if (note_timings[i] > 0)
            {
                float diff = Math.Abs(note_timings[i] - timing);

                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if (minDiff != -1 & minDiff < check_range)
        {
            if (minDiff < beat_range)
            {
                note_timings[minDiffIndex] = -1;
                notes_list[minDiffIndex].SendMessage("OnHitBullet");  // NotesBase.OnHitBullet();
                Debug.Log("GOOD");
                OnGood();
            }
            else
            {
                note_timings[minDiffIndex] = -1;
                notes_list[minDiffIndex].SendMessage("OnHitBullet");
                Debug.Log("BAD");
                OnBad();
            }
        }
    }

}