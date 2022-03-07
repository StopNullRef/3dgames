using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Util;

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    public List<T> Pool { get; private set; } = new List<T>();

    /// <summary>
    /// 하이에라키상에서 해당 오브젝트의 정리용 부모
    /// </summary>
    public Transform poolHolder;

    /// <summary>
    /// 새로운 객체를 풀에 등록할때 사용하는 함수
    /// </summary>
    /// <param name="obj"></param>
    public void RegistPool(T obj)
    {
        obj.transform.SetParent(poolHolder);
        obj.gameObject.SetActive(false);
        obj.CanRecycle = true;
        Pool.Add(obj);
    }

    /// <summary>
    /// 객체를 다시 풀에 담는 함수
    /// </summary>
    /// <param name="obj"></param>
    public void PoolReturn(T obj)
    {
        obj.transform.SetParent(poolHolder);
        obj.CanRecycle = true;
        obj.gameObject.SetActive(false);
    }


    public T GetObject()
    {
        // 풀에서 재사용이 가능한 객체를 찾는다
        if (!Pool.Find(obj => obj.CanRecycle))
        {
            // 재사용은 불가능하지만 같은 형식의 객체가 있는지 체크
            if (Pool.Count > 0 && Pool[0] != null)
            {
                // 있다면 그걸기준으로 생성후 풀에 넣어줌
                var protoObject = Pool[0];
                var result = GameObject.Instantiate(protoObject, poolHolder);
                result.name = protoObject.name;
                RegistPool(result.GetComponent<T>());
            }
            else
                // 없을 경우 null을 반환
                return null;
        }
        // 다시 풀에서 찾는다
        var obj = Pool.Find(_ => _.CanRecycle == true);
        obj.CanRecycle = false;
        // 찾는 객체가 null이라면 null을 반환
        if (obj == null)
            return null;

        return obj;
    }
}
