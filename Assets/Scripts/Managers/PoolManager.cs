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
    /// 다시 활성화 시켜주기 위해
    /// 시간을 체크할 변수
    /// </summary>
    float poolTime;

    /// <summary>
    /// 리스폰 시간
    /// </summary>
    const float respawnTime = 10f;

    //TODO 02/22 respone은 풀매니저에서 말고 매니저 따로만들어서 관리하기

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
    /// 오브젝트 풀 매니저에 딕셔너리에 등록하는 함수
    /// </summary>
    /// <typeparam name="T">넣을 오브젝트 타입</typeparam>
    /// <param name="obj">넣을 오브젝트</param>
    /// <param name="count">넣을 횟수</param>
    public void PoolDictRigist<T>(T obj, int count) where T : MonoBehaviour, IPoolableObject
    {
        ObjectPool<T> pool = null;
        var key = typeof(T);

        // 풀딕트에서 해당 타입의 키가존재하는지 체크
        if (poolDict.ContainsKey(key))
            // 존재한다면 풀딕트에서 가져와 캐스팅해서 넣어준다
            pool = poolDict[key] as ObjectPool<T>;
        else
        {
            // 존재하지 않는다면 생성해서 추가
            pool = new ObjectPool<T>();
            poolDict.Add(key, pool);
        }

        // 풀홀더가 null이라면 생성해서 넣어준다
        if(pool.poolHolder == null)
        {
            pool.poolHolder = new GameObject { name = $"{key}Holder" }.transform;
            pool.poolHolder.parent = transform;
            pool.poolHolder.position = Vector3.zero;
        }

        // 매개 변수로 받은 count 만큼 생성후 풀에 등록
        for(int i=0; i< count; i++)
        {
            var poolObj = Instantiate(obj);
            poolObj.name = obj.name;

            pool.RegistPool(poolObj);
        }
    }

}
