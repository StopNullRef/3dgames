using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Util;

public class ObjectPool<T>  where T: MonoBehaviour, IPoolableObject
{
    public List<T> Pool { get; private set; } = new List<T>();

    public Transform poolHolder;

    /// <summary>
    /// ���ο� ��ü�� Ǯ�� ����Ҷ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="obj"></param>
    public void RegistPool(T obj)
    {
        obj.transform.SetParent(poolHolder);
        obj.gameObject.SetActive(false);
        obj.CanRecycle = false;
        Pool.Add(obj);
    }
    
    /// <summary>
    /// ��ü�� �ٽ� Ǯ�� ��� �Լ�
    /// </summary>
    /// <param name="obj"></param>
    public void PoolReturn(T obj)
    {
        obj.transform.SetParent(poolHolder);
        obj.gameObject.SetActive(false);
    }

    // TODO 12/29 ���⼭ ���� �����
    // ������Ʈ Ǯ�� �ٽ� ��ġ��!!
    //public T GetObject(Func<T,bool> prev = null)
    //{
    //
    //}


}