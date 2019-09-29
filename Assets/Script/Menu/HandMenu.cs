using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.XR;


public class HandMenu : MonoBehaviour
{
    public static HandMenuStateList HandMenuState = HandMenuStateList.Hidden;
    public enum HandMenuStateList
    {
        LeftOpen,
        RightOpen,
        Hidden
    }


    [SerializeField] PopUp popup = null;
    bool paneru_open_l = false;
    System.Action paneru_open_callback = null;
    System.Action paneru_close_callback = null;

    public SteamVR_Input_Sources HandType;

    // 以下プロパティ.
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

    private void Start()
    {
        paneru_open_l = false;
    }


    private void Update()
    {
        // メニューボタンを押すとメニュー開閉
        if (GameInfo.NowGameStatus != GameInfo.GameStatus.GamgeOver)
        {
            if (SteamVR_Actions.default_Teleport.GetStateDown(HandType))//ボタンClick
            {
                if (HandMenuState == HandMenuStateList.Hidden)//何も開いてないとき
                {
                    OpenMenu();
                    
                } else if(HandMenuState == HandMenuStateList.LeftOpen && HandType == SteamVR_Input_Sources.LeftHand)// 左のメニューが開いていて、かつ自分が左手だったら
                {
                    CloseMenu();
                }
                else if (HandMenuState == HandMenuStateList.RightOpen && HandType == SteamVR_Input_Sources.RightHand)// 左のメニューが開いていて、かつ自分が左手だったら
                {
                    CloseMenu();
                }
                
            }
            
        }
    }

    // ポーズ画面開閉
    public void OpenMenu()
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

        // ステータス更新
        if(HandType== SteamVR_Input_Sources.LeftHand)
        {
            HandMenuState = HandMenuStateList.LeftOpen;
        } else
        {
            HandMenuState = HandMenuStateList.RightOpen;
        }

        //  paneru_l.SetActive(true);
        popup.Open();
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Play) GameInfo.NowGameStatus = GameInfo.GameStatus.Pause;
        if (paneru_open_callback != null) paneru_open_callback();
    }
    public void CloseMenu()
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
        // ステータス更新
        HandMenuState = HandMenuStateList.Hidden;

        popup.Close();
        if (paneru_close_callback != null) paneru_close_callback();
        if (GameInfo.NowGameStatus == GameInfo.GameStatus.Pause) GameInfo.NowGameStatus = GameInfo.GameStatus.Play;
    }
}
