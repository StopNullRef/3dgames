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
    //public SceneDropDown dropdown; // 드롭다운 받아줄거

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
    /// 인벤토리 컴포넌트
    /// </summary>
    public InventoryHandler inven;

    public List<UIBase> beforeOpenUIList = new List<UIBase>();

    private void Start()
    {
        //dropdown = GetUI<SceneDropDown>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GetEscCloseUI()?.Close();
    }


    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        if (GameObject.FindObjectOfType<Canvas>() == null)
            GameObject.Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/InGameUICanvas"), null, false);

        RemoveAllOpenUI();
        RemoveUIInfo();
    }

    /// <summary>
    /// 카메라 상태에 따라 UI를 바꿔줄 함수
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
        // buildinginven 슬롯 이미지 초기화했는데 안뜸 백그라운드 이미지만 보임 이거 보이게 고치기


        //카메라 상태에 따라 UI를 켰다가 꺼줄 것임으로 카메라 상태가 다시 돌아왔을때 다시 켜줄 리스트를 만듦
        if (beforeOpenUIList.Count < 1)
            beforeOpenUIList = totalUIList.Where(_ => _.isOpen == true && _.type != Define.UIType.Building).ToList();

        // 모든 열려있는 ui를 꺼준다
        foreach (var ui in beforeOpenUIList)
            ui.Close();

        // 건축에 필요한 UI들을 UIManager에서 찾아서 가져온다
        List<UIBase> buildUI = totalUIList.Where(_ => _.type == Define.UIType.Building).ToList();
        switch (cameraState)
        {
            //카메라가 build상태일때 UIManager에 접근하여 buildingUI를 가지고 와 열어준다
            case Define.CameraState.Build:
                {
                    foreach (var ui in buildUI)
                        ui.Open();
                }
                break;
            case Define.CameraState.None:
                {
                    // 카메라가 빌드 상태가 아닐때는 buildingUI들을 전부 꺼준다
                    foreach (var ui in buildUI)
                        ui.Close();
                    // 그전에 열려있던 UI들을 다시 켜줌
                    foreach (var ui in beforeOpenUIList)
                        ui.Open();
                    beforeOpenUIList.Clear();
                }
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

    /// <summary>
    /// 열려있는 UI리스트를 전부 초기화 시키는함수
    /// </summary>
    public void RemoveAllOpenUI()
    {
        totalOpenUIList.Clear();
    }

    /// <summary>
    /// uidictionary 와 uiList 정보를
    /// 전부 지워주는 함수
    /// </summary>
    public void RemoveUIInfo()
    {
        totalUIDict.Clear();
        totalUIList.Clear();
    }


    // getui에 버그가 존재 조건에 따라서 추가해서 찾는 함수 구현하기
    public T GetUI<T>() where T : UIBase
    {
        T result = null;
        var typeName = typeof(T).Name;


        if (totalUIDict.ContainsKey(typeName))
        {
            result = totalUIDict[typeName] as T;
        }

        return result;
    }

    public T GetUI<T>(string moverName) where T : UIBase
    {
        T result = null;
        var typeName = typeof(T).Name;

        if (moverName != null)
        {
            return totalUIList.Where(_ => _.gameObject.name == moverName + "Mover").SingleOrDefault() as T;
        }

        return result;
    }



    /// <summary>
    /// 열려있는 UI창중 제일 위에있는 UI를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public UIBase GetTopOpenUI()
    {

        for (int i = totalOpenUIList.Count - 1; i >= 0; i--)
        {
            if (totalOpenUIList[i] != null)
                return totalOpenUIList[i];
        }

        return null;
    }

    /// <summary>
    /// 열려있는  ui중에서 제일 위에있으며
    /// esc로 닫을수 있는 ui를 찾아주는 함수
    /// </summary>
    /// <returns></returns>
    public UIBase GetEscCloseUI()
    {
        for (int i = totalOpenUIList.Count - 1; i >= 0; i--)
        {
            if (totalOpenUIList[i] != null && (totalOpenUIList[i].isEscClose == true))
                return totalOpenUIList[i];
        }

        return null;
    }

}