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
    static string log = null;
    static int combo = 0;

    // 以下メンバ変数定義.
    int id_;
    string title = null;
    // int BPM = 0;
    float end = 0;
    List<GameObject> notes;
    float distance = 0.0f;
    float during = 0.0f;
    float play_time = 0.0f;
    int go_index = 0;
    bool is_playing = false;    
    AudioSource music;    
    List<float> note_timings = new List<float>();
    float check_range = 0;
    float beat_range = 0;


    // 以下プロパティ.
   public bool IsPlaying
    {
        get { return is_playing; }
    } 

   public static int Combo
    {
        get { return combo; }
        set { combo = value; }
    }

   public static String TimingLog
    {
        get { return log; }
        set { log = value; }
    } 

    void OnEnable()
    {

        music = this.GetComponent<AudioSource>();

        distance = Mathf.Abs(beat_point.position.z - spawn_point.position.z); // 距離
        distance = Mathf.Abs(beat_point.position.z - spawn_point.position.z); // 距離
        during = 3 * 1000;  // かかる時間
        is_playing = false;
        go_index = 0;
        check_range = 110;
        beat_range = 80;


        // ノーツを出現
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => notes.Count > go_index)
            .Where(_ => notes[go_index].GetComponent<NoteBase>().GetTiming() <= ((Time.time * 1000 - (play_time - 500)) + during))
            .Subscribe(_ => {
                NotesShow(notes[go_index]);
                notes[go_index].GetComponent<NoteBase>().Go(distance, during, go_index);
                go_index++;
            });

        // 曲が終わったらパネル復活
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Time.time * 1000 - play_time >= end)
            .Subscribe(_ =>
            {
                if (is_playing)
                {
                    is_playing = false;
                    this.GetComponent<Select>().OpenPaneruList();
                    this.GetComponent<Select>().GameUISet();
                }
            });

        // プレイ中、ボタンを押したタイムを見る
        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => {
                Beat(Time.time * 1000 - play_time);
            });

        this.UpdateAsObservable()
            .Where(_ => is_playing)
            .Where(_ => Input.GetMouseButtonDown(1))
            .Subscribe(_ => {
                Beat(Time.time * 1000 - play_time);
            });
    }

    

    // jsonからデータを拝借、ノーツを生成しリストへ
    void LoadChart(int id)
    {
        go_index = 0;
        notes = new List<GameObject>();

        string jsonText = Resources.Load<TextAsset>(bgm_filePath_list[id]).ToString();
        music.clip = (AudioClip)Resources.Load(bgm_clipPath_list[id]);

        JsonNode json = JsonNode.Parse(jsonText);
        title = json["title"].Get<string>();
        //    BPM = int.Parse(json["BPM"].Get<string>());
        end = float.Parse(json["end"].Get<string>());

        note_timings = new List<float>();

        System.Random r = new System.Random();
        foreach (var note in json["notes"])
        {
            float timing = float.Parse(note["timing"].Get<string>());
            GameObject noteNew;
            float xrandam = (float)r.Next(-20, 20) / 10; // ブレ幅
            noteNew = Instantiate(zombie_notes, new Vector3(spawn_point.position.x + xrandam, spawn_point.position.y, spawn_point.position.z), Quaternion.Euler(0, 180, 0));
            SetLean(noteNew, note["lean"].Get<string>());
            noteNew.GetComponent<NoteBase>().SetParameter(timing);

            notes.Add(noteNew);

            NotesHide(noteNew);

            note_timings.Add(timing);
        }

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


    // 開始フラグを立て、値を管理。曲再生。
    public void Play(int id)
    {
        LoadChart(id);
        music.Stop();
        music.Play();
        play_time = Time.time * 1000;
        is_playing = true;
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
                notes[minDiffIndex].SendMessage("OnHitBullet");  // NotesBase.OnHitBullet();
                Debug.Log("GOOD");
                TimingLog = "good";
                Combo++;
            }
            else
            {
                note_timings[minDiffIndex] = -1;
                notes[minDiffIndex].SendMessage("OnHitBullet");
                Debug.Log("BAD");
                TimingLog = "bad";
                Combo = 0;
            }
        }
    }

}