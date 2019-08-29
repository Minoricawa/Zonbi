using UnityEngine;
using Valve.VR;

public class GrabTest : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Input_Sources HandType;

    void Update()
    {
        // GrabGrip
        if (SteamVR_Actions.default_GrabGrip.GetStateUp(HandType))
        {
            Debug.Log("GrabGrip.GetStateUp");
        }

        

        // HeadsetOnHead
        if (SteamVR_Actions.default_HeadsetOnHead.GetStateUp(HandType))
        {
            Debug.Log("default_HeadsetOnHead.GetStateUp");
        }

        // GrabPinch
        if (SteamVR_Actions.default_GrabPinch.GetStateUp(HandType))
        {
            Debug.Log("default_GrabPinch.GetStateUp");
        }

        // InteractUI
        if (SteamVR_Actions.default_InteractUI.GetStateUp(HandType))
        {
            Debug.Log("default_InteractUI.GetStateUp");
        }

        // Teleportボタンが離された
        if (SteamVR_Actions.default_Teleport.GetStateUp(HandType))
        {
            Debug.Log("default_Teleport.GetStateUp");
        }

        // Teleportボタンが押された
        if (SteamVR_Actions.default_Teleport.GetStateDown(HandType))
        {
            Debug.Log("default_Teleport.GetStateDown");
        }


    }
}