using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBess : Singleton<CameraBess> {
    private bool ishand = true, isauto = false;

    private CameraController handcontroller; //手动旋转的摄像机
    private AutoController autocontroller;  //按照XML设置的节点自动导航摄像机
	//public HandController CoordinateSystem; //导航系统摄像机

    public PathObjects lasttarget;

    public override void Awake()
    {
        base.Awake();
		handcontroller = GetComponentInChildren<CameraController> ();
        autocontroller = GetComponentInChildren<AutoController>();
    }


    public void ChoiseCamra(bool isauto) 
    {
        autocontroller.enabled = isauto;
        handcontroller.enabled = !isauto;
        isauto = isauto;
        ishand = !isauto;
    }

    public void ChoiseCamera(bool isauto , PathObjects target = null,Action act = null )
    {
        if (isauto)
        {
            if (target != null)
            {
                autocontroller.Init(target,CameraControllerType.atuo, act);
            }
        }

        //CoordinateSystem.enabled = !isauto;
        //handcontroller.enabled = !isauto;


        if (!isauto && target != null)
        {
            Vector3 lo = handcontroller.transform.localPosition;
            handcontroller.Init(target,lo);
        }
        lasttarget = target;
    }


    public void DisHandCmaera(bool isall) 
    {
        if (ishand)
        {
            handcontroller.enabled = isall;
        }
    }

    private Vector3 mousePoint;
//    void Update()
//    {
//        if (Input.GetMouseButton(2) && (Input.GetKey("left ctrl") || Input.GetKey("right ctrl")))
//        {
//            float z = Input.GetAxis("Mouse X")  * Time.deltaTime;
//            float y = Input.GetAxis("Mouse Y")  * Time.deltaTime;         
//   
//			transform.position -= transform.up * y;
//			transform.position -= transform.right * z;
////            transform.position+=new Vector3(0,-y,-z);
//        }
//    }
}
