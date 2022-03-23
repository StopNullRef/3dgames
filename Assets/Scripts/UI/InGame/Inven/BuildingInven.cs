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
        /// 현재 건물을 지을수 있는지 아닌지 체크하는 함수
        /// </summary>
        private bool canBuild = false;

        public BuildingInvenSlot currentSelectSlot;

        /// <summary>
        /// 인벤토리가 보여지는 것은 content좌표 기준으로 감으로 여기 에 받아준다
        /// </summary>
        public RectTransform contentRect;

        public RectTransform viewPort;

        /// <summary>
        /// 맨처음 content의 초기값
        /// </summary>
        Vector3 contentOrginPos;

        Vector2 contentPos;

        private const float contentMoveDistance = 125f;

        Vector2 lastSlotViewPos;

        public override void Start()
        {
            //TODO 03/23 현재 씬변경시 tab키가 먹지않음
            base.Start();
            Initialize();
        }

        /// <summary>
        /// 초기화 작업
        /// </summary>
        void Initialize()
        {
            SlotListRegist();

            // currentSelectSlot을 키 입력을 통해 받아주고 있으므로
            // 맨처음에 키입력을 받지 않으면 1번째 슬롯이 선택되지 않으므로
            // 미리 초기화를 시켜준다
            if (currentSelectSlot == null)
                currentSelectSlot = (BuildingInvenSlot)slotList[0];

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            //선택된 슬롯을 찾는다
            var selectSlot = slotList.Where(_ => _.isSelect == true).SingleOrDefault();
            // 만약 없을 경우 제일 처음에 있는 슬롯을 선택된 슬롯을 설정
            if (selectSlot == null)
                slotList[0].isSelect = true;

            foreach (BuildingInvenSlot slot in slotList)
                slot.SelectImageOnOff();

            SlotRefresh();
        }

        /// <summary>
        /// 슬롯리스트 등록하는 함수
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slotList);
        }

        void AllSelectOnOff()
        {
            // 전체 슬롯에서 선택된 슬롯만 활성화 시켜줌
            foreach (BuildingInvenSlot slot in slotList)
            {
                slot.SelectImageOnOff();
            }
        }

        /// <summary>
        /// 슬롯을 선택하게 해주는 함수
        /// </summary>
        void SelectSlot()
        {
            var selectSlot = slotList.Where(_ => _.isSelect == true).SingleOrDefault();
            selectSlot.isSelect = false;
            // 슬롯이 마지막 번호라면 다시 처음 인벤슬롯을 선택시켜준다
            // 인덱스 값을 기준으로 하므로 listCount에 1을 빼준다
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
        /// 인벤에 아이템을 추가해주는 함수
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem, int count)
        {
            var slots = slotList.Cast<BuildingInvenSlot>();    //as List<BuildingInvenSlot>;

            // 슬롯중에 같은 아이템을 가지고 있는 슬롯이 있는지 찾는다
            var slot = slots.Where(_ => _.sd != null && _.sd.index == buildItem.index).FirstOrDefault();

            // 없는 경우 빈슬롯을 찾는다
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
