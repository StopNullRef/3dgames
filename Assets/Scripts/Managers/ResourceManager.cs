using Project.UI;
using Project.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        LoadAllPrefabs();
    }

    private void LoadAllPrefabs()
    {
        LoadPoolableObject<StoreSlot>("Prefabs/UI/SaleSlot", 10);
    }

    public T ReourceLoad<T>(string path) where T : UnityEngine.Object
    {
        var type = typeof(T);
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 오브젝트풀링을 사용하는 객체
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="count"></param>
    public void LoadPoolableObject<T>(string path, int count = 1) where T : MonoBehaviour, IPoolableObject
    {
        if (count == 0)
            return;
        var obj = Resources.Load<GameObject>(path);
        if (obj == null)
            Debug.Log("obj == null");


        var tComponent = obj.GetComponent<T>();

        tComponent.PoolInit();
        

        PoolManager.Instance.PoolDictRigist<T>(tComponent, count);
    }
}
