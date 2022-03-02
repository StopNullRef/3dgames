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
    public GameObject invenUI; // �κ��丮 UI
    public GameObject invenMover; // �κ��丮 �Ű��ִ� �κ�

    public SceneDropDown dropdown; // ��Ӵٿ� �޾��ٰ�

    public InvenPopUp invenPopUp; // �κ��丮 �˾� UI

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
    /// ������Ʈ�� ������ UI����Ʈ
    /// </summary>
    private List<UIBase> updateUIList = new List<UIBase>();

    /// <summary>
    /// �κ��丮 ������Ʈ
    /// </summary>
    public InventoryHandler inven;

    private void Start()
    {
        dropdown = GetUI<SceneDropDown>();
    }

    public void Update()
    {

        if (updateUIList.Count > 0)
        {
            foreach (var ui in updateUIList)
            {
                ui.OnUpate();
            }
        }
    }


    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        if (GameObject.FindObjectOfType<Canvas>() == null)
            GameObject.Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/InGameUICanvas.prefab"), null, false);
    }

    /// <summary>
    /// ī�޶� ���¿� ���� UI�� �ٲ��� �Լ�
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
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
                List<UIBase> noneUI = totalUIList.Where(_ => _.type == Define.UIType.None).ToList();
                foreach (var ui in noneUI)
                    ui.Open();
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

    public void RigistUpdate(UIBase ui)
    {
        var type = ui.GetType().Name;
        // ����Ÿ���� ���ٸ� ��� �ִٸ� �̹������Ƿ� return
        if (updateUIList.Find(_ => _.GetType().Name == type) == null)
            updateUIList.Add(ui);
        else
            return;
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


    // getui�� ���װ� ���� ���ǿ� ���� �߰��ؼ� ã�� �Լ� �����ϱ�
    public T GetUI<T>(Predicate<UIBase> predicate = null) where T : UIBase
    {
        T result = null;
        var typeName = typeof(T).Name;

        if (predicate != null)
        {
            //TODO ����κ� �߰��ϱ�
            return totalUIList.Find(predicate) as T;
        }

        if (totalUIDict.ContainsKey(typeName))
        {
            result = totalUIDict[typeName] as T;
        }

        return result;
    }

    /// <summary>
    /// �����ִ� UIâ�� ���� �����ִ� UI�� ��ȯ�ϴ� �Լ�
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