using UnityEngine;
using System.Collections;

/// <summary>
/// collider控制器
/// </summary>
public class ClliderController : MonoBehaviour 
{
    public Collider[] last;
    public Gamemanager gamemanager;



    //gos[8]为SA1400总成
    void Awake() 
    {
        gamemanager = GetComponent<Gamemanager>();
      //  AliveCollider(new BoxCollider[1]{ gamemanager.gos[15].GetComponent<BoxCollider>()});
        //给SA1400赋值:
//        gamemanager.gos[8].GetComponent<RealObj>().my = new MyTransform();
//        gamemanager.gos[8].GetComponent<RealObj>().my.name = "SA1400";
//        gamemanager.gos[8].GetComponent<RealObj>().my.info = "总成";
    }




    /// <summary>
    /// 全局collider控制,将上一步打开的boxcollider关闭,打开将要用到的boxcollider
    /// </summary>
    /// <param name="colliders"></param>
    public void AliveCollider(Collider[] colliders) 
    {
        for (int i = 0; i < last.Length; i++)
        {
            last[i].enabled = false;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log(colliders[i].name);
            colliders[i].enabled = true;
        }
        last = colliders;
    }
	
}
