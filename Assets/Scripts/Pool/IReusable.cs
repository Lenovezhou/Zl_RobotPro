using UnityEngine;
using System.Collections;

public interface IReusable  
{
    //取出时调用
    void OnSpawn();
    //回收时调用
    void OnUnspawn();
}
