using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class MapController:Singleton<MapController>
{
    private Animation selfanim;


    private List<string> buttonnames;
    private List<List<int>> steps;
    private List<int> ids;
    //地图下的所有button
    private Dictionary<string,UIEvents> childitems;


	//当前操作button
	private string currentBut = "";
	public string CURRENTBUT
	{
		get{ return currentBut;}
		set{ currentBut = value;}
	}

    public override void Awake()
    {
        base.Awake();
        selfanim = GetComponent<Animation>();
    }


    void Start() 
    {
        //子button的数目必须和进度XML的子节点数目相同
        if (buttonnames == null)
        {
            buttonnames = new List<string>(Date.GetInstance.MODELTOSTEP.Keys);
        }
        if (steps == null)
        {
            steps = new List<List<int>>(Date.GetInstance.MODELTOSTEP.Values);
        }
        if (childitems == null)
        {
            childitems = new Dictionary<string, UIEvents>();
            //Add进字典时,childitems还没有添加完成
            for (int i = 0; i < transform.childCount; i++)
            {
                UIEvents ue = transform.GetChild(i).GetComponent<UIEvents>();
                if (ue)
                {
                    ue.Init(this, buttonnames[i], steps[i][0], steps[i][1], steps[i][2]);
                    childitems.Add(buttonnames[i], ue);
                }
            }
        }
       
        InitAssigne();
    }

    void InitAssigne() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UIEvents ue = transform.GetChild(i).GetComponent<UIEvents>();
            if (ue)
            {
                ue.Init(AddContact(childitems[buttonnames[i]]));
            }
        }
    }

    #region 回调
    void CallBack() 
    {
        
    }
    #endregion



    public void SetAllChildActive(bool isactive) 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Button>())
            {
                transform.GetChild(i).GetComponent<Button>().interactable = isactive;
            }
        }
    }


    public void RefreshMapitem(int step,string whichbutton) 
    {
        if (string.IsNullOrEmpty(whichbutton))
        {
            return;
        }

        UIEvents ui = childitems[whichbutton];

        childitems[whichbutton].RefreshSelef(step);
        //写入进度到XML(暂时不写)
       // Date.GetInstance.UpdateModelstep(whichbutton, step, ui.maxStep);
    }


    /// <summary>
    /// 添加联系
    /// </summary>
    /// <param name="ue"></param>
    /// <returns></returns>
    List<UIEvents> AddContact(UIEvents ue) 
    {
        int id = ue.id;
        //Debug.Log("ue.id::::"+ue.id);
        List<UIEvents> Relative = new List<UIEvents>();
        for (int i = 0; i < buttonnames.Count; i++)
        {
            //检查节点,在比其ID*10大且在+10以内
            if (childitems[buttonnames[i]].id > id * 10 && childitems[buttonnames[i]].id < id * 10 + 10)
            {
             //   Debug.Log(ue.name +"<<<<<"+ue.id+ "添加子关系:::" + childitems[buttonnames[i]].id);
                Relative.Add(childitems[buttonnames[i]]);

                childitems[buttonnames[i]].parent_contact = ue;
            }
        }
        for (int i = 0; i < buttonnames.Count; i++) 
        {
            if (childitems[buttonnames[i]].id == id / 10)
            {
             //   Relative.Add(childitems[buttonnames[i]]);
             //   Debug.Log("找到::" + ue.name + "的父物体::::" + childitems[buttonnames[i]].name);
            }
        }
        return Relative;
    }



    Action<bool> last;
    public void PlayMap(bool isshow,int step,string _name ,Action<bool> act) 
    {
        if (act != null)
        {
            last = act;
        }
        last(isshow);



        if (isshow)
        {
            selfanim.Play("Map");
            RefreshMapitem(step, _name);
        }
        else {
            selfanim.Play("CloseMap");
        }
    }

    public void HideMap() 
    {
     //   selfanim.Play("CloseMap");
    }

}
