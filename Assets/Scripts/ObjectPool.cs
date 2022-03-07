using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Util;

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    public List<T> Pool { get; private set; } = new List<T>();

    /// <summary>
    /// ���̿���Ű�󿡼� �ش� ������Ʈ�� ������ �θ�
    /// </summary>
    public Transform poolHolder;

    /// <summary>
    /// ���ο� ��ü�� Ǯ�� ����Ҷ� ����ϴ� �Լ�
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
    /// ��ü�� �ٽ� Ǯ�� ��� �Լ�
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
        // Ǯ���� ������ ������ ��ü�� ã�´�
        if (!Pool.Find(obj => obj.CanRecycle))
        {
            // ������ �Ұ��������� ���� ������ ��ü�� �ִ��� üũ
            if (Pool.Count > 0 && Pool[0] != null)
            {
                // �ִٸ� �װɱ������� ������ Ǯ�� �־���
                var protoObject = Pool[0];
                var result = GameObject.Instantiate(protoObject, poolHolder);
                result.name = protoObject.name;
                RegistPool(result.GetComponent<T>());
            }
            else
                // ���� ��� null�� ��ȯ
                return null;
        }
        // �ٽ� Ǯ���� ã�´�
        var obj = Pool.Find(_ => _.CanRecycle == true);
        obj.CanRecycle = false;
        // ã�� ��ü�� null�̶�� null�� ��ȯ
        if (obj == null)
            return null;

        return obj;
    }
}
