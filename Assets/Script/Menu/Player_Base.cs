using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : MonoBehaviour
{
    // 以下メンバ変数定義(SerializeField).
    [SerializeField] Transform camera_ = null;
    [SerializeField] Transform camera_dai = null;
    


    void Start()
    {

    }

    
    void Update()
    {
        float Xrot = Input.GetAxis("Mouse X");
        float Yrot = Input.GetAxis("Mouse Y");


        // 視点操作、上下左右反転
        if (Option.eyes_lr && Option.eyes_ud)
        {
            camera_.transform.Rotate(-Yrot, 0, 0);
            camera_dai.transform.Rotate(0, Xrot, 0);
        }
        else if(!Option.eyes_lr && Option.eyes_ud)
        {
            camera_.transform.Rotate(-Yrot, 0, 0);
            camera_dai.transform.Rotate(0, -Xrot, 0);
        }
        else if(Option.eyes_lr && !Option.eyes_ud)
        {
            camera_.transform.Rotate(Yrot, 0, 0);
            camera_dai.transform.Rotate(0, -Xrot, 0);
        }
        else
        {
            camera_.transform.Rotate(Yrot, 0, 0);
            camera_dai.transform.Rotate(0, -Xrot, 0);
        }
        
    }
}
