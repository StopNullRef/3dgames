using Project.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{

    public T ReourceLoad<T>(string path) where T : UnityEngine.Object
    {
        var type = typeof(T);
        return Resources.Load<T>(path);
    }

    public void LoadPoolableObject<T>(string path,int count = 1) where T : MonoBehaviour, IPoolableObject
    {
        var obj = Resources.Load<GameObject>(path);

        var tComponent = obj.GetComponent<T>();

        PoolManager.Instance.PoolDictRigist<T>(tComponent, count);
    }
}
