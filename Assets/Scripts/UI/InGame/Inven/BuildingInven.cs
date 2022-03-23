using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Inven
{

    public class BuildingInven : InvenBase
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

        public RectTransform viewPort;

        /// <summary>
        /// ��ó�� content�� �ʱⰪ
        /// </summary>
        Vector3 contentOrginPos;

        Vector2 contentPos;

        private const float contentMoveDistance = 125f;

        Vector2 lastSlotViewPos;

        public override void Start()
        {
            //TODO 03/23 ���� ������� tabŰ�� ��������
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

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            //���õ� ������ ã�´�
            var selectSlot = slotList.Where(_ => _.isSelect == true).SingleOrDefault();
            // ���� ���� ��� ���� ó���� �ִ� ������ ���õ� ������ ����
            if (selectSlot == null)
                slotList[0].isSelect = true;

            foreach (BuildingInvenSlot slot in slotList)
                slot.SelectImageOnOff();

            SlotRefresh();
        }

        /// <summary>
        /// ���Ը���Ʈ ����ϴ� �Լ�
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slotList);
        }

        void AllSelectOnOff()
        {
            // ��ü ���Կ��� ���õ� ���Ը� Ȱ��ȭ ������
            foreach (BuildingInvenSlot slot in slotList)
            {
                slot.SelectImageOnOff();
            }
        }

        /// <summary>
        /// ������ �����ϰ� ���ִ� �Լ�
        /// </summary>
        void SelectSlot()
        {
            var selectSlot = slotList.Where(_ => _.isSelect == true).SingleOrDefault();
            selectSlot.isSelect = false;
            // ������ ������ ��ȣ��� �ٽ� ó�� �κ������� ���ý����ش�
            // �ε��� ���� �������� �ϹǷ� listCount�� 1�� ���ش�
            if (selectSlot.transform.GetSiblingIndex() == slotList.Count - 1)
            {
                SetSelectSlot(0);
                contentRect.anchoredPosition = contentOrginPos;
                AllSelectOnOff();
                return;
            }

            if (RectTransformUtility.RectangleContainsScreenPoint(viewPort, slotList[slotList.Count - 1].transform.position))
            {
                SetSelectSlot(selectSlot.transform.GetSiblingIndex() + 1);
                AllSelectOnOff();
                if (lastSlotViewPos == Vector2.zero)
                    lastSlotViewPos = new Vector2(contentRect.anchoredPosition.x - 50, contentRect.anchoredPosition.y);

                contentRect.anchoredPosition = lastSlotViewPos;
                return;
            }

            SetSelectSlot(selectSlot.transform.GetSiblingIndex() + 1);
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x - contentMoveDistance, contentRect.anchoredPosition.y);

            AllSelectOnOff();

            void SetSelectSlot(int slotNum)
            {
                slotList[slotNum].isSelect = true;
                currentSelectSlot = (BuildingInvenSlot)slotList[slotNum];
            }
        }


        /// <summary>
        /// �κ��� �������� �߰����ִ� �Լ�
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem, int count)
        {
            var slots = slotList.Cast<BuildingInvenSlot>();    //as List<BuildingInvenSlot>;

            // �����߿� ���� �������� ������ �ִ� ������ �ִ��� ã�´�
            var slot = slots.Where(_ => _.sd != null && _.sd.index == buildItem.index).FirstOrDefault();

            // ���� ��� �󽽷��� ã�´�
            if (slot == null)
                slot = slots.Where(_ => _.sd.index == 0).FirstOrDefault();

            slot.AddItem(buildItem, count);

            slot.SlotRefresh();
        }

        public override void Close(bool intialValue = false)
        {
            var invenButton = UIManager.Instance.GetUI<BuildingInvenButton>();

            //invenButton.InvenMove();
            //base.Close(intialValue);
        }

        public override void Open(bool initialValue = false)
        {
            // base.Open(initialValue);
        }

    }


}
