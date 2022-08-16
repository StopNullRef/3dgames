using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Util;
using Project.SD;
using System.Linq;
using Project.Object;

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
        // TODO 08/08여기 수정좀 해야됨
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


    public GameObject GetObject(SDBuildItem buildItem)
    {
        // 풀에서 구별할수 있는 방법??
        // 내가 가지고있는거 staticdata를 들고있음
        // 건물 별로 각각 다른 풀을 갖게한다?
        // 건물 안에 스태틱 데이터가 있는데 그거를 기준으로 꺼내야되는데..
        var result = Pool.Where(_ => _.name == buildItem.name).FirstOrDefault(_ => _.CanRecycle == true);

        if (result == null)
        {
            Debug.Log("result == null");
            ResourceManager.Instance.LoadPoolableObject<BuildItem>(buildItem.resourcePath[1], 1);
            // 다시 찾아서 넣어줌
            result = Pool.Where(_ => _.name == buildItem.name).FirstOrDefault(_ => _.CanRecycle == true);
        }



        result.CanRecycle = false;

        if (result == null)
            return null;


        return result.gameObject;
    }

}
