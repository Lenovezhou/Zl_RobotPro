using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T instance;
    public static T GetInstance 
    {
        get{return instance; }
    }
		
    public virtual void Awake() 
    {
		instance =this as T;
    }
		
    public virtual void OnDestroy() 
    {
        if (instance != null)
        {
            instance = null;
        }
    }
}
