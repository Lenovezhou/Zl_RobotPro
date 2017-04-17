using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UIEvents:MonoBehaviour
{
	private float slowspeed = 0.2f;
	public float SlowSpeed 
	{
		get { return slowspeed; }
		set { slowspeed = value; }
	}
	//点击时步数大于1时,安装速度
	//private float heigherspeed = 0.3f;
	//读取xml时直接执行到当前步数时,安装速度
	private float fasterspeed = 5f;

    //名称即路径
    [SerializeField]
    private string Name;

    public int maxStep;

    //手动点击上一步,下一步时,上次闪烁物体
    private GameObject lastSparkingobj;

   
    public int step;

    private int stepmin = 0;

    private MapController mapcontroll;

    private float result;

    private Button selfbutton;

	//自身控制的模型list
	private List<GameObject> selfModles = new List<GameObject>();

	//根据自身名字读取获得的mytransform信息
	private Dictionary<string, MyTransform> my = new Dictionary<string, MyTransform>();

	private Stack<RealObj> realstack = new Stack<RealObj>();

    //子步骤
    public List<UIEvents> Contact = new List<UIEvents>();
    //父步骤
    public UIEvents parent_contact;

    public int id;

    public int[] Contacts;

	//所有父节点
	public List<GameObject> AllParents ;
	//所有子节点
	public List<GameObject> AllChildren;

    void Awake() 
    {
        selfbutton = GetComponent<Button>();
        selfbutton.onClick.AddListener(butclick);
    }


    public void Change(UIEvents uie) 
    {
        bool interactable_child = false;
        //bool interactable_parent = false;
        if (!Contact.Count.Equals(0))
        {
            for (int i = 0; i < Contact.Count; i++)
            {
                if (Contact[i].result.Equals(1) && parent_contact != null && parent_contact.result.Equals(0))
                {
                    interactable_child = true;
                }
                else if (!Contact[i].result.Equals(1) || (parent_contact != null && parent_contact.result.Equals(1)))
                {
                    interactable_child = false;
                    break;
                }
                else if(parent_contact == null && Contact[i].result.Equals(1))
                {
                    if (Contact[i].result.Equals(1))
                    {
                        interactable_child = true;
                    }
                    else {
                        interactable_child = false;
                        break;
                    }
                }
            }
        }
        else if (parent_contact != null && Contact.Count.Equals(0))
        {
            if (parent_contact.result.Equals(0))
            {
                interactable_child = true;
            }
            else
            {
                interactable_child = false;
            }
        }
        if (interactable_child)
        {
            selfbutton.GetComponent<Image>().color = Color.white;               //new Color(0,0.76f,1f,0.2f);
        }
        else {
            selfbutton.GetComponent<Image>().color = Color.black;                          //new Color(0.27f,0.27f,0.27f,0.5f);
        }
       // selfbutton.interactable = interactable_child;
    }



	/// <summary>
    /// 根据自己执行进度来刷新父节点及子节点的可交互情况
    /// </summary>
    /// <param name="result"></param>
    public void SetInterractable(float result) 
    {
        //子节点可操作
        bool childcontact = false;
        bool parentcontact = false;
        //自身已拆解完成or安装完成
        if (result.Equals(0))
        {
            childcontact = true;
            parentcontact = false;
        }
        else if (result.Equals(1))
        {
            parentcontact = true;
            if (parent_contact != null)
            {
                for (int i = 0; i < parent_contact.Contact.Count; i++)
                {
                    UIEvents ui = parent_contact.Contact[i];
                    if ((float)ui.step / ui.maxStep != 1)
                    {
                        parentcontact = false;
                    }
                }
            }
            childcontact = false;
        }
        //else {
        //    parentcontact = false;
        //    childcontact = false;
        //}
        for (int i = 0; i < Contact.Count; i++)
        {
            UIEvents ui = Contact[i];
            ui.selfbutton.interactable = childcontact;
        }
        if (parent_contact != null)
        {
       //     parent_contact.selfbutton.interactable = parentcontact;  
        }
    }





    //分两步初始化
    public void Init(MapController m,string _name,int _step,int stepMax,int _id) 
    {
        this.mapcontroll = m;
        this.Name = _name;
		this.step = _step;
        this.maxStep = stepMax;
        this.id = _id;
		string path = Application.dataPath + "/XML/MiniParts/" + _name + ".xml";
		my = MyXML.ReaderXml(path);
		selfModles = Date.GetInstance.GetModels (new List<string>(my.Keys),_name);
		InitSelfModels (selfModles);
		//SetStep (_step);

    }


	private void InitSelfModels(List<GameObject> lis)
	{
//        Debug.Log(Name +"lis.count||||||||"+lis.Count);
		for (int i = 0; i < lis.Count; i++) 
        {
            if (lis[i].name.Equals("J6箱体壳体"))
            {
                Debug.Log("<<<<<<<<<<<<<<");
            }
			if (!lis[i].GetComponent<RealObj>())
			{
				lis[i].AddComponent<RealObj>();                
			}
			List<ShowMessage> mats= new List<ShowMessage>();
			MeshRenderer[] meshs = lis[i].GetComponentsInChildren<MeshRenderer>();
			for (int j = 0; j < meshs.Length; j++)
			{    
                BoxCollider box = null;
                if (!meshs[j].gameObject.GetComponent<BoxCollider>())
                {
                     box = meshs[j].gameObject.AddComponent<BoxCollider>();
                }
                box = meshs[j].gameObject.GetComponent<BoxCollider>();
              
                if (!meshs[j].gameObject.GetComponent<ShowMessage>())
                {
                    meshs[j].gameObject.AddComponent<ShowMessage>();
                }

                ShowMessage showMessage = meshs[j].gameObject.GetComponent<ShowMessage>();
                showMessage.Init(my[lis[i].name]);
                mats.Add(showMessage);
                //矫正boxcollider尺寸
                switch (meshs[j].name)
                {
                    case "旋转座none001":
                         showMessage.contect = showMessage.transform.parent.GetComponent<ShowMessage>();
                         showMessage.transform.parent.GetComponent<ShowMessage>().contect = showMessage;
                        break;
                    case "底座限位座-DC_Shell":
                        box.size = new Vector3(box.size.x,box.size.y,0.08f);
                        break;
                    case "J4电机-02-DC_Shell":
                        box.size = new Vector3(0.15f,0.2f,box.size.z);
                        break;
                    case "J3减速机压垫-DC_Shell":
                        box.center = new Vector3(box.center.x, box.center.y, -0.03f);
                        box.size = new Vector3(box.size.x, box.size.y, 0.11f);
                        break;
                    case "J2减速机压垫-DC_Shell":
                         box.center = new Vector3(box.center.x, box.center.y, 0.06f);
                        box.size = new Vector3(box.size.x, box.size.y, 0.11f);
                        break;
                    default:
                        break;
                }
               
                
        
                
			}
			RealObj real = lis[i].GetComponent<RealObj>();
			foreach (string item in my.Keys)
			{
				if (item.Equals(lis[i].name))
				{
					real.Init(my[item], mats);
				}
			}

		}


	}
		

	//设置自身执行步骤
	public void SetStep(int targetstep ,InstallSpeed _ins = InstallSpeed.slowspeed, bool issingle = false)
	{
//		Debug.Log (Name+"执行步骤：：：："+targetstep );
		Stack<RealObj> rstack = new Stack<RealObj>();
		if (issingle) {
			targetstep = this.step + targetstep;
			if (targetstep > this.step) {
				realstack = PushToStack (targetstep - 1, this.step - 1);
			} else {
				realstack = PushToStack (targetstep, this.step);
			}
		} else {
			if (targetstep > this.step) {
				realstack = PushToStack(targetstep -1 ,this.step -1);
			} else {
				realstack = PushToStack(targetstep ,this.step);
			}
			UImanager.GetInstance.SetOtherButtonDisable(false);
		}
		if (realstack.Count == 0)
		{
			return;
		}

		RealObj re = realstack.Pop();
	
	

        //Debug.Log("现在步数"+step+">>>>>>共推出多少个元素？？？"+realstack.Count+"第一次退出:::::"+re.name);
		re.Excute(targetstep >= this.step, _ins, ModelMoveEnd);
		if (issingle)
		{
			re.Spark();
		}
	}

	Stack<RealObj> PushToStack(int targetstep ,int currentstep)
	{
		//       Debug.Log ("targetstep::::" + targetstep + "currentstep::::" + currentstep);
		int move = targetstep - currentstep;
		Stack<RealObj> s = new Stack<RealObj>();

		if (move > 0)
		{
			for (int i = targetstep; i > currentstep; i--)
			{
				s.Push(selfModles[i].GetComponent<RealObj>());
				//Debug.Log("正向:推入::" + currentitems[i].name);
			}
		}
		else if (move < 0)
		{
			for (int i = targetstep; i < currentstep; i++)
			{
				Debug.Log (selfModles[i].name + "<<<<<<<<<<<<<");
				s.Push(selfModles[i].GetComponent<RealObj>());
				//Debug.Log("反向:::"+currentitems[i].name);
			}
		}
		else {
			s.Push(selfModles[0].GetComponent<RealObj>());
		}
		return s;
	}


	private RealObj lastobj;
	//回调执行
	private void ModelMoveEnd(int aspeacket,InstallSpeed ins)
	{
		step += aspeacket;
//		Debug.Log ("有没有执行步骤添加?"+ step);
		if (realstack.Count > 0)
		{
			RealObj re = realstack.Pop();
			lastobj = re;
			re.Excute(aspeacket > 0, ins, ModelMoveEnd);
		} else {
			if (lastobj != null)
			{
				CameraBess.GetInstance.ChoiseCamera(false, new PathObjects(lastobj.transform.position, lastobj.transform.eulerAngles));
			}
			UImanager.GetInstance.SetOtherButtonDisable ();
            mapcontroll.SetAllChildActive(true);
			LimitStep ();
           // NextModelSparking(aspeacket);
		}
	}
	private void LimitStep()
	{
		if (step >= maxStep)
		{
			step = maxStep;
			UImanager.GetInstance.DisableBut(StepStates.uptop);
		}
		else if (step <= stepmin)
		{
			step = stepmin;
			UImanager.GetInstance.DisableBut(StepStates.downdeep);
		}
		else
		{
			UImanager.GetInstance.DisableBut(StepStates.midlle);
		}
		RefreshSelef (step);
		//刷新UI显示
		//MapController.GetInstance.RefreshMapitem(Step, lastmodel);
	}

    private void NextModelSparking(int aspeacket) 
    {
        //根据回调时的方向让下一个物体闪烁
        if (lastSparkingobj)
        {
            lastSparkingobj.GetComponent<RealObj>().UnSpark();
        }
        GameObject temp = null;
        if (step > 0 && step < maxStep)
        {
            if (aspeacket > 0)
            {
                selfModles[step].GetComponent<RealObj>().Spark();
                temp = selfModles[step];
                //   Debug.Log("<color=yellow>正向:::::" + selfModles[step].name + "</color>");
            }
            else
            {
                temp = selfModles[step - 1];
                selfModles[step - 1].GetComponent<RealObj>().Spark();
                //   Debug.Log("<color=red>反向::::::" + selfModles[step  - 1].name + "</color>"); 

            }
        }
        lastSparkingobj = temp;
        //else if (step == 0)
        //{
        //    selfModles[step].GetComponent<RealObj>().Spark();
        //}
        //else {
        //    selfModles[step].GetComponent<RealObj>().Spark();
        //}
 
    }


    public void Init(List<UIEvents> _contact)
    {
        this.Contact = _contact;
        RefreshSelef(step);
        Contacts = new int[Contact.Count];
    }

    void SendMessages() 
    {
        for (int i = 0; i < Contact.Count; i++)
        {
            Contact[i].Change(this);
        }
        if (parent_contact)
        {
            parent_contact.Change(this);
        }
    }


    //刷新自身UI并根据当前步骤所占比例
    public void RefreshSelef(int step) 
    {
        GetComponentInChildren<Text>().text = Name+"\n" + step + "/" + maxStep;
        result = (float)step / maxStep;

        SendMessages();
    }


    //绑定所有按钮事件
   public void butclick() 
    {
		if (mapcontroll.CURRENTBUT.Equals (Name)) {
			return;
		} else {
			InstallAll();
			UnInstallAll ();			
			mapcontroll.CURRENTBUT = Name;
            UImanager.GetInstance.CleanButtons();
			ObjectPool.GetInstance.UnspawnAll();
			for (int i = 0; i < selfModles.Count; i++) 
			{
				RealObj real = selfModles[i].GetComponent<RealObj>();
				GameObject bu = ObjectPool.GetInstance.Spawn("Button") as GameObject;
				RefreshSelfButton (bu);
				UImanager.GetInstance.buttons.Add(bu.GetComponent<Button>());
				bu.transform.SetParent(UImanager.GetInstance.content);
				bu.transform.localScale = Vector3.one;
				bu.GetComponent<SelfButtonController>().Init(i + 1, selfModles[i].name,Buck);
				real.selfbutton = bu.GetComponent<Button>();
			}
			mapcontroll.PlayMap(false, -1, "",null);
			//SetStep (0, InstallSpeed.fasterspeed, false);
			UImanager.GetInstance.SetDelegate(this,Name);
		}
    }

	//生成的按钮执行事件
	void Buck(Button bu,int _step)
	{
		SetStep (_step);
		UImanager.GetInstance.SetOtherButtonDisable(bu);
        mapcontroll.SetAllChildActive(false);
	}
	void RefreshSelfButton (GameObject bu)
	{
		if (bu.transform.Find("Line"))
		{
			bu.transform.Find("Line").gameObject.SetActive(true);
		}

		bu.transform.Find("Checkmark").gameObject.SetActive(true);	

	}

   void InstallAll() 
   {
		for (int i =  AllChildren.Count - 1; i >= 0 ; i--) 
		{
			UIEvents ue = AllChildren [i].GetComponent<UIEvents> ();
			if (ue.step != ue.maxStep) 
			{
                //Debug.Log (ue.Name +"<color=yellow>InstallAll将要执行到多少步？？？？</color>" + ue.maxStep);
				ue.RechineDestination (true);
				ue.step = ue.maxStep;
				ue.RefreshSelef (ue.step);
			}
		}
   }
   void UnInstallAll() 
   {
//		Debug.Log ("parent>>>NAME::::"+AllParents.Count);
		for (int i = 0; i < AllParents.Count ; i++) 
		{
			UIEvents ue = AllParents [i].GetComponent<UIEvents> ();
			if (ue.step != 0) 
			{
                //Debug.Log (ue.Name +"<color=red>UnInstallAll将要执行到多少步？？？？</color>" + ue.maxStep);
				ue.RechineDestination (false);	
				ue.step = 0;
				ue.RefreshSelef (ue.step);
			}
		}
   }
    //直接到达命令点  涉及父子关系，分执行方向改变发送消息顺序
	public void RechineDestination(bool _isexecute)
	{
		if (_isexecute) {
			for(int i = 0; i < selfModles.Count ; i ++)
			{
				selfModles [i].GetComponent<RealObj> ().Excute (_isexecute, InstallSpeed.fasterspeed);
			}

		} else {
		
			for (int i = selfModles.Count -1; i >=0 ; i--)
			{
				selfModles [i].GetComponent<RealObj> ().Excute (_isexecute, InstallSpeed.fasterspeed);
			}

		}
	}	
}
