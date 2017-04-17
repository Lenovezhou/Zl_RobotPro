using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;


public class Parts  :MonoBehaviour{

    public string Name;

    public string XmlPath;

    public bool isCanHit = false;

    public Vector3 Offset = Vector3.one;

    private MeshRenderer[] rendermeshs;
    private Color[] selfmeshcolors;

    void Start() 
    {
        rendermeshs = GetComponentsInChildren<MeshRenderer>();
        selfmeshcolors = new Color[rendermeshs.Length];


        for (int i = 0; i < rendermeshs.Length; i++)
        {
            selfmeshcolors[i] = rendermeshs[i].material.color;
        }

    }
    public void OnMouseDown() 
    {
        if (isCanHit)
        {
           // ObjectPool.GetInstance.UnspawnAll();
           // Gamemanager.GetInstance.ChoesXml(Const.XmlPath, name, transform.position);
            CameraBess.GetInstance.ChoiseCamera(false, new PathObjects(transform.position, transform.eulerAngles));
        }
    }

    


    //void OnMouseExit()
    //{
    //    for (int i = 0; i < rendermeshs.Length; i++)
    //    {
    //        Debug.Log(rendermeshs.Length +"<><><>"+selfmeshcolors.Length);
    //        rendermeshs[i].material.color = selfmeshcolors[i];
    //    }
    //    UImanager.GetInstance.RefreshInfo(false);
    //}

    void Update() 
    {
    }

}
