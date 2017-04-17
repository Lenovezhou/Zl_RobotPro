using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class SubPool
{
    //预设
    GameObject m_prfab;
    //集合
    List<GameObject> m_objects = new List<GameObject>();


    //名字标识

    public string Name 
    {
        get { return m_prfab.name; }
    }

    //构造

    public SubPool(GameObject prefab) 
    {
        this.m_prfab = prefab;
    }

    //取对象

    public GameObject Spawn() 
    {
        GameObject go = null;

        foreach (GameObject item in m_objects)
        {
            if (!item.activeSelf)
            {
                go = item;
                break;
            }
        }
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(m_prfab);
            m_objects.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    //回收对象

    public void Unspawn(GameObject go) 
    {
        if (Contains(go))
        {
            go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    //回收该池的所有对象

    public void UnSpawnAll() 
    {
        foreach (GameObject item in m_objects)
        {
            if (item.activeSelf)
            {
                Unspawn(item);
            }
        }
    }

    //是否包含对象
    public bool Contains(GameObject go) 
    {
        return m_objects.Contains(go);
    }

}

