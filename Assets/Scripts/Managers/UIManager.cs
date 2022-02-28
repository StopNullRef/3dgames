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

    bool invenActive = false; //�κ� Ȱ��ȭ üũ�� ��Ÿ�� ����

    Transform invenHolder;

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
    /// ���̿���Ű�� �ִ� ĵ�������� ���� �迭
    /// </summary>
    private Canvas[] canvasArr;

    /// <summary>
    /// �κ��丮 ������Ʈ
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


    // public���� ��� ������ â���� �־��ְ� �ɶ�
    // ���� �ٲ�� missing �� �߰� ������ ������ �־���� �ɵ��ϴ�.
    // TODO ������ �־��ִ°� ���� ���� start���� uimanager�� ����ϰ� ���� �ٲٱ�
    public void UIInitialize()
    {


        if (Define.Scene.LoadingScene != (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            canvasArr = GameObject.FindObjectsOfType<Canvas>(true);

            // 0 �κ� 1 �κ� ����

            invenHolder = canvasArr.Where(_ => _.CompareTag("InGameCanvas")).SingleOrDefault().transform.GetChild(1);

            //invenHolder = GameObject.Find(Define.FindDataString.ingameUI).transform.GetChild(1);
            //dropdown = invenHolder.parent.GetChild(0).GetComponent<SceneDropDown>();
            //dropdown.Init();
            inven = GameObject.FindObjectOfType<InventoryHandler>(true);
            invenPopUp = GameObject.FindObjectOfType<InvenPopUp>(true);
        }
    }

    /// <summary>
    /// �κ��丮 �¿��� ���
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
    /// ī�޶� ���¿� ���� UI�� �ٲ��� �Լ�
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
        // ó���� ��� ui�� ���ش�
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

        if(predicate != null)
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