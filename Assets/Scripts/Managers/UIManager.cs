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
    //public SceneDropDown dropdown; // ��Ӵٿ� �޾��ٰ�

    /// <summary>
    /// ���� ���� �ִ� ��� UI���� ���� ����Ʈ
    /// </summary>
    public List<UIBase> totalOpenUIList = new List<UIBase>();

    /// <summary>
    /// ��� UI���� ���� ����Ʈ
    /// </summary>
    public List<UIBase> totalUIList = new List<UIBase>();

    /// <summary>
    /// ��� ui�� ���� ��ųʸ�
    /// </summary>
    private Dictionary<string, UIBase> totalUIDict = new Dictionary<string, UIBase>();

    /// <summary>
    /// �κ��丮 ������Ʈ
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
            GameObject.Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/InGameUICanvas.prefab"), null, false);

        RemoveAllOpenUI();
        RemoveUIInfo();
    }

    /// <summary>
    /// ī�޶� ���¿� ���� UI�� �ٲ��� �Լ�
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
        beforeOpenUIList ??= totalOpenUIList;

        // ó���� ��� ui�� ���ش�
        foreach (var ui in totalUIList)
            ui.Close();

        switch (cameraState)
        {
            case Define.CameraState.Build:
                List<UIBase> buildUI = totalUIList.Where(_ => _.type == Define.UIType.Building).ToList();
                foreach (var ui in buildUI)
                    ui.Open();
                break;
            case Define.CameraState.None:

                    UIManager.Instance.GetUI<BuildingInvenButton>().InvenMove();
                foreach (var ui in beforeOpenUIList)
                {
                    ui.Open();
                }
                break;
        }
    }

    /// <summary>
    /// UIManager�� ����ϴ� �Լ�
    /// </summary>
    public void RegistUI(UIBase ui)
    {
        var key = ui.GetType().Name;

        bool hasKey = false;

        // ��ųʸ��� ����Ʈ�� ��ϵǾ��ִٸ�
        if (totalUIDict.ContainsKey(key) || totalUIList.Contains(ui))
        {
            // ����� �Ǿ��ִµ� ���̾ƴ϶�� �̹� ��� �Ǿ� �ִٴ� �� ������
            // ����� ��ų�ʿ䰡�����Ƿ� ����
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
    /// �����ִ� UI����Ʈ ������ִ� �Լ�
    /// </summary>
    /// <param name="ui"></param>
    public void RegistOpenUI(UIBase ui)
    {
        if (ui.isOpen)
            totalOpenUIList.Add(ui);
    }

    /// <summary>
    /// �����ִ� UI����Ʈ�� �����ִ� �Լ�
    /// </summary>
    public void RemoveOpenUI(UIBase ui)
    {
        if (totalOpenUIList.Contains(ui))
            totalOpenUIList.Remove(ui);
    }

    /// <summary>
    /// �����ִ� UI����Ʈ�� ���� �ʱ�ȭ ��Ű���Լ�
    /// </summary>
    public void RemoveAllOpenUI()
    {
        totalOpenUIList.Clear();
    }

    /// <summary>
    /// uidictionary �� uiList ������
    /// ���� �����ִ� �Լ�
    /// </summary>
    public void RemoveUIInfo()
    {
        totalUIDict.Clear();
        totalUIList.Clear();
    }


    // getui�� ���װ� ���� ���ǿ� ���� �߰��ؼ� ã�� �Լ� �����ϱ�
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
    /// �����ִ� UIâ�� ���� �����ִ� UI�� ��ȯ�ϴ� �Լ�
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
    /// �����ִ�  ui�߿��� ���� ����������
    /// esc�� ������ �ִ� ui�� ã���ִ� �Լ�
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