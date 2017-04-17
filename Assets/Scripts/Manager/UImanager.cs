using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UImanager : Singleton<UImanager> {

    private Gamemanager gamamanager;

    //上一步,下一步按钮
    public Button Previous,Next;

    //地图toggle
    public Toggle MapToggle;

    //零件的提示内容
	public GameObject note;

    //当前contant内的buttons
	public List<Button> buttons;

    //主页抬头
    public GameObject Title;

    //动态生成button的父物体
    public Transform content;

    public ScrollViewController scrollcontroller;

    //切换手自动Toggle
    public Toggle ChoiseCmera;

    //安装进度slider
    public Slider m_installspeed;

    //退出
    public Button Exit;

	//全局最慢安装速度
	public float SlowSpeed =0.2f;

	public override void Awake()
	{
		base.Awake ();
        DisableBut(StepStates.shoutup);

		gamamanager = GetComponent<Gamemanager>();
        MapToggle.onValueChanged.AddListener(delegate 
        {
            MapController.GetInstance.PlayMap(MapToggle.isOn, gamamanager.Step,gamamanager.lastmodel,WotchMapToggle);
        });

        ChoiseCmera.onValueChanged.AddListener(delegate
        {
            CameraBess.GetInstance.ChoiseCamra(ChoiseCmera.isOn);
            ChoiseCmera.transform.Find("Background").GetComponent<Image>().enabled = !ChoiseCmera.isOn;
            ChoiseCmera.transform.Find("Image").GetComponent<Image>().enabled = ChoiseCmera.isOn;
        });
        m_installspeed.onValueChanged.AddListener(delegate 
        {
            SlowSpeed = m_installspeed.value;
        });
        Exit.onClick.AddListener(delegate { Application.Quit(); });
	}


    public void CleanButtons() 
    {
        buttons.Clear();
    }

	public void SetDelegate(UIEvents ue,String _name)
	{
        RefreshTitle(_name);
		Previous.onClick.RemoveAllListeners ();
		Next.onClick.RemoveAllListeners ();
		Previous.onClick.AddListener(delegate 
			{
				ue.SetStep(-1,InstallSpeed.slowspeed,true);
				DisableBut(StepStates.shoutup);
				scrollcontroller.Move((float)ue.step/ue.maxStep);
			});
		Next.onClick.AddListener(delegate 
			{ 
				ue.SetStep(1,InstallSpeed.slowspeed,true);
				DisableBut(StepStates.shoutup);
				scrollcontroller.Move((float)ue.step/ue.maxStep);
			});
        DisableBut(StepStates.uptop);
	}


    void WotchMapToggle(bool isactive) 
    {
        MapToggle.isOn = isactive;
    }

	public void SetOtherButtonDisable(Button self = null)
	{
        DisableBut(StepStates.shoutup);
        for (int i = 0; i < buttons.Count; i++)
        {
            if (self != null && !self.Equals(buttons[i]))
            {
                buttons[i].interactable = false;
            }
            else if (self == null)
            {
                buttons[i].interactable = true;
            }
        }
	}


	public void SetOtherButtonDisable(bool isalldisable)
	{
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
	}

    public void RefreshInfo(bool isactive, string name = "", string info = "")
	{
        //Debug.Log("isactive:::" + isactive + "name::::"+name+ "info:::::"+ info);
		note.gameObject.SetActive (isactive);
		if (!isactive) 
		{
			return;
		}
        note.transform.Find("Image/Name").GetComponentInChildren<Text>().text = name;
        note.transform.Find("Note/Note").GetComponentInChildren<Text>().text = info;
       // note.transform.localPosition =pos ;
	}

    public void RefreshTitle(string title) 
    {
        Title.GetComponentInChildren<Text>().text = "SA1400A机器人（" + title + ")";
        
    }



    public void DisableBut(StepStates ss) 
    {
        switch (ss)
        {
		case StepStates.uptop:
			Next.interactable = false;
			Previous.interactable = true;
                break;
		case StepStates.downdeep:
			Previous.interactable = false;
			Next.interactable = true;
                break;
		case StepStates.shoutup:
			Previous.interactable = false;
			Next.interactable = false;
				break;
        default:
            Previous.interactable = true;
            Next.interactable = true;
                break;
        }
    }

}
