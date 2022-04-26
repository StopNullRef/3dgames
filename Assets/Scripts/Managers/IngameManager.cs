using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngameManager : Singleton<IngameManager>
{
    /// <summary>
    /// �⺻ Ŀ�� texture
    /// </summary>
    Texture2D basicCursor;

    /// <summary>
    /// ����ĳ��Ʈ ���� ���̾ �����϶� �ٲ��� Ŀ�� texture
    /// </summary>
    Texture2D treeCursor;

    /// <summary>
    /// ����ִ� �ǹ��κ� ���� �̹���
    /// </summary>
    public Sprite buildingInvenSlot;

    /// <summary>
    /// Ŀ���� �ٲܼ� �ִ��� �ƴ��� üũ�ϴ� ��Ÿ�� ����
    /// </summary>
    public bool canCusorChange = false;

    //TODO 04/21 ���� ������ ó�� ������ �ǹ� �����ȵ� ���� �ǰԲ����ֱ�

    static BuidingSystemController buildingSystem = new BuidingSystemController();

    public BuidingSystemController BuildingSystem { get => buildingSystem; }


    protected override void Awake()
    {
        base.Awake();

        basicCursor = Resources.Load<Texture2D>(Define.ResourcePath.basicCursor);
        basicCursor.alphaIsTransparency = true;

        treeCursor = Resources.Load<Texture2D>(Define.ResourcePath.treeCursor);
        treeCursor.alphaIsTransparency = true;

        buildingInvenSlot = Resources.Load<Sprite>("Assets/Resources/Texture/gui_01_bg_03");

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
        buildingSystem.OnUpdate();
    }

    public void Clear()
    {

    }



    public void SetBasicCursor()
    {
        // Ŀ�� ��� forcesoftware�� �ϸ� �ʹ� �۰Գ��´�
        Cursor.SetCursor(basicCursor, Vector2.zero, CursorMode.Auto);
    }

    // ���콺 ��ġ�κ��� Raycast�� �ؼ� �´� Object�� �±׸� �˻��ؼ� ���콺 Ŀ���� �ٲ��ִ� �Լ�
    public void ChangeCursor()
    {
        RaycastHit hit;
        // ���� ���콺 ��ġ�� UI�� �ִٸ� Ŀ���� �ٲ���������
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
