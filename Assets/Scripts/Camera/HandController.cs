
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 坐标系专用相机
/// </summary>
public class HandController:MonoBehaviour
{
	private float begineZ;

	private float x = 0.0f;
	private float y = 0.0f;
	private float h = 0.0f;

	public float xSpeed = 200;
	public float ySpeed = 200;
	public float mSpeed = 10;


    public float ScrollWheelspeed = 1;

	public float yMinLimit = -50;
	public float yMaxLimit = 50;

	private float distance = 3;
	public float minDistance = -0.4f;
	public float maxDistance = -1.4f;



	public bool needDamping = true;
	float damping = 5.0f;

    bool ishandler = false;
    Quaternion targetquaternion;


   public Transform TarTransform;

   public Transform lerptarget;

	void Awake()
	{
        TarTransform.gameObject.SetActive(true);
		begineZ = transform.position.z;
        //Debug.Log (begineZ);
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

    public void SetRotation(bool ishandlermove,Quaternion q) 
    {
        targetquaternion = q;
        ishandler = ishandlermove;
        Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
    }



	void LateUpdate()
	{
        Vector3 temptarget = new Vector3(lerptarget.position.x, 0, lerptarget.position.z);
        transform.rotation = lerptarget.rotation;
	    transform.position = temptarget;
	}

}
