using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RealObj :  StateMachine{
	private float fasterspeed = 5f;
    private Vector3 NormalOffset = new Vector3(0,0.397f,-0.011f);

	private float timer = 1f;

	private Renderer[] rendermeshs ;
	private Color[] selfmeshcolors;

    private int currentWayPoint = 0;
    private bool IsExcute = false;
    private bool IsUndo = false;
    private PathObjects[] path = new PathObjects[3];//路径
    public MyTransform my;

    private PathObjects camepath;

    private float selfdamping = 0.2f;

    private float pathpointdistance = 0.0001f;

    public ModelStates ms;

    private BoxCollider box;

    public List<ShowMessage> subMats;

    //通知gamemanager
	public Action<int,InstallSpeed> move_end;

    public Button selfbutton;

	//brief 跟随路径点正向移动，如果有多种状态，继续添加
	private State follow = new State();
	///brief跟随路径点反向移动 ，如果有多种状态，继续添加
	private State chase = new State();
	//停止移动
	private State stopmove = new State();
	//闪光
	private State sparking = new State();
    //移动摄像机
    private State cameramoveing = new State();

	private InstallSpeed selfinstallspeed;

    public void Init(MyTransform my,  List<ShowMessage> subMats) 
    {
        this.subMats = subMats;
        this.my = my;
		RefreshSelfButton (true);

        PathObjects po1 = new PathObjects(my.Spownpos , my.Spownrot);

        PathObjects po2 = new PathObjects(my.Spownpos , my.Spownrot);

        PathObjects po3 = new PathObjects(my.Targetpos , my.Targetrot);

        camepath = new PathObjects(my.Camerapos , my.Camerarot);

        path[0] = po1;
        path[1] = po2;
        path[2] = po3;

    }


    void Awake() 
    {
	//安装
        follow.OnEnter = Spark;
        follow.OnUpdate = StartFollow; //跟随路径状态
        follow.OnLeave = UnSpark;

	//拆解
        chase.OnEnter = Spark;
		chase.OnUpdate = StartUnDo; //追逐状态
        chase.OnLeave = UnSpark;

	//闪光
        sparking.OnEnter = Spark;
        sparking.OnLeave = UnSpark;
        sparking.OnUpdate = StartSparking;
    
    //移动摄像机

        cameramoveing.OnEnter = delegate
        {
            CameraBess.GetInstance.ChoiseCamera(true, camepath,EndCameraMoveCall);
        };

        subMats = new List<ShowMessage>();
        box = GetComponent<BoxCollider>();
		selfmeshcolors = new Color[transform.childCount];
		ms = ModelStates.none;
		rendermeshs = GetComponentsInChildren<MeshRenderer> ();
    }

    //子物体闪烁
    public void Spark() 
    {
       // iTween.Resume(gameObject);
        for (int i = 0; i < subMats.Count; i++)
        {
           // subMats[i].Spark();
            subMats[i].HighLight();
        }
    }

    //取消闪烁
    public void UnSpark() 
    {
       // iTween.Pause();
        for (int i = 0; i < subMats.Count; i++)
        {
            //subMats[i].ResetColor();
            subMats[i].ResetHighLight();
        }
    }


    //装
	public void Excute(bool _isexcute,InstallSpeed inspeed, Action<int,InstallSpeed> act = null) 
    {
        IsExcute = _isexcute;
		selfinstallspeed = inspeed;
		switch (inspeed)
		{
			//手动改变安装速度
			case InstallSpeed.slowspeed:
			case InstallSpeed.heigherspeed:
                if (gameObject.name =="RAJ1减速机")
                {
                    Debug.Log("<><><><><");
                }

				selfdamping = UImanager.GetInstance.SlowSpeed;
                state = cameramoveing;
				break;
			case InstallSpeed.fasterspeed:

                if (gameObject.name == "RAJ1减速机")
                {
                    Debug.LogError("<><>父父父<><><");
                }

                if (gameObject.name =="RA151.99x3.53O型圈")
                {
                    Debug.Log("<><>子子子<><><");
                }

				selfdamping = fasterspeed;
					if (_isexcute)
					{
						currentWayPoint = 2;
						transform.position = path[currentWayPoint].position;
						transform.eulerAngles = path [currentWayPoint].eulerangel;
					} else 
					{
						currentWayPoint = 0;
						transform.position = path[currentWayPoint].position;
						transform.eulerAngles = path [currentWayPoint].eulerangel;	
                        if (gameObject.name =="151.99x3.53O型圈")
                        {
                        Debug.Log( transform.localPosition);
                         //   transform.localPosition = new Vector3(1.12892f,-0.0149f,-8.9127f);
                        }
					}
					switch (gameObject.name)
					{
					case "AS568 O形圈151.99x3.53-DC_Shell":	
						transform.localPosition = new Vector3 (0,-0.0156f, 0);
						break;
					case"151.99x3.53O型圈":
						transform.localPosition = new Vector3 (0,-0.01f,0);
						break;
					default:
						break;
					}
                    CameraBess.GetInstance.ChoiseCamera(true, camepath);
				break;
			default:
				break;
		}
        move_end = act;
	}

	void RefreshSelfButton (bool isactive)
	{
		if (selfbutton && selfbutton.transform.Find("Line"))
		{
			selfbutton.transform.Find("Line").gameObject.SetActive(isactive);
		}
		if (selfbutton) {
			selfbutton.transform.Find("Checkmark").gameObject.SetActive(isactive);	
		}
	}

    void Update() 
    {
		OnUpdateState(Time.deltaTime);
		if (ms == ModelStates.onair) 
		{
			timer += Time.deltaTime;
			if (timer > 0.5f && selfbutton)
            {
				timer = 0;
                //代表自身步骤的button闪烁
			//	RefreshSelfButton (!selfbutton.transform.Find("Checkmark").gameObject.activeSelf);
			}
		}
    }


	private void StartFollow(float obj) 
    {
        if (currentWayPoint <= path.Length - 1 && Vector3.Distance(transform.position, path[currentWayPoint].position) > pathpointdistance)
        {
            transform.position = Vector3.Lerp(transform.position, path[currentWayPoint].position, selfdamping);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, path[currentWayPoint].eulerangel, selfdamping);
            if (ms != ModelStates.onair)
            {
                ms = ModelStates.onair;
            }           
        }
        else
        {
            //Debug.Log("正向执行到这里::::");
            currentWayPoint++;
            if (currentWayPoint >= path.Length)
            {
                currentWayPoint = path.Length - 1;

                ms = ModelStates.complete;
                UnSpark();
                if (move_end != null)
                {
					move_end(1,selfinstallspeed);
                }
				state = stopmove;
				RefreshSelfButton(true);	
            }
        }
    }

	private void StartUnDo(float obj) 
    {
        if (currentWayPoint >= 0 && Vector3.Distance(transform.position, path[currentWayPoint].position) > pathpointdistance)
        {
            transform.position = Vector3.Lerp(transform.position, path[currentWayPoint].position, selfdamping);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, path[currentWayPoint].eulerangel, selfdamping);
			if (ms != ModelStates.onair) 
            {
				ms = ModelStates.onair;
			}
        }
        else
        {
            currentWayPoint--;
            if (currentWayPoint < 0)
            {
                currentWayPoint = 0;
                if (move_end != null)
                {
					move_end(-1, selfinstallspeed);
                }
				state = stopmove;
				ms = ModelStates.complete;
                UnSpark();
               
				RefreshSelfButton (false);
                return;
            }
        }
    }


    private void StartSparking(float obj) 
    {
        if (stateTime > 0f)
        {
            state = cameramoveing;
        }
    }

    private void EndCameraMoveCall() 
    {
        if (IsExcute)
        {
            state = follow;
        }
        else
        {
            state = chase;
        }

    }
}
	

public class PathObjects 
{
    public Vector3 position;
    public Vector3 eulerangel;

    public PathObjects(Vector3 p,Vector3 e)
    {
        this.position = p;
        this.eulerangel = e;
    }
}
