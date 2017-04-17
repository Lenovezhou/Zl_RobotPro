using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoController : StateMachine
{

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

    CameraControllerType cameraType;
    Quaternion targetquaternion;

    AutoState autoss;


    private Action selfact;

    private PathObjects AutoTarget;


    private State moveto = new State();

    private State stopat = new State();

    public void Init(PathObjects target,CameraControllerType cc,Action act = null)
    {
        stopat.OnEnter = act;

        if (this.enabled)
        {
            state = moveto;
        }
        else {
            state = stopat;
        }

        if (target != null)
        {
            AutoTarget = target;
        }
        this.cameraType = cc;
    }


    void OnEnable() 
    {
        state = moveto;
        transform.localEulerAngles = Vector3.zero;
        if (AutoTarget == null)
        {
            transform.localPosition = new Vector3(0,0.57f,-2.09f);
        }
    }

    void Update() 
    {
        OnUpdateState(Time.deltaTime);
    }


    void Awake()
    {
        moveto.OnUpdate = StartMove;
    }



    void StartMove(float obj)
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;
        if (AutoTarget != null)
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(AutoTarget.eulerangel), Time.deltaTime * damping);

            transform.position = Vector3.Lerp(transform.position, AutoTarget.position, Time.deltaTime * damping);
            if (Vector3.Distance(transform.position, AutoTarget.position) < 0.01f)
            {
                state = stopat;
            }
        }
        else {
            state = stopat;
        }  
        
    }


}
