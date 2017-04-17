using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

//执行步数时,button状态切换
public enum StepStates 
{
   midlle,uptop,downdeep,shoutup
}

//模型状态
public enum ModelStates
{
    none,ready,onair,complete
}

//安装速度
public enum InstallSpeed 
{
    slowspeed, heigherspeed, fasterspeed
}

//选择摄像机
public enum CameraControllerType 
{
    handler,
    choiseauto,//点击按钮切换到自动,没有目标
    atuo        //有目标的自动导航
}

//摄像机此时状态
public enum AutoState 
{
    begine,moveing,end
}

//节点执行进度
public enum Progress 
{
    Dismantling,Complete,Middle
}

public class Gamemanager : Singleton<Gamemanager> {

    //正常安装速度
    private float slowspeed = 0.2f;
    public float SlowSpeed 
    {
        get { return slowspeed; }
        set { slowspeed = value; }
    }
    //点击时步数大于1时,安装速度
    private float heigherspeed = 0.3f;
    //读取xml时直接执行到当前步数时,安装速度
    private float fasterspeed = 10f;


   //碰撞控制器
    private ClliderController collidercontroller;

    // 临时栈,将要执行步骤包含的物体加入该栈
    private Stack<RealObj> realstack = new Stack<RealObj>();


    private UImanager uimanager;


    //上次操作对象的名称,用于写入XML使用
    public string lastmodel = "";
    //当前执行到多少步,用于写入XML使用
    private int m_step = 0;
    public int Step 
    {
        get { return m_step; }
        private set { m_step = value; }
    }
    private int stepMin = 0;
    private int stepMax  = 1;



//    //所有需要操作模型集合
//    public List<GameObject> gos = new List<GameObject>();
//
//    //J4电机组件物体模型集合
//    public List<GameObject> J4Dynamos = new List<GameObject>();
//
//    //J3电机组件物体模型集合
//    public List<GameObject> J3Dynamos = new List<GameObject>();
//
//    //前臂筒模型集合
//    public List<GameObject> Forearmtubes = new List<GameObject>();
//
//    //腕关节总成模型集合
//    public List<GameObject> WristJointAssemblys = new List<GameObject>();
//
//    //手腕侧盖组件模型集合
//    public List<GameObject> WristSideCovers = new List<GameObject>();
//
//    //J5J6箱体总成
//    public List<GameObject> J5J6Dynamos = new List<GameObject>();
//
//    //J5电机组件物体模型集合
//    public List<GameObject> J5Dynamos = new List<GameObject>();
//
//    //J6电机组件物体模型集合
//    public List<GameObject> J5Boxings = new List<GameObject>();
    //当前需操作模型集合
    private List<GameObject> currentitems = new List<GameObject>();


    public override void Awake()
    {
        base.Awake();
        collidercontroller = GetComponent<ClliderController>();
        uimanager = GetComponent<UImanager>();
       
    }

    
    void Start()
    {
      //  ChoesXml(Const.MainXmlPath, "整机", Vector3.zero);
    }

}
