using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ObjectPool :Singleton<ObjectPool>
{
    public string ResourcDir = "";
    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();


    public override void Awake()
    {
        base.Awake();
    }
    //取对象

    public GameObject Spawn(string name) 
    {
        if (!m_pools.ContainsKey(name))
            Rigister(name);
        SubPool pool = m_pools[name];
        return pool.Spawn();
    }


    //回收对象

    public void Unspawn(GameObject go) 
    {
        foreach (SubPool item in m_pools.Values)
        {
            if (item.Contains(go))
            {
                item.Unspawn(go);
                break;
            }
        }
    }
    //回收所有对象
    public void UnspawnAll() 
    {
        foreach (SubPool item in m_pools.Values) 
        {
            item.UnSpawnAll();
        }
    }

    //创建新子池子
    void Rigister(string name) 
    {
        //预设路径
        string path = "";
        if (string.IsNullOrEmpty(ResourcDir))
        {
            path = name;
        }
        else {
            path = ResourcDir + "/" + name;
        }
        //加载预设
        GameObject prefab = Resources.Load<GameObject>(path);
        //创建子对象池
        SubPool pool = new SubPool(prefab);
        m_pools.Add(prefab.name, pool);
    }

}
