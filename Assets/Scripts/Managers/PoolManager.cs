using Project.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public List<GameObject> poolList = new List<GameObject>();
    public Transform map;
    GameObject poolHolder;

    public Dictionary<Type, object> poolDict = new Dictionary<Type, object>();

    /// <summary>
    /// �ٽ� Ȱ��ȭ �����ֱ� ����
    /// �ð��� üũ�� ����
    /// </summary>
    float poolTime;

    /// <summary>
    /// ������ �ð�
    /// </summary>
    const float respawnTime = 10f;

    //TODO 02/22 respone�� Ǯ�Ŵ������� ���� �Ŵ��� ���θ��� �����ϱ�

    protected override void Awake()
    {
        base.Awake();
        //Init();
    }

    private void Start()
    {
        Init();
    }

    public void FixedUpdate()
    {
        if (SceneManager.Instance.sceneName == Define.Scene.LoadingScene || (poolList.Count == 0))
            return;

        poolTime += Time.deltaTime;

        if (poolTime > respawnTime)
        {
            PoolListRemove();
            poolTime = 0;
        }

    }


    void Init()
    {
        map = SceneManager.Instance.map;
        if (GameObject.Find("PoolHolder") == null)
        {
            GameObject go = new GameObject { name = "PoolHolder" };
            poolHolder = go;
        }
    }

    public void Init(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (SceneManager.Instance.sceneName == Define.Scene.LoadingScene)
            return;

        map = SceneManager.Instance.map;
        if (GameObject.Find("PoolHolder") == null)
        {
            GameObject go = new GameObject { name = "PoolHolder" };
            poolHolder = go;
        }
    }

    public void PoolListAdd(GameObject poolObject)
    {
        GameObject poolObjParent = poolObject.transform.parent.gameObject;
        poolList.Add(poolObjParent);

        poolObjParent.transform.SetParent(poolHolder.transform);

        poolObjParent.SetActive(false);
    }

    public void PoolListRemove()
    {
        foreach (GameObject go in poolList)
        {
            if (go == null)
            {
                break;
            }
            go.transform.SetParent(map);
            go.SetActive(true);
        }
        poolList.Clear();
    }

    /// <summary>
    /// ������Ʈ Ǯ �Ŵ����� ��ųʸ��� ����ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T">���� ������Ʈ Ÿ��</typeparam>
    /// <param name="obj">���� ������Ʈ</param>
    /// <param name="count">���� Ƚ��</param>
    public void PoolDictRigist<T>(T obj, int count) where T : MonoBehaviour, IPoolableObject
    {
        ObjectPool<T> pool = null;
        var key = typeof(T);

        // Ǯ��Ʈ���� �ش� Ÿ���� Ű�������ϴ��� üũ
        if (poolDict.ContainsKey(key))
            // �����Ѵٸ� Ǯ��Ʈ���� ������ ĳ�����ؼ� �־��ش�
            pool = poolDict[key] as ObjectPool<T>;
        else
        {
            // �������� �ʴ´ٸ� �����ؼ� �߰�
            pool = new ObjectPool<T>();
            poolDict.Add(key, pool);
        }

        // ǮȦ���� null�̶�� �����ؼ� �־��ش�
        if(pool.poolHolder == null)
        {
            pool.poolHolder = new GameObject { name = $"{key}Holder" }.transform;
            pool.poolHolder.parent = transform;
            pool.poolHolder.position = Vector3.zero;
        }

        // �Ű� ������ ���� count ��ŭ ������ Ǯ�� ���
        for(int i=0; i< count; i++)
        {
            var poolObj = Instantiate(obj);
            poolObj.name = obj.name;

            pool.RegistPool(poolObj);
        }
    }

}
