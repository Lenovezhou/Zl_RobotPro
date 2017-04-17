using UnityEngine;

///@brief
///文件名称:NPCControl
///功能描述:
///数据表:
///作者:YuXianQiang
///日期:#CreateTime#
///R1:
///修改作者:
///修改日期:
///修改理由:

namespace HuangDong.Yu
{
    //NPC的控制类，继承自StateMachine
    //类实现了游戏开始时，NPC开始按规定路线巡逻
    //当视野中出现玩家，则开始追逐玩家
    //玩家和NPC的距离拉开后，NPC自动返回规定路线巡逻
    //这样就实现了状态的切换
    public class NPCControl : StateMachine
    {
        //brief 玩家
        public GameObject player;
        //brief NPC
        public GameObject npc;
        //brief 路径
        public Transform[] wayPoints;
        //brief 当前路径点
        private int currentWayPoint = 0;
        //brief 跟随路径点移动，如果有多种状态，继续添加
        private State follow = new State();
        ///brief 追着玩家跑，如果有多种状态，继续添加
        private State chase = new State();

        void Start()
        {
            follow.OnEnter = StartFollow; //跟随路径
            //follow.OnLeave = StopFollow;//这里没有使用退出状态，可根据需要自行添加

            chase.OnEnter = StartChase; //追逐玩家
            //chase.OnLeave = StopChase; //这里没有使用退出状态，可根据需要自行添加

            follow.OnUpdate = FollowUpdate; //跟随路径状态
            chase.OnUpdate = ChaseUpdate; //追逐状态

            state = follow; //初始为跟随路径点移动
        }

        void Update()
        {
            ///更新状态
            OnUpdateState(Time.deltaTime);
        }

        /// <summary>
        /// brief 满足条件，更换状态
        /// </summary>
        /// <param name="obj"></param>
        private void ChaseUpdate(float obj)
        {
			///如果距离大于30，则为寻路状态，
            if (Vector3.Distance(npc.transform.position, player.transform.position) >= 10)
               ///执行follow.update
				state = follow;
             ///startchase
            chase.OnEnter(); //开始追逐
        }



		Ray ray;
        /// <summary>
        /// brief 满足条件，更换状态
        /// </summary>
        /// <param name="obj"></param>
        private void FollowUpdate(float obj)
        {
			///如果正前方15内有player则为跟随player状态
//            RaycastHit hit;
//            if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
//				{
//                if (hit.transform.gameObject.tag == "Player")
//					{
//					state = chase;
//					}
//				}
			//正前方15米处的球形半径内有player则切换为跟随状态
			ray.origin = npc.transform.position;
			ray.direction = npc.transform.forward;
			RaycastHit[] hits = Physics.SphereCastAll (ray, 5f);
			foreach (var a in hits) 
			{
				if (a.collider.CompareTag("Player")) 
				{
					///执行chase.Onupdate
					state = chase;
				}
			}
			///startfollow
            follow.OnEnter(); //开始跟随
        }

        /// <summary>
        /// brief 追逐玩家移动方法
        /// </summary>
        private void StartChase()
        {
            Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
            Vector3 moveDir = player.transform.position - npc.transform.position;

            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                Quaternion.LookRotation(moveDir), 5*Time.deltaTime);

            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
            vel = moveDir.normalized*10;

            npc.GetComponent<Rigidbody>().velocity = vel;
        }

        /// <summary>
        /// brief 跟随路径点移动的方法
        /// </summary>
        private void StartFollow()
        {
            Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
            Vector3 moveDir = wayPoints[currentWayPoint].position - npc.transform.position;

            if (moveDir.magnitude < 1)
            {
                currentWayPoint++;
                if (currentWayPoint >= wayPoints.Length)
                {
                    currentWayPoint = 0;
                }
            }
            else
            {
                vel = moveDir.normalized*10;
                npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                    Quaternion.LookRotation(moveDir), 5*Time.deltaTime);
                npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
            }
            npc.GetComponent<Rigidbody>().velocity = vel;
        }
    }
}