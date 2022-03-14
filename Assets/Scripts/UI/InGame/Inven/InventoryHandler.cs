using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Project.UI;
using System.Linq;

public class InventoryHandler : UIBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 처음 드래그 될때 마우스위치에 넣어줄 이미지
    /// </summary>
    public Image tempSlotImage;

    /// <summary>
    /// 처음 tempslot 위치
    /// </summary>
    private Vector3 originTempSlotPos;

    /// <summary>
    /// tempslot 의 itemslot 컴포넌트
    /// </summary>
    public ItemSlot tempItemSlot;

    /// <summary>
    /// 인벤토리 하위에 아이템 슬롯 리스트
    /// </summary>
    public List<ItemSlot> itemSlots;

    /// <summary>
    /// BeginDrag 때 마우스위치에있는 슬롯의  itemSlot
    /// </summary>
    public ItemSlot beginDragItemSlot;

    /// <summary>
    /// endDrag 때 마우스위치에있는 슬롯의  itemSlot
    /// </summary>
    public ItemSlot endDragItemSlot;

    /// <summary>
    /// 인벤토리 RectTransform
    /// </summary>
    RectTransform invenRect;

    public UIMover mover;

    ItemManager itemManager;

    public void Awake()
    {
        SlotInitialize();
    }

    public override void Start()
    {
        base.isEscClose = true;
        base.isOpen = false;
        base.Start();
        originTempSlotPos = tempSlotImage.transform.localPosition;
        tempItemSlot = tempSlotImage.GetComponent<ItemSlot>();
        invenRect = transform.GetComponent<RectTransform>();
        mover ??= transform.parent.GetComponentInChildren<UIMover>();
        itemManager = ItemManager.Instance;
        DataManager.Instance.InvenLoad();

    }

    public void Update()
    {
        OnUpate();
    }

    /// <summary>
    /// 처음 슬롯 생성시켜주고 
    /// 리스트에 add해주는 함수
    /// </summary>
    public void SlotInitialize()
    {
        itemSlots.Clear();
        for (int i = 1; i < gameObject.transform.childCount; i++)
        {
            itemSlots.Add(gameObject.transform.GetChild(i - 1).GetComponent<ItemSlot>());
            itemSlots[i - 1].SlotInitialize();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //처음에 들어올때 비워주고 다시 넣어줌
        beginDragItemSlot = null;
        endDragItemSlot = null;

        //마우스 위에 UI가 올려져있지않다면 작동x
        if (EventSystem.current.IsPointerOverGameObject() == false)
            return;


        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out beginDragItemSlot))
        {
            //처음 드래그 될때 해당 slot이 비어있는 거라면 작동x
            if ((beginDragItemSlot == endDragItemSlot) ||
                beginDragItemSlot.itemInfo == itemManager.itemList[(int)Define.ScriptableItem.None])
                return;

            tempItemSlot.itemInfo = beginDragItemSlot.itemInfo;

            tempItemSlot.SlotRefresh(tempItemSlot.itemInfo, tempItemSlot.itemCount);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (beginDragItemSlot == null || (beginDragItemSlot.itemInfo == itemManager.itemList[(int)Define.ScriptableItem.None]))
        {
            tempSlotImage.transform.GetChild(0).localPosition = originTempSlotPos;
            return;
        }

        tempSlotImage.transform.GetChild(0).position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //처음 드래그가 끝날때 tempslot을 다시 제자리로 돌려줌
        tempSlotImage.transform.GetChild(0).localPosition = originTempSlotPos;

        //RectTransformUtility.RectangleContainsScreenPoint
        if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, eventData.position))
        {
            //EndDrag할때 inven rect범위를 넘으면 삭제 할건지 팝업 창띄워주는부분
            UIManager.Instance.GetUI<InvenPopUp>().Open();
            return;
        }

        eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out endDragItemSlot);

        if ((endDragItemSlot == null) || (beginDragItemSlot == endDragItemSlot))
            return;

        int EndDragMaxCount = MaxCountReturn(beginDragItemSlot.itemInfo);

        // BeginDarg slot이랑 EndDrag slot이랑 같이 않으면 서로 바꿔줌
        if (endDragItemSlot.itemInfo.itemCode != beginDragItemSlot.itemInfo.itemCode)
        {
            tempItemSlot.itemInfo = endDragItemSlot.itemInfo;
            tempItemSlot.itemCount = endDragItemSlot.itemCount;

            endDragItemSlot.itemInfo = beginDragItemSlot.itemInfo;
            endDragItemSlot.itemCount = beginDragItemSlot.itemCount;

            beginDragItemSlot.itemInfo = tempItemSlot.itemInfo;
            beginDragItemSlot.itemCount = tempItemSlot.itemCount;
        }
        else
        {
            // 드래그 시작 이랑 끝이랑 아이템이 같을때  합쳐줘야되는데 최대치를 넘으면
            // 최대 소지갯수랑 합친거를 뺀 나머지를 넣어주고 다른  한쪽에는 최대치를 넣어줌
            if ((beginDragItemSlot.itemCount + endDragItemSlot.itemCount) > EndDragMaxCount)
            {
                int overCount = (beginDragItemSlot.itemCount + endDragItemSlot.itemCount) - EndDragMaxCount;

                beginDragItemSlot.itemCount = overCount;
                endDragItemSlot.itemCount = EndDragMaxCount;
            }
            else
            {
                // 합쳤을때 최대 갯수가 아니라면 합쳐주고 처음 드래그 슬롯은 비워줌
                endDragItemSlot.itemCount += beginDragItemSlot.itemCount;
                beginDragItemSlot.itemCount = 0;
                beginDragItemSlot.itemInfo = itemManager.itemList[(int)Define.ScriptableItem.None];
            }
        }

        InvenRefresh();
    }

    // 들여쓰기가 너무 많다 가독성이 떨어짐
    // 하나의 리스트에 받고 작동시키게끔하자
    public void AddItem(ObjInfo obj)
    {
        int itemMaxCount = MaxCountReturn(obj.ItemDrop);

        var nonItem = itemManager.itemList[(int)Define.ScriptableItem.None];

        // 아이템 최대 갯수를 넘지않으며 획득할아이템과 같은 아이템을 들고있는 슬롯을 찾는다
        var addItemSlot = itemSlots.Where(_ => (_.itemCount < itemMaxCount) && (_.itemInfo.itemCode == obj.ItemDrop.itemCode)).FirstOrDefault();

        if (addItemSlot == null)
            AddNonHaveItem();
        else
            AddHaveItem();

        // 바뀐 정보에 따라 이미지와 텍스트 보이게 끔 만듦
        InvenRefresh();

        // 인벤에 같은 아이템이 있을 때 작동하는 함수
        void AddHaveItem()
        {
            var addItemCount = UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop);

            if ((addItemSlot.itemCount + addItemCount) > itemMaxCount)
            {
                int remainItemCount = (addItemSlot.itemCount + addItemCount) - itemMaxCount;

                // 아이템 최대 갯수로 넣어주고
                addItemSlot.SlotInitialize(obj, itemMaxCount);

                // 비어있는 아이템 슬롯 찾아서 넣어주기
                itemSlots.Where(_ => _.itemInfo == nonItem).FirstOrDefault().SlotInitialize(obj, remainItemCount);
            }
            else
                addItemSlot.SlotInitialize(addItemCount);
        }

        // 인벤에 같은 아이템이 없을 때 작동하는 함수
        void AddNonHaveItem()
        {
            // 비어있는 아이템 슬롯을 찾는다
            addItemSlot = itemSlots.Where(_ => _.itemInfo == nonItem).FirstOrDefault();
            // 해당슬롯 아이템 정보 넣어주고 초기화
            addItemSlot.SlotInitialize(obj, UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop));
        }

    }

    /// <summary>
    /// 인벤토리 슬롯 전부다 이미지 텍스트를 다시 조정해주는 함수
    /// </summary>
    public void InvenRefresh()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.SlotRefresh();
        }
    }

    /// <summary>
    /// 매개변수로 받은 item의 인벤토리의 최대 갯수를
    /// 반환해주는 함수
    /// </summary>
    /// <param name="item">반환할 아이템</param>
    /// <returns>item의 최대 소지 갯수</returns>
    public int MaxCountReturn(ItemScriptableObj item)
    {
        int itemMaxCount = 0;
        switch (item.itemKind)
        {
            case Define.ItemType.None:
                itemMaxCount = 0;
                break;

            case Define.ItemType.Equipment:
                itemMaxCount = Define.MaxCount.equipment;
                break;

            case Define.ItemType.Ingredient:
                itemMaxCount = Define.MaxCount.ingredient;
                break;

            case Define.ItemType.Potion:
                itemMaxCount = Define.MaxCount.potion;
                break;
        }
        return itemMaxCount;
    }

    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);

        if (mover == null)
            mover = transform.parent.GetComponentInChildren<UIMover>();

        mover.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);

        if (mover == null)
            mover = transform.parent.GetComponentInChildren<UIMover>();

        mover.Close();
    }

    /// <summary>
    /// 매니저에서 돌릴 업데이트 문
    /// </summary>
    public override void OnUpate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InvenOnOff();
        }
    }

    /// <summary>
    /// 인벤토리 껏다키는 함수
    /// </summary>
    private void InvenOnOff()
    {
        if (isOpen)
            Close();
        else
            Open();
    }


    /// <summary>
    /// 아이템 버리는 함수
    /// </summary>
    /// <param name="slot">버릴 아이템을 가지고있는 슬롯</param>
    /// <param name="count">버릴 갯수</param>
    public void ItemDestroy(ItemSlot slot, int count)
    {
        //버릴 갯수가 더크면 작동x
        if (count > slot.itemCount)
            return;

        //매개변수 count가 slot itemCount랑 같으면
        //아이템 정보 비워주고 카운트 0으로
        if (count == slot.itemCount)
        {
            slot.itemInfo = ItemManager.Instance.itemList[(int)(Define.ScriptableItem.None)];
            slot.itemCount = 0;
        }
        else
            //버릴갯수가 보유갯수보다 작다면 갯수만 차감
            slot.itemCount -= count;
        InvenRefresh();
    }
}
