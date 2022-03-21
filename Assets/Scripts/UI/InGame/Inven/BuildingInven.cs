using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Inven
{

    public class BuildingInven : InvenBase,IPointerEnterHandler
    {

        /// <summary>
        /// ���� �ǹ��� ������ �ִ��� �ƴ��� üũ�ϴ� �Լ�
        /// </summary>
        private bool canBuild = false;

        public BuildingInvenSlot currentSelectSlot;

        /// <summary>
        /// �κ��丮�� �������� ���� content��ǥ �������� ������ ���� �� �޾��ش�
        /// </summary>
        public RectTransform contentRect;

        /// <summary>
        /// ��ó�� content�� �ʱⰪ
        /// </summary>
        Vector3 contentOrginPos;

        Vector2 contentPos;
        /// <summary>
        /// viewPort RectTransform 
        /// </summary>
        public RectTransform viewport;

        public List<BuildingInvenSlot> invenSlots = new List<BuildingInvenSlot>();

        private const float contentMoveDistance = 125f;

        public override void Start()
        {
            base.Start();
            Initialize();
        }

        /// <summary>
        /// �ʱ�ȭ �۾�
        /// </summary>
        void Initialize()
        {
            SlotListRegist();

            // currentSelectSlot�� Ű �Է��� ���� �޾��ְ� �����Ƿ�
            // ��ó���� Ű�Է��� ���� ������ 1��° ������ ���õ��� �����Ƿ�
            // �̸� �ʱ�ȭ�� �����ش�
            if (currentSelectSlot == null)
                currentSelectSlot = (BuildingInvenSlot)slotList[0];


            var inputEvent = GameManager.InputEvent;

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            SlotRefresh();
            SetMaskable();
        }


        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            // �巡�� �Ҷ� ���� ��ġ�� �ִ� ��ü�� slot�� �޴´�
            //beginDragSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<BuildingInvenSlot>();
            beginDragSlot = slotList[GetSlotNum(eventData.pointerCurrentRaycast.gameObject)];


            // ������ GetComponent������ null �̸� �巡�׵Ȱ�
            // ������ �巡���Ѱ� �ƴϹǷ� ���� �����༭ �۵�x
            if (beginDragSlot == null)
                return;


            dragSlotImage = beginDragSlot.itemIconImage;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            if (beginDragSlot == null)
                return;

            // �巡�� �ɶ� �ش� �̹����� ���콺 ��ġ���� ���󰡰Բ� ���ش�
            dragSlotImage.transform.position = eventData.position;

        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            // �巡�װ� ������ ���콺�� �ִ� ���������� �޴´�
            endDragSlot = eventData.pointerCurrentRaycast.gameObject?.GetComponent<BuildingInvenSlot>();

            if (endDragSlot == null)
            {
                dragSlotImage.transform.position = dragSlotImage.transform.parent.position;
                SlotRefresh();
                DragSlotClear();
                return;
            }


            dragSlotImage.transform.position = dragSlotImage.transform.parent.position;
            SlotSwap();
            SlotRefresh();
            DragSlotClear();
        }

        /// <summary>
        /// ���Ը���Ʈ ����ϴ� �Լ�
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slotList);
        }

        /// <summary>
        /// ������ �����ϰ� ���ִ� �Լ�
        /// </summary>
        void SelectSlot()
        {

            //��ü ���� ����Ʈ�� ��ȸ�ؼ� 
            foreach (BuildingInvenSlot slot in slotList)
            {

                // �Ϲ����� ��Ȳ�϶��� �����Ÿ� Ȱ��ȭ �����ش�
                if (slot.isSelect)
                {
                    slot.isSelect = !slot.isSelect;

                    // ������ ������ ��ȣ��� �ٽ� ó�� �κ������� ���ý����ش�
                    // �ε��� ���� �������� �ϹǷ� listCount�� 1�� ���ش�
                    if (slot.transform.GetSiblingIndex() == slotList.Count - 1)
                    {
                        slotList[0].isSelect = true;
                        currentSelectSlot = (BuildingInvenSlot)slotList[0];
                        contentRect.anchoredPosition = contentOrginPos;
                        break;
                    }
                    slotList[(slot.transform.GetSiblingIndex() + 1)].isSelect = true;
                    currentSelectSlot = (BuildingInvenSlot)slotList[(slot.transform.GetSiblingIndex() + 1)];
                    contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x - contentMoveDistance, contentRect.anchoredPosition.y);

                    break;
                }
            }

            // ��ü ���Կ��� ���õ� ���Ը� Ȱ��ȭ ������
            foreach (BuildingInvenSlot slot in slotList)
            {
                slot.SelectImageOnOff();
            }

        }

        /// <summary>
        /// ���õ� ���Ա������� �������� �Ű��ִ� �Լ�
        /// </summary>
        /// <Param>���õ� ����</Param>
        void MoveContent(BuildingInvenSlot slot)
        {
            Vector2 desVec;
            desVec.x = slot.transform.position.x + contentMoveDistance;
            desVec.y = 0;
            if (RectTransformUtility.RectangleContainsScreenPoint(viewport, desVec))
            {
                contentPos.x = contentRect.anchoredPosition.x - contentMoveDistance;
                contentRect.anchoredPosition = contentPos;
            }
        }

        /// <summary>
        /// ������ ��ȣ�� ��ȯ���ִ� �Լ�
        /// </summary>
        /// <param name="go">�̺�Ʈ �ý������� ���� 
        /// ���콺���� ����ĳ��Ʈ �´� GameObject</param>
        /// <returns></returns>
        private int GetSlotNum(GameObject go)
        {
            // slot�� �̸����� slot1,slot2,slot3... �̷�������
            // �ö󰡴� ��Ģ�� �����������Ƿ� slot�� ���� �ڿ��ִ� ����
            // �����ͼ� list�� ��ȣ�� �޴´�
            var name = go.name;

            name = name.Substring(name.LastIndexOf('t') + 1);
            // ���� �ʿ��� ��ȣ�� slotlist�� �ε��� ��ȣ�� �־��� ��������
            // 1�� ���ش�
            return (int.Parse(name) - 1);
        }


        // ����Ʈ �κп��� minX, maxX���� �����ͼ� �׹����� �Ѿ��ٸ� maskableüũ �ƴ϶�� üũ�������ִ� �Լ� �����

        private void SetMaskable()
        {
            var minX = viewport.rect.xMin;
            var maxX = viewport.rect.xMax;

            foreach (var slot in slotList)
            {
                if((slot.transform.position.x> minX)&&(slot.transform.position.x< maxX))
                {
                    slot.itemIconImage.maskable = false;
                }
                else
                {
                    slot.itemIconImage.maskable = true;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetMaskable();
        }

        /// <summary>
        /// �κ��� �������� �߰����ִ� �Լ�
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem)
        {
            //TODO 03/21 ���� ������ add���ִ� �� ����
            // buildinginvenslotonoff�ϴ°� ���� ��ü Ȱ��ȭ ��Ȱ��ȭ�� �װ� ��ġ��
        }
    }
}
