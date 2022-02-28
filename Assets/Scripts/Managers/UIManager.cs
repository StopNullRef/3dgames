using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Project.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    public GameObject invenUI; // 인벤토리 UI
    public GameObject invenMover; // 인벤토리 옮겨주는 부분

    bool invenActive = false; //인벤 활성화 체크용 불타입 변수

    Transform invenHolder;

    public SceneDropDown dropdown; // 드롭다운 받아줄거

    public InvenPopUp invenPopUp; // 인벤토리 팝업 UI

    /// <summary>
    /// 현재 열려 있는 모든 UI들을 담을 리스트
    /// </summary>
    public List<UIBase> totalOpenUIList = new List<UIBase>();

    /// <summary>
    /// 모든 UI들을 담을 리스트
    /// </summary>
    public List<UIBase> totalUIList = new List<UIBase>();

    /// <summary>
    /// 모든 ui를 담을 딕셔너리
    /// </summary>
    private Dictionary<string, UIBase> totalUIDict = new Dictionary<string, UIBase>();

    /// <summary>
    /// 업데이트를 돌려줄 UI리스트
    /// </summary>
    private List<UIBase> updateUIList = new List<UIBase>();

    /// <summary>
    /// 하이에라키에 있는 캔버스들을 담을 배열
    /// </summary>
    private Canvas[] canvasArr;

    /// <summary>
    /// 인벤토리 컴포넌트
    /// </summary>
    public InventoryHandler inven;

    protected override void Awake()
    {
        UIInitialize();
    }

    private void Start()
    {
        dropdown = GetUI<SceneDropDown>();
    }

    public void Update()
    {
        //InvenOnOff();

        if (updateUIList.Count > 0)
        {
            foreach (var ui in updateUIList)
            {
                ui.OnUpate();
            }
        }
    }


    // public으로 열어서 에디터 창에서 넣어주게 될때
    // 씬이 바뀌면 missing 이 뜨게 됨으로 일일이 넣어줘야 될듯하다.
    // TODO 일일이 넣어주는거 말고 각각 start에서 uimanager로 등록하게 전부 바꾸기
    public void UIInitialize()
    {


        if (Define.Scene.LoadingScene != (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            canvasArr = GameObject.FindObjectsOfType<Canvas>(true);

            // 0 인벤 1 인벤 무브

            invenHolder = canvasArr.Where(_ => _.CompareTag("InGameCanvas")).SingleOrDefault().transform.GetChild(1);

            //invenHolder = GameObject.Find(Define.FindDataString.ingameUI).transform.GetChild(1);
            //dropdown = invenHolder.parent.GetChild(0).GetComponent<SceneDropDown>();
            //dropdown.Init();
            inven = GameObject.FindObjectOfType<InventoryHandler>(true);
            invenPopUp = GameObject.FindObjectOfType<InvenPopUp>(true);
        }
    }

    /// <summary>
    /// 인벤토리 온오프 기능
    /// </summary>
    void InvenOnOff()
    {
        if (SceneManager.Instance.sceneName == Define.Scene.LoadingScene)
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            invenActive = !invenActive;
            invenHolder.gameObject.SetActive(invenActive);
            IngameManager.Instance.canCusorChange = !IngameManager.Instance.canCusorChange;
        }
    }

    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        if (GameObject.FindObjectOfType<Canvas>() == null)
        {
            //Canvas canvas = new Canvas { name = "InGameUICanvas" };
            //Canvas canvas = Resources.Load<Canvas>("Prefabs/InGameUICanvas.prefab");
            GameObject.Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/InGameUICanvas.prefab"), null, false);
        }

        //UIInitialize();
    }

    /// <summary>
    /// 카메라 상태에 따라 UI를 바꿔줄 함수
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
        // 처음에 모든 ui를 꺼준다
        foreach (var ui in totalUIList)
        {
            ui.Close();
        }

        switch (cameraState)
        {
            case Define.CameraState.Build:
                List<UIBase> buildUI = totalUIList.Where(_ => _.type == Define.UIType.Building).ToList();
                foreach (var ui in buildUI)
                    ui.Open();
                break;
            case Define.CameraState.None:
                List<UIBase> noneUI = totalUIList.Where(_ => _.type == Define.UIType.None).ToList();
                foreach (var ui in noneUI)
                    ui.Open();
                break;
        }
    }

    /// <summary>
    /// UIManager에 등록하는 함수
    /// </summary>
    public void RegistUI(UIBase ui)
    {
        var key = ui.GetType().Name;

        bool hasKey = false;

        // 딕셔너리와 리스트에 등록되어있다면
        if (totalUIDict.ContainsKey(key) || totalUIList.Contains(ui))
        {
            // 등록이 되어있는데 널이아니라면 이미 등록 되어 있다는 것 임으로
            // 등록을 시킬필요가없으므로 리턴
            if (totalUIDict[key] != null)
                return;
            else
            {
                hasKey = true;

                for (int i = 0; i < totalUIList.Count; i++)
                {
                    if (totalUIList[i] == null)
                    {
                        totalUIList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        totalUIList.Add(ui);

        if (hasKey)
            totalUIDict[key] = ui;
        else
        {
            totalUIDict.Add(key, ui);
        }

    }

    public void RigistUpdate(UIBase ui)
    {
        var type = ui.GetType().Name;
        // 같은타입이 없다면 등록 있다면 이미있으므로 return
        if (updateUIList.Find(_ => _.GetType().Name == type) == null)
            updateUIList.Add(ui);
        else
            return;
    }

    /// <summary>
    /// 열려있는 UI리스트 등록해주는 함수
    /// </summary>
    /// <param name="ui"></param>
    public void RegistOpenUI(UIBase ui)
    {
        if (ui.isOpen)
            totalOpenUIList.Add(ui);
    }

    /// <summary>
    /// 열려있는 UI리스트에 지워주는 함수
    /// </summary>
    public void RemoveOpenUI(UIBase ui)
    {
        if (totalOpenUIList.Contains(ui))
            totalOpenUIList.Remove(ui);
    }


    // getui에 버그가 존재 조건에 따라서 추가해서 찾는 함수 구현하기
    public T GetUI<T>(Predicate<UIBase> predicate = null) where T : UIBase
    {
        T result = null;
        var typeName = typeof(T).Name;

        if(predicate != null)
        {
            //TODO 여기부분 추가하기
            return totalUIList.Find(predicate) as T;
        }

        if (totalUIDict.ContainsKey(typeName))
        {
            result = totalUIDict[typeName] as T;
        }

        return result;
    }

    /// <summary>
    /// 열려있는 UI창중 제일 위에있는 UI를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public UIBase GetTopOpenUI()
    {
        UIBase result = null;

        for (int i = totalOpenUIList.Count - 1; i <= 0; i--)
        {
            if (totalOpenUIList != null)
            {
                result = totalOpenUIList[i];
            }
        }

        return result;
    }
}