using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    public GameObject invenUI; // �κ��丮 UI
    public GameObject invenMover; // �κ��丮 �Ű��ִ� �κ�

    bool invenActive = false; //�κ� Ȱ��ȭ üũ�� ��Ÿ�� ����

    Transform invenHolder;

    public SceneDropDown dropdown; // ��Ӵٿ� �޾��ٰ�

    public InvenPopUp invenPopUp; // �κ��丮 �˾� UI

    private const string buildingCanvas = "BuildingCanvas";

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
        // UIInitialize();
    }

    public void Update()
    {
        InvenOnOff();
    }

    // public���� ��� ������ â���� �־��ְ� �ɶ�
    // ���� �ٲ�� missing �� �߰� ������ ������ �־���� �ɵ��ϴ�. 
    public void UIInitialize()
    {

        if (Define.Scene.LoadingScene != (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            canvasArr = GameObject.FindObjectsOfType<Canvas>(true);

            // 0 �κ� 1 �κ� ����

            invenHolder = canvasArr.Where(_ => _.CompareTag("InGameCanvas")).SingleOrDefault().transform.GetChild(1);

            //invenHolder = GameObject.Find(Define.FindDataString.ingameUI).transform.GetChild(1);
            dropdown = invenHolder.parent.GetChild(0).GetComponent<SceneDropDown>();
            dropdown.Init();
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
            CursorManager.Instance.canCusorChange = !CursorManager.Instance.canCusorChange;
        }
    }

    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        if (GameObject.FindObjectOfType<Canvas>() == null)
        {
            Canvas canvas = new Canvas { name = "InGameUICanvas" };
            canvas = Resources.Load<Canvas>("Prefabs/InGameUICanvas.prefab");
        }

        UIInitialize();
    }

    /// <summary>
    /// ī�޶� ���¿� ���� UI�� �ٲ��� �Լ�
    /// </summary>
    public void CameraStateToSetCanvas(Define.CameraState cameraState)
    {
        switch (cameraState)
        {
            case Define.CameraState.Build:
                foreach (var canvas in canvasArr)
                {
                    if (canvas.GetComponent<CanvasType>().type == Define.CanvasType.Building)
                        canvas.gameObject.SetActive(true);
                    else
                        canvas.gameObject.SetActive(false);
                }
                break;
            case Define.CameraState.None:
                foreach (var canvas in canvasArr)
                {
                    if (canvas.GetComponent<CanvasType>().type == Define.CanvasType.Building)
                        canvas.gameObject.SetActive(false);
                    else
                        canvas.gameObject.SetActive(true);
                }
                break;
        }
    }
}