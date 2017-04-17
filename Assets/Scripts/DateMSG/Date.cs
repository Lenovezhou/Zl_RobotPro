using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Date:Singleton<Date>
{
#region 所需操作的模型集合
	//SA1400下模型集合
	public List<GameObject> SA1400models = new List<GameObject>();

	//基体总成物体模型集合
	public List<GameObject> MatrixAssemblys = new List<GameObject>();

	//J2防撞块组件物体模型集合
	public List<GameObject> J2s = new List<GameObject>();

	//旋转臂物体模型集合
	public List<GameObject> RoattingArms = new List<GameObject>();

	//J1J2电机组件物体模型集合
	public List<GameObject> J1J2partses = new List<GameObject>();

	//旋转坐组件的物体模型集合
	public List<GameObject> RotatingSeats = new List<GameObject>();

	//机座的物体模型集合
	public List<GameObject> JZBase = new List<GameObject>();

	//前臂驱动总成物体模型集合
	public List<GameObject> ForwordArmDirves = new List<GameObject>();

	//J4电机组件物体模型集合
	public List<GameObject> J4Dynamos = new List<GameObject>();

	//J3电机组件物体模型集合
	public List<GameObject> J3Dynamos = new List<GameObject>();

	//前臂筒模型集合
	public List<GameObject> Forearmtubes = new List<GameObject>();

	//腕关节总成模型集合
	public List<GameObject> WristJointAssemblys = new List<GameObject>();

	//手腕侧盖组件模型集合
	public List<GameObject> WristSideCovers = new List<GameObject>();

	//J5J6箱体总成
	public List<GameObject> J5J6Dynamos = new List<GameObject>();

	//J5电机组件物体模型集合
	public List<GameObject> J5Dynamos = new List<GameObject>();

	//J6电机组件物体模型集合
	public List<GameObject> J5Boxings = new List<GameObject>();
#endregion
    public Gamemanager gamemanager;
    public override void Awake()
    {
        base.Awake();
        Addmodeltostep();
        gamemanager = GetComponent<Gamemanager>();
//		ItemsAddComponent(SA1400models);
//		ItemsAddComponent(MatrixAssemblys);
//		ItemsAddComponent(J2s);
//		ItemsAddComponent(RoattingArms);
//		ItemsAddComponent(JZBase);
//		ItemsAddComponent(ForwordArmDirves);
//		ItemsAddComponent(J4Dynamos);
//		ItemsAddComponent(J3Dynamos);
//		ItemsAddComponent(Forearmtubes);
//		ItemsAddComponent(WristJointAssemblys);
//		ItemsAddComponent(WristSideCovers);
//		ItemsAddComponent(J5J6Dynamos);
//		ItemsAddComponent(J5Dynamos);
//		ItemsAddComponent(J5Boxings);

//        AddnametoModels();
    }

//    private List<GameObject> gos = new List<GameObject>();

	//导航地图上每个按钮对应的模型列表
	public Dictionary<string,List<GameObject>> maps = new Dictionary<string, List<GameObject>>();

    //模型名称对应模型
    private Dictionary<string, GameObject> m_nameTomodels = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> NAMETOMODELS 
    {
        get { return m_nameTomodels; }
        set { m_nameTomodels = value; }
    }

    //模型名称对应完成进度
    public Dictionary<string, List<int>> m_modelTOstep = new Dictionary<string, List<int>>();

    public Dictionary<string, List<int>> MODELTOSTEP 
    {
        get { return m_modelTOstep; }
        set { m_modelTOstep = value; }
    }


    #region 方法


//    void AddnametoModels()
//    {
//        for (int i = 0; i < gos.Count; i++)
//        {
//            foreach (string item in m_modelTOstep.Keys)
//            {
//                if (item.Equals(gos[i].name))
//                {
//                    m_nameTomodels.Add(item, gos[i]);
//                }
//            }
//        }
//    }

	//设置map对应关系
	public List<GameObject> GetModels(List<string> name,string choiselist) 
	{
		if (maps.ContainsKey(choiselist)) {
			return maps [choiselist];
		}
		List<GameObject> temp = new List<GameObject>();
		List<GameObject> transfor = new List<GameObject> ();
		switch (choiselist)
		{
		case "SA1400A总成":
			temp = SA1400models;
			break;
		case "基体总成":
			temp = MatrixAssemblys;
			break;
		case "J2防撞块组件":
			temp = J2s;
			break;
		case "旋转臂":
			temp = RoattingArms;
			break;
		case "J1J2电机组件":
			temp = J1J2partses;
			break;
		case "旋转座组件":
			temp = RotatingSeats;
			break;
		case "机座":
			temp = JZBase;
			break;
		case "前臂驱动总成":
			temp = ForwordArmDirves;
			break;
		case "J3电机组件":
			temp = J3Dynamos;
			break;
		case "J4电机组件":
			temp = J4Dynamos;
			break;
		case "前臂筒":
			temp = Forearmtubes;
			break;
		case "腕关节总成":
			temp = WristJointAssemblys;
			break;
		case "手腕侧盖组件":
			temp = WristSideCovers;
			break;
		case "J5J6箱体总成":
			temp = J5J6Dynamos;
			break;
		case "J5电机组件":
			temp = J5Dynamos;
			break;
		case "J5箱体组件":
			temp = J5Boxings;
			break;
		default:
			Debug.Log ("<color=yellow>非法输入：：：：：</color>"+choiselist);
			break;
		}
		//效验模型顺序
		for (int i = 0; i < name.Count; i++) {
			for (int j = 0; j < temp.Count; j++) 
			{
				if (name[i].Equals(temp[j].name))
				{
					transfor.Add (temp[j]);
					if (!m_nameTomodels.ContainsKey(name[i]))
					{
						m_nameTomodels.Add(name[i], temp[j]);
					}
					break;
				}
			}
		}
		//添加到字典
		if (!maps.ContainsKey(choiselist)) 
		{
			maps.Add (choiselist, transfor);
		}
		return transfor;
	}

//	public List<GameObject> GetList()
//	{
//		
//	}
	//添加名称与实际模型对应关系
//   public void AddnametoModels(string key, GameObject value) 
//    {
//       // Debug.Log("key::::" + key + "value::::" + value + "contains::::::" + m_nameTomodels.ContainsKey(key));
//        if (!m_nameTomodels.ContainsKey(key))
//        {
//            m_nameTomodels.Add(key, value);
//        }
//    }

    //获取上次存储的模型对应的进度
    void Addmodeltostep()
    {
        m_modelTOstep = MyXML.ReaderStepXml(Const.SavePartesOfSteps);
    }

    //将每一步的进度写入XML
    public void UpdateModelstep(string name, int step,int stepmax)
    {
        MyXML.UpdateStepsXML(Const.SavePartesOfSteps, name, step, stepmax);
    }
#endregion

   
}
