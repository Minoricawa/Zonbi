using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;


public class LaserInputModule : BaseInputModule
{
    
    [SerializeField] private Camera UICamera;
    [SerializeField] private GameObject CameraRig;

    private LaserController[] lasers;

    private GameObject[] hitObjects;
    private GameObject[] pressedObjects;
    private GameObject[] dragObjects;
    private PointerEventData[] pointEvents;

    public SteamVR_Input_Sources HandType;

    //------------------------------------------------------------------------------------------------------------------------------------------//
    protected override void Start()
    {

        base.Start();

        SetUICamera();


        // ヒエラルキーからコントローラーを取得
        lasers = CameraRig.GetComponentsInChildren<LaserController>(true);

        hitObjects = new GameObject[lasers.Length];
        pressedObjects = new GameObject[lasers.Length];
        dragObjects = new GameObject[lasers.Length];
        pointEvents = new PointerEventData[lasers.Length];
    }

    void SetUICamera()
    {
        // GUI-Raycast用のCameraを作成
        //UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        //DontDestroyOnLoad(UICamera);
        UICamera.fieldOfView = 5;
        UICamera.nearClipPlane = 0.01f;
        UICamera.stereoTargetEye = StereoTargetEyeMask.None;
        UICamera.clearFlags = CameraClearFlags.Nothing;
        UICamera.cullingMask = 0;


        // 全UIキャンバスにCameraをセット
        Canvas[] canvases = UnityEngine.Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.worldCamera = UICamera;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------//
    private bool GUIRaycast(int index)
    {
        if (pointEvents == null) return false;

        if (pointEvents[index] == null)
        {
            pointEvents[index] = new PointerEventData(base.eventSystem);
        }
        else
        {
            pointEvents[index].Reset();
        }

        

        // UICameraのベクトルをレーザーと一致させる
        UICamera.transform.position = lasers[index].gameObject.transform.position;
        UICamera.transform.forward = lasers[index].gameObject.transform.forward;

        // UICameraの中心からUIキャンバスへレイキャスト
        pointEvents[index].delta = Vector2.zero;
        pointEvents[index].position = new Vector2(Screen.width / 2, Screen.height / 2);
        pointEvents[index].scrollDelta = Vector2.zero;

        // ヒットテスト
        base.eventSystem.RaycastAll(pointEvents[index], m_RaycastResultCache);
        pointEvents[index].pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();

        return pointEvents[index].pointerCurrentRaycast.gameObject != null;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------//
    public override void Process()
    {

        // 全UIキャンバスとレーザーのヒットテスト
        for (int index = 0; index < lasers.Length; index++)
        {

            // 現在選択中のUIの選択を解除
            ClearSelection();

            // レイキャスト & ヒットテストを行う
            bool hit = GUIRaycast(index);
            if (hit == false)
            {
                lasers[index].AdjustLaserDistance(0);
                continue;
            }

            // ヒットしたオブジェクトを保持
            hitObjects[index] = pointEvents[index].pointerCurrentRaycast.gameObject;
            base.HandlePointerExitAndEnter(pointEvents[index], hitObjects[index]);

            // レーザーの長さを調整
            if (pointEvents[index].pointerCurrentRaycast.distance > 0.0f)
            {
                lasers[index].AdjustLaserDistance(pointEvents[index].pointerCurrentRaycast.distance);
            }

            // プレスダウンをトラック
            if (IsPressDown(index))
            {

                pointEvents[index].pressPosition = pointEvents[index].position;
                pointEvents[index].pointerPressRaycast = pointEvents[index].pointerCurrentRaycast;
                pointEvents[index].pointerPress = null;

                if (hitObjects[index] != null)
                {

                    //プレス開始イベントの発行
                    pressedObjects[index] = hitObjects[index];
                    GameObject newPressed = ExecuteEvents.ExecuteHierarchy(pressedObjects[index], pointEvents[index], ExecuteEvents.pointerDownHandler);
                    if (newPressed == null)
                    {

                        // プレスに反応しないUI用にクリック開始イベントを発行
                        newPressed = ExecuteEvents.ExecuteHierarchy(pressedObjects[index], pointEvents[index], ExecuteEvents.pointerClickHandler);
                        pressedObjects[index] = newPressed;

                    }
                    else
                    {

                        //クリック開始イベントの発行
                        pressedObjects[index] = newPressed;
                        ExecuteEvents.Execute(newPressed, pointEvents[index], ExecuteEvents.pointerClickHandler);
                    }

                    // 新しく押下したUIを保持する
                    if (newPressed != null)
                    {
                        pointEvents[index].pointerPress = newPressed;
                        pressedObjects[index] = newPressed;
                        Select(pressedObjects[index]);
                    }

                    // ドラッグ開始イベントの発行
                    ExecuteEvents.Execute(pressedObjects[index], pointEvents[index], ExecuteEvents.beginDragHandler);
                    pointEvents[index].pointerDrag = pressedObjects[index];
                    dragObjects[index] = pressedObjects[index];
                }
            }

            //ドラッグ中イベントの発行
            if (dragObjects[index] != null)
            {
                ExecuteEvents.Execute(dragObjects[index], pointEvents[index], ExecuteEvents.dragHandler);
            }

            // プレスアップをトラック
            if (IsPressUp(index))
            {

                //ドラッグ終了イベントを発行
                if (dragObjects[index])
                {
                    ExecuteEvents.Execute(dragObjects[index], pointEvents[index], ExecuteEvents.endDragHandler);
                    if (hitObjects[index] != null)
                    {
                        ExecuteEvents.ExecuteHierarchy(hitObjects[index], pointEvents[index], ExecuteEvents.dropHandler);
                    }
                    pointEvents[index].pointerDrag = null;
                    dragObjects[index] = null;
                }

                //プレス終了イベントを発行
                if (pressedObjects[index])
                {
                    ExecuteEvents.Execute(pressedObjects[index], pointEvents[index], ExecuteEvents.pointerUpHandler);
                    pointEvents[index].rawPointerPress = null;
                    pointEvents[index].pointerPress = null;
                    pressedObjects[index] = null;
                    hitObjects[index] = null;
                }
            }
        }
    }

    public static bool test = false; 

    //------------------------------------------------------------------------------------------------------------------------------------------//
    private bool IsPressDown(int index)
    {
        return SteamVR_Actions.default_InteractUI.GetStateDown(SteamVR_Input_Sources.Any);
        /*
        return true;
        
        Debug.LogFormat("IsPressDown {0}", test);
        return test;
        return SteamVR_Actions.default_Teleport.GetStateDown(HandType);
        
        var device = SteamVR_Controller.Input((int)lasers[index].gameObject.GetComponent<SteamVR_TrackedObject>().index);
        return device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        */
    }

    private bool IsPressUp(int index)
    {
        return SteamVR_Actions.default_InteractUI.GetStateUp(SteamVR_Input_Sources.Any);
        //return SteamVR_Actions.default_Teleport.GetStateUp(HandType);
        /*
        var device = SteamVR_Controller.Input((int)lasers[index].gameObject.GetComponent<SteamVR_TrackedObject>().index);
        return device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
        */
    }

    //------------------------------------------------------------------------------------------------------------------------------------------//
    public void ClearSelection()
    {
        if (base.eventSystem.currentSelectedGameObject)
        {
            base.eventSystem.SetSelectedGameObject(null);
        }
    }

    private void Select(GameObject go)
    {
        ClearSelection();
        if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
        {
            base.eventSystem.SetSelectedGameObject(go);
        }
    }


}