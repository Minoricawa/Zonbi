using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller : MonoBehaviour
{
    [SerializeField] LineRenderer beam;
    public GameObject reticle = null;

    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Input_Sources HandType;



    // Start is called before the first frame update
    void Start()
    {
        //beam.SetActive(false);
        beam.startWidth = 0.001f;
        beam.endWidth = 0.001f;
        beam.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            beam.SetPosition(0, this.transform.position);
            beam.SetPosition(1, hit.point);

            if (reticle == null)
            {
                reticle.transform.position = hit.point;
            }

            if (SteamVR_Actions.default_Teleport.GetStateDown(HandType))
            {
                Debug.Log(hit.collider.name);

                if (hit.collider.name == "aaa")
                {
                    Debug.Log("aaaにクリック");

                }
            }
            

        }


    }
}
