using Project.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenPopUp : UIBase
{

    /// <summary>
    /// 확인 버튼
    /// </summary>
    public Button okayButton;

    /// <summary>
    /// 캔슬버튼
    /// </summary>
    public Button cancelButton;

    /// <summary>
    /// 위에 움직여주는거
    /// </summary>
    public UIMover mover;

    public override void Start()
    {
        base.Start();

        ButtonInit();
    }

    /// <summary>
    /// 버튼이벤트 초기화하는 함수
    /// </summary>
    void ButtonInit()
    {
        InventoryHandler inven = UIManager.Instance.GetUI<InventoryHandler>();

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
