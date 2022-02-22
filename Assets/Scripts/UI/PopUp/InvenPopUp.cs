using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenPopUp : UIPopUpBase
{
    public bool okayClick = false;
    InventoryHandler inven;


    public override void Start()
    {
        base.Start();
        Initialize();
        inven = UIManager.Instance.inven;
    }

    protected override void Initialize()
    {
        base.Initialize();
        ButtonInit();
    }

    void ButtonInit()
    {
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

    /// <summary>
    /// �˾�â ��Ȱ��ȭ ���ִ� �Լ�
    /// </summary>
    void Close()
    {
        transform.gameObject.SetActive(false);
    }

}
