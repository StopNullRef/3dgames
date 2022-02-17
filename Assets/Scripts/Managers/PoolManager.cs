using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public List<GameObject> poolList = new List<GameObject>();
    public Transform map;
    GameObject poolHolder;

    public Dictionary<string, object> poolDict = new Dictionary<string, object>();

    /// <summary>
    /// 다시 활성화 시켜주기 위해
    /// 시간을 체크할 변수
    /// </summary>
    float poolTime;

    /// <summary>
    /// 리스폰 시간
    /// </summary>
    const float respawnTime = 10f;

    protected override void Awake()
    {
        base.Awake();
        //Init();
    }

    private void Start()
    {
        // TODO 씬 로드 될때 들어가는 함수들 순서 정해줘야 될듯
        Init();
    }

    public void FixedUpdate()
    {
        if (SceneManager.Instance.sceneName == Define.Scene.LoadingScene || (poolList.Count ==0))
            return;

        poolTime += Time.deltaTime;

        if(poolTime > respawnTime)
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
        foreach(GameObject go in poolList)
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

}
