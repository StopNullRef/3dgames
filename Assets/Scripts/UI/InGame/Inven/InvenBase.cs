using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Inven
{
    public class InvenBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        /// <summary>
        /// 해당 인벤에 모든 슬롯을 가지고 있는 리스트
        /// </summary>
        public List<SlotBase> slotList = new List<SlotBase>();


        /// <summary>
        /// 드래그 앤 드랍시 beginDragSlot에 이미지를
        /// 마우스 위치를 따라가게 끔 구현을 하기 위해 받아둘 변수
        /// </summary>
        protected Image dragSlotImage;

        /// <summary>
        /// 마우스 처음 클릭시 들어올 슬롯 변수
        /// </summary>
        protected SlotBase beginDragSlot;

        /// <summary>
        /// 마우스 드래그 끝날때 넣을 슬롯 변수
        /// </summary> 
        protected SlotBase endDragSlot;

        /// <summary>
        /// 인벤토리를 벗어났는지 체크해줄 inventory RectTransform
        /// </summary>
        protected RectTransform invenRect;

        /// <summary>
        /// 해당 인벤토리가 활성화 되어있는지 안되어있는지 
        /// 체크 하는 변수
        /// </summary>
        public bool invenOnOff;

        /// <summary>
        /// 드래그앤 드랍 할때 사용해줄 임시 슬롯 이미지
        /// </summary>
        public Image tempSlotImage;
        /// <summary>
        /// 임시슬롯 처음 위치
        /// </summary>
        protected Vector3 tempSlotOriginPos;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // 현재 마우스위에 UI가 없다면 작동x
            if (!CanDragCheck())
                return;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {

        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            // 현재 마우스위에 UI가 없다면 작동x
            if (!CanDragCheck())
                return;
        }

        /// <summary>
        /// 드래그가 가능한지 체크해주는 함수
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        protected virtual bool CanDragCheck()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        // 슬롯 정보 바꿔주는 함수
        protected virtual void SlotSwap()
        {
            SlotBase tempSlot = beginDragSlot;

            beginDragSlot = endDragSlot;
            endDragSlot = beginDragSlot;
        }

        protected virtual void SlotRefresh()
        {
            foreach (var slot in slotList)
                slot.SetSlotImage();
        }

        protected  void DragSlotClear()
        {
            beginDragSlot = null;
            endDragSlot = null;
        }
    }
}
