using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 手动转动摄像机专用
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform TarTransform;

    public float xSpeed = 200;
    public float ySpeed = 200;
    public float mSpeed = 10;
    public float yMinLimit = -50;
    public float yMaxLimit = 50;
    public float distance = 10;
    public float minDistance = 2;
    public float maxDistance = 30;

    //鼠标滚轮运动速率
    public float ScrollWheelspeed = 10;

    //是否平滑运行
    public bool needDamping = true;
    public float damping = 20.0f;

    public float x = 0.0f;
    public float y = 0.0f;
    public float h = 0.0f;
    public float w = 0.0f;

    private float startX,startY;
    Vector3 m_position;

    private bool IsChangeMiddle = false;
    private Vector3 center;

    private float originDis;

    //一定时间内双击有效
    private float timer = 0;

    //计数器
    private int count = 0;

    void Awake()
    {
        originDis = distance;
        Vector3 angles = transform.eulerAngles;
        startX = angles.y;
        startY = angles.x;


        transform.localEulerAngles = Vector3.zero;

        transform.localPosition = new Vector3(0, 0.57f, -2.09f);
        x = angles.y;
        y = angles.x;
        h = 0.57f;
        w = 0;
        // TarTransform.position = transform.parent.position;

    }

    void OnEnable()
    {
       
    }


   

    public void Init(PathObjects target, Vector3 local) 
    {
        center = target.position;
        //distance = Mathf.Abs(local.z) ;
        //Debug.Log("distance::::"+distance);
        //x = 0;
        //y = 0;
       // h = local.y;
        IsChangeMiddle = true;
       // distance = dis;
    }


    Vector3 newPos, oldPos, posDir;

    void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        //手动调节摄像机对焦点
        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    w += Input.GetAxis("Horizontal") * 1 * Time.deltaTime;
        //}

        //if (Input.GetAxis("Vertical") != 0)
        //{
        //    h += Input.GetAxis("Vertical") * 1 * Time.deltaTime;
        //}

        //同时按住鼠标中键和ctrl键,变换视角
        //if (Input.GetMouseButton(2) && (Input.GetKey("left ctrl") || Input.GetKey("right ctrl")))
        //{
        //    w -= Input.GetAxis("Mouse X") * 5 * Time.deltaTime;
        //    h -= Input.GetAxis("Mouse Y") * 5 * Time.deltaTime;


        if (Input.GetMouseButton(2) && (Input.GetKey("left ctrl") || Input.GetKey("right ctrl")))
        {
            float z = Input.GetAxis("Mouse X") * 5 * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * 5 * Time.deltaTime;

            float rate = distance / originDis;

			transform.position -= transform.up * y*rate;
			transform.position -= transform.right * z*rate;

            TarTransform.position += new Vector3(0, -y*rate, -z*rate);
        }

        //}
        //双击鼠标中键模型回到视野中央
        if (Input.GetMouseButtonDown(2))
        {
            count++;
            if (count == 1)
            {
                timer = Time.time;
            }

            if (count == 2 && Time.time - timer <= 0.5f)
            {
                count = 0;
                w = 0;
                h = 0;
                distance = 2.8f;
                //x = startX;
                //y = startY;
            }
            if (Time.time - timer > 0.5f)
            {
                count = 0;
            }
        }

        if (TarTransform)
        {
            //鼠标控制
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
            }



            //触屏控制
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                newPos = Input.mousePosition;
                if (Input.GetMouseButtonDown(0))
                {
                    oldPos = newPos;
                }
                posDir = (newPos - oldPos) * 0.1f;
                float touchX = posDir.x;
                float touchY = posDir.y;


                x += touchX * xSpeed * 0.02f;
                y -= touchY * ySpeed * 0.02f;

                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

                oldPos = newPos;

            }

            distance -= Input.GetAxis("Mouse ScrollWheel") * ScrollWheelspeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(w, h, -distance);

            Vector3 m_position = rotation * disVector + TarTransform.position;
            if (needDamping)
            {
				transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, m_position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = m_position;
            }
        }
        else if (TarTransform)
        {
            if (Input.GetMouseButton(0))
            {
                float v = Input.GetAxis("Mouse X");
                //float h = Input.GetAxis("Mouse Y");

                if (v != 0)
                {
                    transform.RotateAround(TarTransform.position, TarTransform.up, v * 10);
                }
            }
        }
    }      
}