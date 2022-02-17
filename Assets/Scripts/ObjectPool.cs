using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Util;

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    public List<T> Pool { get; private set; } = new List<T>();

    public Transform poolHolder;

    /// <summary>
    /// 새로운 객체를 풀에 등록할때 사용하는 함수
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
    /// 객체를 다시 풀에 담는 함수
    /// </summary>
    /// <param name="obj"></param>
    public void PoolReturn(T obj)
    {
        obj.transform.SetParent(poolHolder);
        obj.gameObject.SetActive(false);
    }

    // TODO 2/17 여기서 부터 만들기
    // 오브젝트 풀링 다시 고치기!!
    //public T GetObject()
    //{
    //    // 풀에서 재사용이 가능한 객체를 찾는다
    //    if (!Pool.Find(obj => obj.CanRecycle == true))
    //        return null;
    //    else
    //    {
    //
    //    }
    //
    //}


}
