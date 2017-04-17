using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;


public enum _States 
{
    Spown, SlowDown, Target, Camera
}

    public static class MyXML
{
    public static void CreateXML(string path,Transform trans) 
    {

        Transform[] childtransforms = new Transform[trans.childCount];
        for (int i = 0; i < childtransforms.Length; i++)
        {
            childtransforms[i] = trans.GetChild(i);
        }

        if (!File.Exists(path))
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement root = xmldoc.CreateElement("RobotPro");
            for (int i = 0; i < childtransforms.Length; i++)
            {
                XmlElement Eltransform = xmldoc.CreateElement("Node");
                Eltransform.SetAttribute("name", childtransforms[i].name);
                Eltransform.SetAttribute("id", "0");
                Eltransform.SetAttribute("info", "填充info内容");
                //Eltransform.InnerText = "0/10";

                XmlElement SpownPos = xmldoc.CreateElement("childnode");
                SpownPos.SetAttribute("name", "SpownPos");
                SpownPos.InnerText = childtransforms[i].position.x.ToString() + "," + childtransforms[i].position.y.ToString() + "," + childtransforms[i].position.z.ToString()
                + "/" + childtransforms[i].eulerAngles.x.ToString() + "," + childtransforms[i].eulerAngles.y.ToString() + "," + childtransforms[i].eulerAngles.z.ToString();
                XmlElement SlowDown = xmldoc.CreateElement("childnode");
                SlowDown.SetAttribute("name","SlowDown");
                SlowDown.InnerText = SpownPos.InnerText;
                Eltransform.AppendChild(SpownPos);
                Eltransform.AppendChild(SlowDown);
                root.AppendChild(Eltransform);
            }
            xmldoc.AppendChild(root);

            xmldoc.Save(path);

            Debug.Log("CreateXML OK!!!");
        }
    }

    static bool IsFindChildNode = false;


    static bool isfindStepChildnode = false;
    public static void UpdateStepsXML(string path,string name,int step,int stepmax) 
    {
        //Debug.Log("nowstep::::"+ step);
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        XmlNode root = xmldoc.SelectSingleNode("RobotPro");
        XmlNodeList nodelist = root.ChildNodes;

        //Debug.Log("path::::::"+ path);

        if (nodelist.Count > 0)
        {
            for (int i = 0; i < nodelist.Count; i++)
            {
                if (nodelist[i].Attributes["name"].Value.Equals(name))
                {
                    nodelist[i].InnerText = step.ToString() + "/" + stepmax;
                    isfindStepChildnode = true;
                }
            }
            if (!isfindStepChildnode)
	        {
                XmlElement xe = xmldoc.CreateElement("Node");
                xe.SetAttribute("name", name);
                xe.InnerText = step.ToString() + "/" + stepmax;
                root.AppendChild(xe);
	        }
        }
        else {
            XmlElement xe = xmldoc.CreateElement("Node");
            xe.SetAttribute("name", name);
            xe.InnerText = step.ToString() + "/" + stepmax;
            root.AppendChild(xe);
        }
        xmldoc.Save(path);
        isfindStepChildnode = false;
    }

    public static void UpdateXML(string path, Transform trans,_States _s) 
    {
        if (File.Exists(path))
        {
            //如果不包含则创建
            ContainsNode(path,trans,_s);
        }
    }



    static bool IsFindNode = false;
    public static void ContainsNode(string path,Transform trans,_States _s)
    {
        Caching.CleanCache();

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        XmlNode root = xmldoc.SelectSingleNode("RobotPro");
        XmlNodeList nodelist = root.ChildNodes;
        for (int i = 0; i < nodelist.Count; i++)
        {
            //找到点击物体对应的xml节点
            if (nodelist[i].Attributes["name"].Value.Equals(trans.name))
            {
                IsFindNode = true;
            }
        }

        if (!IsFindNode)
        {
            XmlElement xe = xmldoc.CreateElement("Node");
            xe.SetAttribute("id", "0");
            xe.SetAttribute("info","填充info信息........");
            xe.SetAttribute("name", trans.name);
            root.AppendChild(xe);
        }
        //RobotPro节点下的所有子节点
        if (nodelist.Count > 0)
        {
            for (int i = 0; i < nodelist.Count; i++)
            {
                //找到点击物体对应的xml节点
                if (nodelist[i].Attributes["name"].Value.Equals(trans.name))
                {
                    XmlNodeList childnode = nodelist[i].ChildNodes;
                    //遍历该节点下的子节点
                    if (childnode.Count > 0)
                    {
                        for (int j = 0; j < childnode.Count; j++)
                        {
                            Debug.Log(_s.ToString());
                            if (childnode[j].Attributes["name"].Value.Equals(_s.ToString()))
                            {
                                WriteDefult(_s, childnode[j], trans);
                                IsFindChildNode = true;
                            }
                        }
                        if (!IsFindChildNode)
                        {
                            XmlElement xe = xmldoc.CreateElement("childnode");
                            xe.SetAttribute("name", _s.ToString());
                            WriteDefult(_s, xe, trans);
                            nodelist[i].AppendChild(xe);
                           // root.AppendChild(nodelist[i]);
                        }
                    }
                    else
                    {
                        XmlElement xe = xmldoc.CreateElement("childnode");
                        xe.SetAttribute("name",_s.ToString());
                        WriteDefult(_s,xe, trans);
                        nodelist[i].AppendChild(xe);
                    }

                }
            }
        }
        else
        {
            Debug.Log(trans.position.ToString() + trans.position.x);

            XmlElement xx = xmldoc.CreateElement("Node");
            xx.SetAttribute("name", trans.name);
            XmlElement xe = xmldoc.CreateElement(_s.ToString());
            xe.InnerText = trans.position.ToString() + "/" + trans.localEulerAngles.ToString();
            xx.AppendChild(xe);
            root.AppendChild(xx);
        }
        xmldoc.Save(path);
        IsFindChildNode = false;
        IsFindNode = false;
    }

    public static void WriteDefult(_States _s , XmlNode xn,Transform trans) 
    {
        if (_s.Equals(_States.Camera))
        {
            xn.InnerText = Camera.main.transform.position.x.ToString() +","+Camera.main.transform.position.y+","+Camera.main.transform.position.z.ToString()+
                "/" + Camera.main.transform.eulerAngles.x.ToString() + "," + Camera.main.transform.eulerAngles.y.ToString() + "," + Camera.main.transform.parent.eulerAngles.z.ToString();
        }
        else
        {
            xn.InnerText = trans.position.x.ToString() + "," + trans.position.y.ToString() + "," + trans.position.z.ToString()
                + "/" + trans.eulerAngles.x.ToString() + "," + trans.eulerAngles.y.ToString() + "," + trans.eulerAngles.z.ToString();
        }
        
    }


    public static Dictionary<string, MyTransform> ReaderXml(string path)
    {
        Dictionary<string, MyTransform> itemstransform = new Dictionary<string, MyTransform>();
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        XmlNode xn = xmldoc.SelectSingleNode("RobotPro");
        for (int i = xn.ChildNodes.Count - 1; i >= 0 ; i--)
        {
            MyTransform my = new MyTransform();
            string str = xn.ChildNodes[i].Attributes["name"].Value;
            my.name = str;
            my.id = int.Parse(xn.ChildNodes[i].Attributes["id"].Value);
            my.info = xn.ChildNodes[i].Attributes["info"].Value;
            XmlNodeList list = xn.ChildNodes[i].ChildNodes;
            for (int j = 0; j <list.Count; j++)
            {
                string[] single = list[j].InnerText.Split('/');
                string[] pos = single[0].Split(',');
                string[] rot = single[1].Split(',');
                Vector3 p = Vector3.zero;
                p.x = float.Parse(pos[0]);
                p.y = float.Parse(pos[1]);
                p.z = float.Parse(pos[2]);
                Vector3 r = Vector3.zero;
                r.x = float.Parse(rot[0]);
                r.y = float.Parse(rot[1]);
                r.z = float.Parse(rot[2]);
                switch (list[j].Attributes["name"].Value)
                {
                    case "Spown":
                        my.Spownpos = p;
                        my.Spownrot = r;
                        break;
                    case "SlowDown":
                        my.SlowDownpos = p;
                        my.SlowDownrot = r;
                        break;
                    case "Target":
                        my.Targetpos = p;
                        my.Targetrot = r;
                        break;
                    case "Camera":
                        my.Camerapos = p;
                        my.Camerarot = r;
                        break;
                    default:
                        break;
                }
            }
            itemstransform.Add(str, my);
        }
        return itemstransform;
    }

    public static Dictionary<string, List<int>> ReaderStepXml(string path) 
    {

        Dictionary<string, List<int>> temp = new Dictionary<string, List<int>>();

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        XmlNode xn = xmldoc.SelectSingleNode("RobotPro");
        for (int i = 0; i < xn.ChildNodes.Count; i++) 
        {
            //Debug.Log("name:::::" + xn.ChildNodes[i].Attributes["name"].Value);

            int id = int.Parse(xn.ChildNodes[i].Attributes["id"].Value);
            int currentstep = int.Parse(xn.ChildNodes[i].InnerText.Split('/')[0]);
            int maxstep = int.Parse(xn.ChildNodes[i].InnerText.Split('/')[1]);
            temp.Add(xn.ChildNodes[i].Attributes["name"].Value, new List<int>() { currentstep,maxstep,id});
        }

        return temp;
    }

}
