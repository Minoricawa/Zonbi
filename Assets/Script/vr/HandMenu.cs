﻿using UnityEngine;
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

    // 以下メンバ変数定義(SerializeField).
    [SerializeField] PopUp popup = null;

    // 以下公開メンバ変数定義.
    public SteamVR_Input_Sources HandType;


   
    void Start()
    {

    }


    void Update()
    {
        // メニューボタンを押すとメニュー開閉
        if (GameInfo.NowGameStatus != GameInfo.GameStatus.GamgeOver)
        {
            if (SteamVR_Actions.default_Teleport.GetStateDown(HandType))//ボタンClick
            {
                if (HandMenuState == HandMenuStateList.Hidden)//何も開いてないとき
                {
                    OpenMenu();
                }
                else if(HandMenuState == HandMenuStateList.LeftOpen && HandType == SteamVR_Input_Sources.LeftHand)// 左のメニューが開いていて、かつ自分が左手だったら
                {
                    CloseMenu();
                }
                else if (HandMenuState == HandMenuStateList.RightOpen && HandType == SteamVR_Input_Sources.RightHand)// 右のメニューが開いていて、かつ自分が右手だったら
                {
                    CloseMenu();
                }
            }
        }
    }

    // ポーズ画面開閉
    public void OpenMenu()
    {
        // ステータス更新
        if(HandType== SteamVR_Input_Sources.LeftHand)
        {
            HandMenuState = HandMenuStateList.LeftOpen;
        }
        else
        {
            HandMenuState = HandMenuStateList.RightOpen;
        }
        
        popup.Open();
    }
    public void CloseMenu()
    {
        // ステータス更新
        HandMenuState = HandMenuStateList.Hidden;

        popup.Close();
    }
}
