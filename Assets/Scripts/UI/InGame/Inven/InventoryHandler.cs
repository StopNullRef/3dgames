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
    /// ó�� �巡�� �ɶ� ���콺��ġ�� �־��� �̹���
    /// </summary>
    public Image tempSlotImage;

    /// <summary>
    /// ó�� tempslot ��ġ
    /// </summary>
    private Vector3 originTempSlotPos;

    /// <summary>
    /// tempslot �� itemslot ������Ʈ
    /// </summary>
    public ItemSlot tempItemSlot;

    /// <summary>
    /// �κ��丮 ������ ������ ���� ����Ʈ
    /// </summary>
    public List<ItemSlot> itemSlots;

    /// <summary>
    /// BeginDrag �� ���콺��ġ���ִ� ������  itemSlot
    /// </summary>
    public ItemSlot beginDragItemSlot;

    /// <summary>
    /// endDrag �� ���콺��ġ���ִ� ������  itemSlot
    /// </summary>
    public ItemSlot endDragItemSlot;

    /// <summary>
    /// �κ��丮 RectTransform
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
    /// ó�� ���� ���������ְ� 
    /// ����Ʈ�� add���ִ� �Լ�
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
        //ó���� ���ö� ����ְ� �ٽ� �־���
        beginDragItemSlot = null;
        endDragItemSlot = null;

        //���콺 ���� UI�� �÷��������ʴٸ� �۵�x
        if (EventSystem.current.IsPointerOverGameObject() == false)
            return;


        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out beginDragItemSlot))
        {
            //ó�� �巡�� �ɶ� �ش� slot�� ����ִ� �Ŷ�� �۵�x
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
        //ó�� �巡�װ� ������ tempslot�� �ٽ� ���ڸ��� ������
        tempSlotImage.transform.GetChild(0).localPosition = originTempSlotPos;

        //RectTransformUtility.RectangleContainsScreenPoint
        if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, eventData.position))
        {
            //EndDrag�Ҷ� inven rect������ ������ ���� �Ұ��� �˾� â����ִºκ�
            UIManager.Instance.GetUI<InvenPopUp>().Open();
            return;
        }

        eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out endDragItemSlot);

        if ((endDragItemSlot == null) || (beginDragItemSlot == endDragItemSlot))
            return;

        int EndDragMaxCount = MaxCountReturn(beginDragItemSlot.itemInfo);

        // BeginDarg slot�̶� EndDrag slot�̶� ���� ������ ���� �ٲ���
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
            // �巡�� ���� �̶� ���̶� �������� ������  ������ߵǴµ� �ִ�ġ�� ������
            // �ִ� ���������� ��ģ�Ÿ� �� �������� �־��ְ� �ٸ�  ���ʿ��� �ִ�ġ�� �־���
            if ((beginDragItemSlot.itemCount + endDragItemSlot.itemCount) > EndDragMaxCount)
            {
                int overCount = (beginDragItemSlot.itemCount + endDragItemSlot.itemCount) - EndDragMaxCount;

                beginDragItemSlot.itemCount = overCount;
                endDragItemSlot.itemCount = EndDragMaxCount;
            }
            else
            {
                // �������� �ִ� ������ �ƴ϶�� �����ְ� ó�� �巡�� ������ �����
                endDragItemSlot.itemCount += beginDragItemSlot.itemCount;
                beginDragItemSlot.itemCount = 0;
                beginDragItemSlot.itemInfo = itemManager.itemList[(int)Define.ScriptableItem.None];
            }
        }

        InvenRefresh();
    }

    // �鿩���Ⱑ �ʹ� ���� �������� ������
    // �ϳ��� ����Ʈ�� �ް� �۵���Ű�Բ�����
    public void AddItem(ObjInfo obj)
    {
        int itemMaxCount = MaxCountReturn(obj.ItemDrop);

        var nonItem = itemManager.itemList[(int)Define.ScriptableItem.None];

        // ������ �ִ� ������ ���������� ȹ���Ҿ����۰� ���� �������� ����ִ� ������ ã�´�
        var addItemSlot = itemSlots.Where(_ => (_.itemCount < itemMaxCount) && (_.itemInfo.itemCode == obj.ItemDrop.itemCode)).FirstOrDefault();

        if (addItemSlot == null)
            AddNonHaveItem();
        else
            AddHaveItem();

        // �ٲ� ������ ���� �̹����� �ؽ�Ʈ ���̰� �� ����
        InvenRefresh();

        // �κ��� ���� �������� ���� �� �۵��ϴ� �Լ�
        void AddHaveItem()
        {
            var addItemCount = UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop);

            if ((addItemSlot.itemCount + addItemCount) > itemMaxCount)
            {
                int remainItemCount = (addItemSlot.itemCount + addItemCount) - itemMaxCount;

                // ������ �ִ� ������ �־��ְ�
                addItemSlot.SlotInitialize(obj, itemMaxCount);

                // ����ִ� ������ ���� ã�Ƽ� �־��ֱ�
                itemSlots.Where(_ => _.itemInfo == nonItem).FirstOrDefault().SlotInitialize(obj, remainItemCount);
            }
            else
                addItemSlot.SlotInitialize(addItemCount);
        }

        // �κ��� ���� �������� ���� �� �۵��ϴ� �Լ�
        void AddNonHaveItem()
        {
            // ����ִ� ������ ������ ã�´�
            addItemSlot = itemSlots.Where(_ => _.itemInfo == nonItem).FirstOrDefault();
            // �ش罽�� ������ ���� �־��ְ� �ʱ�ȭ
            addItemSlot.SlotInitialize(obj, UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop));
        }

    }

    /// <summary>
    /// �κ��丮 ���� ���δ� �̹��� �ؽ�Ʈ�� �ٽ� �������ִ� �Լ�
    /// </summary>
    public void InvenRefresh()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.SlotRefresh();
        }
    }

    /// <summary>
    /// �Ű������� ���� item�� �κ��丮�� �ִ� ������
    /// ��ȯ���ִ� �Լ�
    /// </summary>
    /// <param name="item">��ȯ�� ������</param>
    /// <returns>item�� �ִ� ���� ����</returns>
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
    /// �Ŵ������� ���� ������Ʈ ��
    /// </summary>
    public override void OnUpate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InvenOnOff();
        }
    }

    /// <summary>
    /// �κ��丮 ����Ű�� �Լ�
    /// </summary>
    private void InvenOnOff()
    {
        if (isOpen)
            Close();
        else
            Open();
    }


    /// <summary>
    /// ������ ������ �Լ�
    /// </summary>
    /// <param name="slot">���� �������� �������ִ� ����</param>
    /// <param name="count">���� ����</param>
    public void ItemDestroy(ItemSlot slot, int count)
    {
        //���� ������ ��ũ�� �۵�x
        if (count > slot.itemCount)
            return;

        //�Ű����� count�� slot itemCount�� ������
        //������ ���� ����ְ� ī��Ʈ 0����
        if (count == slot.itemCount)
        {
            slot.itemInfo = ItemManager.Instance.itemList[(int)(Define.ScriptableItem.None)];
            slot.itemCount = 0;
        }
        else
            //���������� ������������ �۴ٸ� ������ ����
            slot.itemCount -= count;
        InvenRefresh();
    }
}
