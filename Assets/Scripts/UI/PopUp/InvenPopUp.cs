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

        //취소 버튼 누르면 그냥 팝업창 닫아주기
        cancelButton.onClick.AddListener(() =>
        {
            Close();
        });
    }

    /// <summary>
    /// 팝업창 비활성화 해주는 함수
    /// </summary>
    void Close()
    {
        transform.gameObject.SetActive(false);
    }

}
