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
        /// �ش� �κ��� ��� ������ ������ �ִ� ����Ʈ
        /// </summary>
        public List<SlotBase> slotList = new List<SlotBase>();


        /// <summary>
        /// �巡�� �� ����� beginDragSlot�� �̹�����
        /// ���콺 ��ġ�� ���󰡰� �� ������ �ϱ� ���� �޾Ƶ� ����
        /// </summary>
        protected Image dragSlotImage;

        /// <summary>
        /// ���콺 ó�� Ŭ���� ���� ���� ����
        /// </summary>
        protected SlotBase beginDragSlot;

        /// <summary>
        /// ���콺 �巡�� ������ ���� ���� ����
        /// </summary> 
        protected SlotBase endDragSlot;

        /// <summary>
        /// �κ��丮�� ������� üũ���� inventory RectTransform
        /// </summary>
        protected RectTransform invenRect;

        /// <summary>
        /// �ش� �κ��丮�� Ȱ��ȭ �Ǿ��ִ��� �ȵǾ��ִ��� 
        /// üũ �ϴ� ����
        /// </summary>
        public bool invenOnOff;

        /// <summary>
        /// �巡�׾� ��� �Ҷ� ������� �ӽ� ���� �̹���
        /// </summary>
        public Image tempSlotImage;
        /// <summary>
        /// �ӽý��� ó�� ��ġ
        /// </summary>
        protected Vector3 tempSlotOriginPos;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // ���� ���콺���� UI�� ���ٸ� �۵�x
            if (!CanDragCheck())
                return;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {

        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            // ���� ���콺���� UI�� ���ٸ� �۵�x
            if (!CanDragCheck())
                return;
        }

        /// <summary>
        /// �巡�װ� �������� üũ���ִ� �Լ�
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        protected virtual bool CanDragCheck()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        // ���� ���� �ٲ��ִ� �Լ�
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
