using Project.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenPopUp : UIBase
{

    /// <summary>
    /// Ȯ�� ��ư
    /// </summary>
    public Button okayButton;

    /// <summary>
    /// ĵ����ư
    /// </summary>
    public Button cancelButton;

    /// <summary>
    /// ���� �������ִ°�
    /// </summary>
    public UIMover mover;

    public override void Start()
    {
        base.Start();

        ButtonInit();
    }

    /// <summary>
    /// ��ư�̺�Ʈ �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    void ButtonInit()
    {
        InventoryHandler inven = UIManager.Instance.GetUI<InventoryHandler>();

        okayButton.onClick.AddListener(() =>
        {
            inven.ItemDestroy(inven.beginDragItemSlot, inven.beginDragItemSlot.itemCount);
            Close();
        });

        //��� ��ư ������ �׳� �˾�â �ݾ��ֱ�
        cancelButton.onClick.AddListener(() =>
        {
            Close();
        });
    }

    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);
        mover.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);
        mover.Close();
    }

}
