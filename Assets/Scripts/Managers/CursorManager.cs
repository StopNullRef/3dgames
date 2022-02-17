using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>
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
    /// Ŀ���� �ٲܼ� �ִ��� �ƴ��� üũ�ϴ� ��Ÿ�� ����
    /// </summary>
    public bool canCusorChange = false;

    protected override void Awake()
    {
        base.Awake();

        basicCursor = Resources.Load<Texture2D>(Define.ResourcePath.basicCursor);
        basicCursor.alphaIsTransparency = true;

        treeCursor = Resources.Load<Texture2D>(Define.ResourcePath.treeCursor);
        treeCursor.alphaIsTransparency = true;

        Init();
    }

    public void Init()
    {
        SetBasicCursor();
    }

    public void Update()
    {
        ChangeCursor();
    }

    public void Clear()
    {

    }



    public void SetBasicCursor()
    {
        if (basicCursor == null)
        {
            Debug.Log("Ŀ�� �ؽ��� ���� �ȹ޾��� ");
        }

        // Ŀ�� ��� forcesoftware�� �ϸ� �ʹ� �۰Գ��´�
        Cursor.SetCursor(basicCursor, Vector2.zero, CursorMode.Auto);
    }

    // ���콺 ��ġ�κ��� Raycast�� �ؼ� �´� Object�� �±׸� �˻��ؼ� ���콺 Ŀ���� �ٲ��ִ� �Լ�
    public void ChangeCursor()
    {
        RaycastHit hit;

        // ���� ���콺 ��ġ�� UI�� �ִٸ� Ŀ���� �ٲ���������
        if((EventSystem.current.IsPointerOverGameObject() == true) || (!canCusorChange))
        {
            SetBasicCursor();
            return;
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f))
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
