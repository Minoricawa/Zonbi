using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class LaserController : MonoBehaviour
{
    // 以下メンバ変数定義.
    protected GameObject laser;
    protected GameObject cursor;

    public float thickness = 0.002f;
    public float cursorSize = 0.04f;
    public Color laserColor = new Color(1, 1, 0);
    public SteamVR_Input_Sources HandType;
    public System.Action hit_callback;

    GameObject hit_note = null;


    // 以下プロパティ.
    public System.Action HitCallback
    {
        get { return hit_callback; }
        set { hit_callback = value; }
    }

    public GameObject HitNote
    {
        get { return hit_note; }
    }

    void Start()
    {

        //レーザーポインタを作成する
        CreateLaserPointer();
    }

    //------------------------------------------------------------------------------------------------------------------------------//
    protected void CreateLaserPointer()
    {

        laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.SetParent(transform, false);
        laser.transform.localScale = new Vector3(thickness, thickness, 2.0f);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        laser.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(laser.GetComponent<BoxCollider>());

        cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cursor.transform.SetParent(transform, false);
        cursor.transform.localScale = new Vector3(cursorSize, cursorSize, cursorSize);
        cursor.transform.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
        cursor.GetComponent<MeshRenderer>().material.color = laserColor;
        Object.DestroyImmediate(cursor.GetComponent<SphereCollider>());
    }

    public void AdjustLaserDistance(float distance)
    {

        if (laser == null) { return; }
        distance += 0.01f;

        //レーザーの長さを調整
        laser.transform.localScale = new Vector3(thickness, thickness, distance);
        laser.transform.localPosition = new Vector3(0.0f, 0.0f, distance * 0.5f);
        cursor.transform.localPosition = new Vector3(0.0f, 0.0f, distance);
    }

    void Update()
    {
        //LaserInputModule.test = SteamVR_Actions.default_Teleport.GetStateDown(HandType);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hit_note == other) return;
        if (other.tag != "enemy") return;
        hit_note = other.gameObject;
        if (hit_callback != null) hit_callback();
        

    }
}

