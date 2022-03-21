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
        /// 현재 건물을 지을수 있는지 아닌지 체크하는 함수
        /// </summary>
        private bool canBuild = false;

        public BuildingInvenSlot currentSelectSlot;

        /// <summary>
        /// 인벤토리가 보여지는 것은 content좌표 기준으로 감으로 여기 에 받아준다
        /// </summary>
        public RectTransform contentRect;

        /// <summary>
        /// 맨처음 content의 초기값
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


            var inputEvent = GameManager.InputEvent;

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            SlotRefresh();
            SetMaskable();
        }


        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            // 드래그 할때 현재 위치에 있는 객체에 slot을 받는다
            //beginDragSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<BuildingInvenSlot>();
            beginDragSlot = slotList[GetSlotNum(eventData.pointerCurrentRaycast.gameObject)];


            // 슬롯을 GetComponent했을때 null 이면 드래그된게
            // 슬롯을 드래그한게 아니므로 리턴 시켜줘서 작동x
            if (beginDragSlot == null)
                return;


            dragSlotImage = beginDragSlot.itemIconImage;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            if (beginDragSlot == null)
                return;

            // 드래그 될때 해당 이미지를 마우스 위치에로 따라가게끔 해준다
            dragSlotImage.transform.position = eventData.position;

        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            // 드래그가 끝날때 마우스에 있는 슬롯정보를 받는다
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
        /// 슬롯리스트 등록하는 함수
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slotList);
        }

        /// <summary>
        /// 슬롯을 선택하게 해주는 함수
        /// </summary>
        void SelectSlot()
        {

            //전체 슬롯 리스트를 순회해서 
            foreach (BuildingInvenSlot slot in slotList)
            {

                // 일반적인 상황일때는 다음거를 활성화 시켜준다
                if (slot.isSelect)
                {
                    slot.isSelect = !slot.isSelect;

                    // 슬롯이 마지막 번호라면 다시 처음 인벤슬롯을 선택시켜준다
                    // 인덱스 값을 기준으로 하므로 listCount에 1을 빼준다
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

            // 전체 슬롯에서 선택된 슬롯만 활성화 시켜줌
            foreach (BuildingInvenSlot slot in slotList)
            {
                slot.SelectImageOnOff();
            }

        }

        /// <summary>
        /// 선택된 슬롯기준으로 컨텐츠를 옮겨주는 함수
        /// </summary>
        /// <Param>선택된 슬롯</Param>
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
        /// 슬롯의 번호를 반환해주는 함수
        /// </summary>
        /// <param name="go">이벤트 시스템으로 받은 
        /// 마우스위에 레이캐스트 맞는 GameObject</param>
        /// <returns></returns>
        private int GetSlotNum(GameObject go)
        {
            // slot의 이름들은 slot1,slot2,slot3... 이런식으로
            // 올라가는 규칙을 가지고있으므로 slot을 빼고 뒤에있는 값을
            // 가져와서 list의 번호를 받는다
            var name = go.name;

            name = name.Substring(name.LastIndexOf('t') + 1);
            // 내가 필요한 번호는 slotlist의 인덱스 번호에 넣어줄 것임으로
            // 1을 빼준다
            return (int.Parse(name) - 1);
        }


        // 뷰포트 부분에서 minX, maxX값을 가져와서 그범위를 넘었다면 maskable체크 아니라면 체크해제해주는 함수 만들기

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
        /// 인벤에 아이템을 추가해주는 함수
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem)
        {
            //TODO 03/21 여기 아이템 add해주는 거 구현
            // buildinginvenslotonoff하는거 보면 객체 활성화 비활성화임 그거 고치기
        }
    }
}
