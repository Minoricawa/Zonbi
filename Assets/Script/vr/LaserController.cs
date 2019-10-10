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
    // 以下公開メンバ変数定義.
    public float thickness = 0.002f;
    public float cursorSize = 0.04f;
    public Color laserColor = new Color(1, 1, 0);
    public SteamVR_Input_Sources HandType;

    // 以下メンバ変数定義.
    protected GameObject laser;
    

    void Start()
    {
        CreateLaserPointer();
    }

    void Update()
    {
        
    }

    //レーザーポインタを作成する
    protected void CreateLaserPointer()
    {
        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.SetParent(transform, false);
        laser.transform.localScale = new Vector3(thickness, thickness, 2.0f);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        laser.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(laser.GetComponent<BoxCollider>());
    }

    //レーザーの長さを調整
    public void AdjustLaserDistance(float distance)
    {
        if (laser == null) { return; }
        distance += 0.01f;
        
        laser.transform.localScale = new Vector3(thickness, thickness, distance);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, distance * 0.5f);
    }
    
}
