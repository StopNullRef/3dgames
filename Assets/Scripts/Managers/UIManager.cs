using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    public GameObject invenUI; // 인벤토리 UI
    public GameObject invenMover; // 인벤토리 옮겨주는 부분

    bool invenActive = false; //인벤 활성화 체크용 불타입 변수

    Transform invenHolder;

    public SceneDropDown dropdown; // 드롭다운 받아줄거

    public InvenPopUp invenPopUp; // 인벤토리 팝업 UI

    private const string buildingCanvas = "BuildingCanvas";

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
        // UIInitialize();
    }

    public void Update()
    {
        InvenOnOff();
    }

    // public으로 열어서 에디터 창에서 넣어주게 될때
    // 씬이 바뀌면 missing 이 뜨게 됨으로 일일이 넣어줘야 될듯하다. 
    public void UIInitialize()
    {

        if (Define.Scene.LoadingScene != (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            canvasArr = GameObject.FindObjectsOfType<Canvas>(true);

            // 0 인벤 1 인벤 무브

            invenHolder = canvasArr.Where(_ => _.CompareTag("InGameCanvas")).SingleOrDefault().transform.GetChild(1);

            //invenHolder = GameObject.Find(Define.FindDataString.ingameUI).transform.GetChild(1);
            dropdown = invenHolder.parent.GetChild(0).GetComponent<SceneDropDown>();
            dropdown.Init();
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
    /// 카메라 상태에 따라 UI를 바꿔줄 함수
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