using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MyEditor : Editor
{
    [MenuItem("Tools/Begine Create MainFiles")]
    static void CreateFiles() 
    {
        Directory.CreateDirectory(Application.dataPath + "/Scenes");
        Directory.CreateDirectory(Application.dataPath + "/Materials");
        Directory.CreateDirectory(Application.dataPath + "/Prefab");
        Directory.CreateDirectory(Application.dataPath + "/Game");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Application");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Application/1.Model");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Application/2.View");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Application/1.Controllers");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Framework");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Framework/Pool");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Framework/Singleton");
        Directory.CreateDirectory(Application.dataPath + "/Game/Scripts/Framework/Sound");
        Directory.CreateDirectory(Application.dataPath + "/Prefab");
    }

    //[MenuItem("Tools/XML /CreateXML")]
    //static void Creat() 
    //{
    //    GameObject obj = Selection.activeGameObject;
    //    string temppath = Application.dataPath + "/XML/MiniParts/" + obj.name + ".xml";
    //    MyXML.CreateXML(temppath, obj.transform);
    //  //  MyXML.CreateXML(Const.SA1400, trans);
    //}


     [MenuItem("Tools/XML /Write Spown info")]

    static void Spown()
     {
		MyXML.UpdateXML(Const.J5Dynamo, Selection.activeGameObject.GetComponent<Transform>(), _States.Spown);
     }

     [MenuItem("Tools/XML /Write SlowDown info")]
    static void SlowDown()
     {
		MyXML.UpdateXML(Const.J5Dynamo, Selection.activeGameObject.GetComponent<Transform>(), _States.SlowDown);
     }

     [MenuItem("Tools/XML /Write Target info")]
      static void Target()
     {
		MyXML.UpdateXML(Const.J5Dynamo, Selection.activeGameObject.GetComponent<Transform>(), _States.Target);
     }

     [MenuItem("Tools/XML /Write Camera info")]
     static void Camera()
     {
		MyXML.UpdateXML(Const.J5Dynamo, Selection.activeGameObject.GetComponent<Transform>(), _States.Camera);
     }
}
