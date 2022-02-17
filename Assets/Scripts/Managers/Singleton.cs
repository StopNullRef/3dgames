using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    // 다른 쓰레드에서 사용하는지 확인할 객체
    private static object obj = new object();

    private static T instance;

    /// <summary>
    /// 외부에서 접근하게할 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                lock(obj)
                {
                    instance = FindObjectOfType<T>();

                    if(instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
