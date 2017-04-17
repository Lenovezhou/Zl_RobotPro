using UnityEngine;
using System.Collections;

public class ShowMessage : MonoBehaviour {

    private Color oldcolor;
    public Material m_material;
    private MyTransform my;

    //旋转坐组件合并同时反应
    public ShowMessage contect;

    public 

    void Awake() 
    {
        m_material = GetComponent<MeshRenderer>().material;
        oldcolor = m_material.color;
    }


    public void ResetColor() 
    {
        iTween.Pause(gameObject);
        m_material.color = oldcolor;
    }


    public void Init(MyTransform my)
    {
       
        this.my = my;
    }

    void OnMouseEnter()
    {
        
        if (contect)
        {
            contect.HighLight();
        }
        
        m_material.SetColor("_EmissionColor", Color.gray);
        UImanager.GetInstance.RefreshInfo(true, my.name, my.info);
    }


    void OnMouseExit()
    {
        if (contect)
        {
            contect.ResetHighLight();
        }
        UImanager.GetInstance.RefreshInfo(false);
        m_material.SetColor("_EmissionColor", Color.black);
        
    }

    //材质高亮
    public void HighLight() 
    {
        m_material.SetColor("_EmissionColor", Color.gray);
    }

    //恢复材质颜色
    public void ResetHighLight() 
    {
        m_material.SetColor("_EmissionColor", Color.black);
    }

    //材质闪烁
    public void Spark()
    {
        iTween.Resume(gameObject);
        Hashtable hashtabel = new Hashtable();
        //hashtabel.Add("r", 0.1f);
        hashtabel.Add("g", 0.1f);
        //hashtabel.Add("b", 0.1f);
        hashtabel.Add("time", 0.5f);
        //hashtabel.Add("easytype",iTween
        hashtabel.Add("looptype", iTween.LoopType.pingPong);
        iTween.ColorTo(gameObject, hashtabel);        
    }

}
