using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

    public void Awake()
    {
        SlotInitialize();
    }

    public void Start()
    {
        originTempSlotPos = tempSlotImage.transform.localPosition;
        tempItemSlot = tempSlotImage.GetComponent<ItemSlot>();
        invenRect = transform.GetComponent<RectTransform>();
    }

    /// <summary>
    /// ó�� ���� ���������ְ� 
    /// ����Ʈ�� add���ִ� �Լ�
    /// </summary>
    public void SlotInitialize()
    {
        itemSlots.Clear();
        for (int i = 1; i < 26; i++)
        {
            //Instantiate(tempSlotImage, gameObject.transform).name = "Slot" + i;
            itemSlots.Add(gameObject.transform.GetChild(i - 1).GetComponent<ItemSlot>());
            itemSlots[i - 1].SlotInitialize();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //ó���� ���ö� ����ְ� �ٽ� �־���
        beginDragItemSlot = null;
        endDragItemSlot = null;
        //tempSlotImage.sprite = null;

        //���콺 ���� UI�� �÷��������ʴٸ� �۵�x
        if (EventSystem.current.IsPointerOverGameObject() == false)
            return;


        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out beginDragItemSlot))
        {
            //ó�� �巡�� �ɶ� �ش� slot�� ����ִ� �Ŷ�� �۵�x
            if ((beginDragItemSlot == endDragItemSlot) ||
                beginDragItemSlot.itemInfo == ItemManager.Instance.itemList[(int)Define.ScriptableItem.None])
                return;

            tempItemSlot.itemInfo = beginDragItemSlot.itemInfo;

            tempItemSlot.SlotRefresh(tempItemSlot.itemInfo, tempItemSlot.itemCount);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (beginDragItemSlot.itemInfo == ItemManager.Instance.itemList[(int)Define.ScriptableItem.None])
        {
            tempSlotImage.transform.GetChild(0).localPosition = originTempSlotPos;
            return;
        }

        tempSlotImage.transform.GetChild(0).position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�� �κ� ���̱� �ִ��� ����ϰ� ����� ����

        //ó�� �巡�װ� ������ tempslot�� �ٽ� ���ڸ��� ������
        tempSlotImage.transform.GetChild(0).localPosition = originTempSlotPos;


        //RectTransformUtility.RectangleContainsScreenPoint
        if (!RectTransformUtility.RectangleContainsScreenPoint(invenRect, eventData.position))
        {
            //EndDrag�Ҷ� inven rect������ ������ ���� �Ұ��� �˾� â����ִºκ�
            UIManager.Instance.invenPopUp.gameObject.SetActive(true);
            return;
        }

        // ������ gameobj�� itemslot component�� ������ �۵� x 
        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ItemSlot>(out endDragItemSlot))
        {
            if ((beginDragItemSlot == endDragItemSlot) || (endDragItemSlot == null))
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
            else if (endDragItemSlot.itemInfo.itemCode == beginDragItemSlot.itemInfo.itemCode)
            {
                // �巡�� ���� �̶� ���̶� �������� ������  ������ߵǴµ� �ִ�ġ�� ������
                // �ִ� ���������� ��ģ�Ÿ� �� �������� �־��ְ� �ٸ�  ���ʿ��� �ִ�ġ�� �־���
                if ((beginDragItemSlot.itemCount + endDragItemSlot.itemCount) > EndDragMaxCount)
                {
                    int overCount = (beginDragItemSlot.itemCount + endDragItemSlot.itemCount) - EndDragMaxCount;

                    beginDragItemSlot.itemCount = overCount;
                    endDragItemSlot.itemCount = EndDragMaxCount;
                    InvenRefresh();
                }
                else
                {
                    // �������� �ִ� ������ �ƴ϶�� �����ְ� ó�� �巡�� ������ �����
                    endDragItemSlot.itemCount += beginDragItemSlot.itemCount;
                    beginDragItemSlot.itemCount = 0;
                    beginDragItemSlot.itemInfo = ItemManager.Instance.itemList[(int)Define.ScriptableItem.None];
                }
            }

        }

        InvenRefresh();
    }

    public void AddItem(ObjInfo obj)
    {
        int itemMaxCount = MaxCountReturn(obj.ItemDrop);

        // ������ �� ������ �ִٸ� ������ �ش� ���Կ� �����ؼ�
        // ������ ������ �÷��ش�.
        for (int i = 0; i < itemSlots.Count; i++)
        {
            //ó���� ���Ե��� �˻��ؼ� ���Կ� �ִ� �������ڵ尡 ����
            //�� ������ 999������ ������ ���� �÷���
            if (itemSlots[i].itemInfo.itemCode == obj.ItemDrop.itemCode && (itemSlots[i].itemCount < itemMaxCount)) // �������ڵ尡 ������
            {
                itemSlots[i].itemCount += UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop);

                //���࿡ ���Կ� �������� �־�Z�µ� �ִ� ������ �Ѿ�����
                //�ִ밹�� ���� �������� �󽽷Կ� �־� �ش�
                if (itemSlots[i].itemCount > itemMaxCount)
                {
                    for (int k = 0; k < itemSlots.Count; k++)
                    {
                        if (itemSlots[k].itemInfo == ItemManager.Instance.itemList[(int)Define.ScriptableItem.None])
                        {
                            int overCount = itemSlots[i].itemCount - 999;
                            itemSlots[i].itemCount -= overCount;

                            itemSlots[k].itemInfo = obj.ItemDrop;

                            itemSlots[k].itemCount += overCount;

                            itemSlots[k].SlotRefresh(obj.ItemDrop, itemSlots[k].itemCount);
                            itemSlots[i].SlotRefresh(obj.ItemDrop, itemSlots[i].itemCount);
                            return;
                        }
                    }
                }
                itemSlots[i].SlotRefresh(obj.ItemDrop, itemSlots[i].itemCount);
                return;
            }

        }

        // �������� ������ ���ٸ� ������ ����ִ��� üũ��
        // �� ���Կ� �������� �־��ش�
        for (int j = 0; j < itemSlots.Count; j++)
        {
            if (itemSlots[j].itemInfo == ItemManager.Instance.itemList[(int)Define.ScriptableItem.None])
            {
                itemSlots[j].itemInfo = obj.ItemDrop;
                itemSlots[j].itemCount += UnityEngine.Random.Range(899, Define.MaxCount.ObjectMaxDrop);
                itemSlots[j].SlotRefresh(obj.ItemDrop, itemSlots[j].itemCount);
                itemSlots[j].itemInfo = obj.ItemDrop;
                return;
            }
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
        {
            //���������� ������������ �۴ٸ� ������ ����
            slot.itemCount -= count;
        }
        InvenRefresh();
    }

    private void OnEnable()
    {
        InvenRefresh();
    }

}
