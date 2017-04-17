using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelfButtonController :ReusbleObject
{
    public int selfstep;

	public Button bu;

	public Action<Button,int> callback;

	public void Init(int _step,string _name,Action<Button,int> call) 
    {
        if (_step == 1)
        {
            if (transform.Find("Line"))
            {
                Destroy(transform.Find("Line").gameObject);
            }
        }
        else {
            selfstep = _step;
        }
        transform.Find("Text").GetComponent<Text>().text = (_step).ToString();
        transform.Find("Label").GetComponent<Text>().text = _name;
		callback = call;
		if (_step == 1) {
		//	call (bu,0);
		}
    }

    public void Start() 
    {
        bu = GetComponent<Button>();
        bu.onClick.AddListener(delegate 
			{ 
                //Debug.Log("按下！！！！！"+ callback == null);
				if (callback != null) {
					callback(bu,selfstep);
				}
			});
    }



    public override void OnSpawn()
    {
        
    }

    public override void OnUnspawn()
    {
    }
}
