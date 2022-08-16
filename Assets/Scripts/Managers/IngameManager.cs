using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngameManager : Singleton<IngameManager>
{
    /// <summary>
    /// 기본 커서 texture
    /// </summary>
    Texture2D basicCursor;

    /// <summary>
    /// 레이캐스트 맞은 레이어가 나무일때 바꿔줄 커서 texture
    /// </summary>
    Texture2D treeCursor;

    /// <summary>
    /// 비어있는 건물인벤 슬롯 이미지
    /// </summary>
    public Sprite buildingInvenSlot;

    /// <summary>
    /// 커서를 바꿀수 있는지 아닌지 체크하는 불타입 변수
    /// </summary>
    public bool canCusorChange = false;

    static BuidingSystemController buildingSystem = new BuidingSystemController();

    public BuidingSystemController BuildingSystem { get => buildingSystem; }

    public bool isBuilding = false;

    protected override void Awake()
    {
        base.Awake();

        basicCursor = Resources.Load<Texture2D>(Define.ResourcePath.basicCursor);
        //basicCursor.alphaIsTransparency = true;

        treeCursor = Resources.Load<Texture2D>(Define.ResourcePath.treeCursor);
        //treeCursor.alphaIsTransparency = true;

        buildingInvenSlot = Resources.Load<Sprite>("Texture/gui_01_bg_03");

        Init();
    }

    public void Start()
    {
    }

    public void Init()
    {
        SetBasicCursor();
    }

    public void Update()
    {
        ChangeCursor();
        // 현재 처음에는 잘생성이되나 마우스 위치 레이 쏜 위치 가 y값이 이상하게 낮게 잡혀서 plane이 안보이는 버그가 있음 이거 고치기
        buildingSystem.OnUpdate();
    }

    public void Clear()
    {

    }



    public void SetBasicCursor()
    {
        // 커서 모드 forcesoftware로 하면 너무 작게나온다
        Cursor.SetCursor(basicCursor, Vector2.zero, CursorMode.Auto);
    }

    // 마우스 위치로부터 Raycast를 해서 맞는 Object에 태그를 검사해서 마우스 커서를 바꿔주는 함수
    public void ChangeCursor()
    {
        RaycastHit hit;
        // 현재 마우스 위치에 UI가 있다면 커서를 바꿔주지않음
        if ((EventSystem.current.IsPointerOverGameObject() == true))
        {
            SetBasicCursor();
            return;
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Obj")))
        {
            switch (hit.transform.tag)
            {
                case "Tree":
                    Cursor.SetCursor(treeCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    SetBasicCursor();
                    break;
            }
        }
        else
        {
            SetBasicCursor();
        }
    }


}
