
using System;
using System.Collections.Generic;
using UnityEngine;
using HuangDong.Yu;

public class ModelController:StateMachine
{
	//brief 路径
	public Transform[] wayPoints;
	//brief 当前路径点
	private int currentWayPoint = 0;
	//安装
	private State Excute = new State();
	//拆解
	private State Undo = new State();


	void Start()
	{
//		follow.OnEnter = StartFollow; //跟随路径
//		//follow.OnLeave = StopFollow;//这里没有使用退出状态，可根据需要自行添加
//
//		chase.OnEnter = StartChase; //追逐玩家
//		//chase.OnLeave = StopChase; //这里没有使用退出状态，可根据需要自行添加
//
//		follow.OnUpdate = FollowUpdate; //跟随路径状态
//		chase.OnUpdate = ChaseUpdate; //追逐状态
//
//		state = follow; //初始为跟随路径点移动
	}


}
