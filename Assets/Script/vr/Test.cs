using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Test:MonoBehaviour
{

    public Transform target;
    public GraphicRaycaster canvasRaycaster;
    public List<RaycastResult> list;
    public Vector2 screenPoint;
    public Camera cam;

    void Update()
    {
        list = new List<RaycastResult>();
        screenPoint = cam.WorldToScreenPoint(this.transform.position);
        PointerEventData ed = new PointerEventData(EventSystem.current);
        ed.position = screenPoint;
        canvasRaycaster.Raycast(ed, list);

        if (list != null && list.Count > 0)
        {
            Debug.Log("Hit: " + list[0].gameObject.name);
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}