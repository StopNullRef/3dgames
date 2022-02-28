using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public Define.Scene sceneName = Define.Scene.LoadingScene; // 현제 씬의 이름을 넣어줄 변수

    public int sceneNum; // 로드씬을 해주기위한 변수

    public Transform map; // 하이에라키에서 맵을 가지고있는 GameObejct를 받는 변수

    protected override void Awake()
    {
        base.Awake();
        //대리자로 함수를 넣어서 씬이 바뀔때 호출되게 끔 할수 있다
        Init();
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneChangeInit;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneChangeInit;
    }

    public void Init()
    {
        // 현재 씬 이름을 sceneName 변수에 넣어줌
        sceneName = (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        sceneNum = (int)sceneName;

        if (!(sceneName == (int)Define.Scene.LoadingScene))
        {
            map = GameObject.Find("Maps").transform.GetChild(0);
            ObjectGiveTagandLayer(Define.TagName.Tree, 10);
            ObjectGiveTagandLayer(Define.TagName.Grass, 11);
        }
    }

    public void OnUpdate()
    {

    }

    public void Clear()
    {

    }

    /// <summary>
    /// 씬이 전환될때 다시 초기화 해주는 함수
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mod"></param>
    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        sceneName = (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        // 씬 바뀔때 체크 해주고 넣어주는 부분

        if (sceneName != Define.Scene.LoadingScene)
        {
            //Init();
            map = GameObject.Find("Maps").transform.GetChild(0);
            UIManager.Instance.UIInitialize();
           // UIManager.Instance.dropdown.dropdown.value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
            UIManager.Instance.GetUI<SceneDropDown>().dropdown.value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        }

    }

    // 맵에 tag 와 layer 넣어주는 함수
    // map에 있는 GameObject 의 앞글자부터 검사를 해서 같으면 tag와 해당 object의 자식에게 layer 를 넣어줌
    // 자식에 넣는 이유는 맵 에디터가 해당 게임오브젝트 자식쪽에서 collider를 가지고 있어서
    public void ObjectGiveTagandLayer(Define.TagName tagName, int layer) // 검사할 이름, 넣어줄 layer
    {
        // 처음부터 넣어주는 것이 Best 이지만 맵에디터로 만들어서 일일이 넣어 주기가 힘들어서 코드로 넣었다
        // 별로 좋은 방법은 아님

        int count = 0;

        if (map.childCount == 0)
        {
            return;
        }
        string objName = tagName.ToString();

        Transform go;

        for (int j = 0; j < map.childCount; j++)
        {
            go = map.GetChild(j);
            for (int i = 0; i < objName.Length; i++)
            {
                if (go.name[i].ToString() == objName[i].ToString())
                {
                    count++;
                    if (count == objName.Length)
                    {
                        go.GetChild(0).gameObject.tag = objName;
                        go.GetChild(0).gameObject.layer = layer;
                    }
                }
            }
            count = 0;
        }
    }

    // 씬인덱스 번호를 받아 씬이름을 한국어로 바꿔주는 함수
    /// <summary>
    ///  씬 인덱스로 씬 DropDown에 option에 들어갈 text string 을 반환 함수
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public string GetSceneName(int i)
    {
        string _name = null;
        //0 로딩
        //1 숲
        //2 광산
        //3 집
        switch (i)
        {
            case 0:
                _name = "로딩";
                break;
            case 1:
                _name = "숲";
                break;
            case 2:
                _name = "광산";
                break;
            case 3:
                _name = "집";
                break;
            default:
                break;
        }

        return _name;
    }

    public void ObjectAddComponent()
    {
        for (int i = 0; i < map.childCount; i++)
        {
            if (map.GetChild(i).GetComponent<ObjInfo>() == null)
            {
                map.GetChild(i).GetChild(0).gameObject.AddComponent<ObjInfo>();
            }
        }
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneChangeInit;
    }

}
