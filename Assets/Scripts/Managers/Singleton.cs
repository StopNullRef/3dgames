using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    // �ٸ� �����忡�� ����ϴ��� Ȯ���� ��ü
    private static object obj = new object();

    private static T instance;

    /// <summary>
    /// �ܺο��� �����ϰ��� ������Ƽ
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
