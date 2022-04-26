using Project.Object;
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
                currentSelectSlot = (BuildingInvenSlot)slots[0];

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            //���õ� ������ ã�´�
            var selectSlot = slots.Where(_ => _.isSelect == true).SingleOrDefault();

            // ���� ���� ��� ���� ó���� �ִ� ������ ���õ� ������ ����
            if (selectSlot == null)
                slots[0].isSelect = true;

            foreach (BuildingInvenSlot slot in slots)
                slot.SelectImageOnOff();

            SlotRefresh();
        }

        /// <summary>
        /// ���Ը���Ʈ ����ϴ� �Լ�
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slots);
        }

        void AllSelectOnOff()
        {
            // ��ü ���Կ��� ���õ� ���Ը� Ȱ��ȭ ������
            foreach (BuildingInvenSlot slot in slots)
            {
                slot.SelectImageOnOff();
            }
        }

        /// <summary>
        /// ������ �����ϰ� ���ִ� �Լ�
        /// </summary>
        void SelectSlot()
        {
            var selectSlot = slots.Where(_ => _.isSelect == true).SingleOrDefault();
            selectSlot.isSelect = false;

            // ������ ������ ��ȣ��� �ٽ� ó�� �κ������� ���ý����ش�
            // �ε��� ���� �������� �ϹǷ� listCount�� 1�� ���ش�
            if (selectSlot.transform.GetSiblingIndex() == slots.Count - 1)
            {
                SetSelectSlot(0);
                contentRect.anchoredPosition = contentOrginPos;
                AllSelectOnOff();
                return;
            }

            if (RectTransformUtility.RectangleContainsScreenPoint(viewPort, slots[slots.Count - 1].transform.position))
            {
                SetSelectSlot(selectSlot.transform.GetSiblingIndex() + 1);
                AllSelectOnOff();
                if (lastSlotViewPos == Vector2.zero)
                    lastSlotViewPos = new Vector2(contentRect.anchoredPosition.x - 50, contentRect.anchoredPosition.y);

                contentRect.anchoredPosition = lastSlotViewPos;
                return;
            }
            else
            {
                SetSelectSlot(selectSlot.transform.GetSiblingIndex() + 1);
                contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x - contentMoveDistance, contentRect.anchoredPosition.y);

            }


            AllSelectOnOff();

            void SetSelectSlot(int slotNum)
            {
                slots[slotNum].isSelect = true;
                var beforeSelectSlot = currentSelectSlot;
                currentSelectSlot = (BuildingInvenSlot)slots[slotNum];
                var ingame = IngameManager.Instance;

                // ���� ���õ� ������ ������� �ʴٸ� ���Կ� �ִ� �ǹ��� ����������
                if(currentSelectSlot.sd.index != 0)
                {
                    ingame.BuildingSystem.RemoveObject();
                    ingame.BuildingSystem.CreateObject(currentSelectSlot);
                }
                else
                {
                    // ���⿡ ���Դٴ°��� ������ ����ִܰ��̹Ƿ� ���� ���ӿ� �ִ� �ǹ��� �ٽ� pool�� �־���
                    ingame.BuildingSystem.RemoveObject();
                }
            }
        }


        /// <summary>
        /// �κ��� �������� �߰����ִ� �Լ�
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem, int count)
        {
            var slots = base.slots.Cast<BuildingInvenSlot>();    //as List<BuildingInvenSlot>;

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

            invenButton.SetOriginPos();
            //base.Close(intialValue);
        }

        public override void Open(bool initialValue = false)
        {
            // �ڽ��̶� buildinginven ������ alpha���� 0�̵�����
            // �ٽ� ������ slot���� uibase�� ��ӹ��� �����Ƿ� alpha���� �ٽ� 1�� �ٲ���ߵȴ�
            base.Open(initialValue);
            foreach (BuildingInvenSlot slot in slots)
                slot.IconImageOn();
        }

        /// <summary>
        /// �ǹ� ������Ʈ�� ���� ���� ������ �ִ� ������ ��ŭ
        /// Ǯ�� ��� �ϴ� �Լ�
        /// </summary>
        public void BuildPoolRigist()
        {
            var poolManager = PoolManager.Instance;
            var itemPool = poolManager.GetPool<BuildItem>();
            var resourceManager = ResourceManager.Instance;

            // �ǹ� �κ��� ��������� Ǯ�� ������� �ʿ䰡 �����Ƿ�
            // ���� �����ش�.
            // TODO 04/13 ���� ���Ͻ����ִ� ���� ����
            // �ǹ��� pool���� ������ ���� ���°� �����ؾߵ�
            // ������ ����ִٴ°� ��� üũ?
            if (!HaveItem())
                return;

            var haveItemSlots = HaveItemList();

            // ���� Ǯ�� ã�Ҵµ� ���ٸ� ����� �ش�
            // Ǯ�� ���ٴ� ���� �ǹ� ��ü�� �θ����� ���ٴ� ��������
            // Ǯ�� ������ְ� �� ��ü�� �ʱ�ȭ�� ����
            if (itemPool == null)
            {
                // �ε�Ǯ���� ������Ʈ�� �������µ� ������ ��� �Ұ��ΰ�..?
                // ���๰ ������..

                //RigistPool(slotList.Count);

                for (int i = 0; i < haveItemSlots.Count; i++)
                {
                    //slotlist�� slotbase �� buildinginvenSlot���� ��ȯ ��
                    BuildingInvenSlot slot = haveItemSlots[i];
                    // �������� loadpoolableobject�� ���ؼ� Ǯ ��� ����

                    // �������� ����� �ִ� ������ ���ҽ� �н� ���� �ȵ� ���� ����Ʈ ������ŭ �ϴ� ���� �ƴ� ������ ������
                    // �������Բ� �����ߵ�
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], (int)haveItemSlots[i].count);
                }

                itemPool = poolManager.GetPool<BuildItem>();

                PoolInit();

            }
            else
            {
                // ���⿡ ���Դٴ� ���� �̹� ó���� ����� ����
                // ó���� ��������� ������ ��ŭ�� �ҷ��ָ� ��
                for (int i = 0; i < base.slots.Count; i++)
                {
                    // Ǯ���� ���԰� ���� sd������ ������ �ִ� ��ü�� ������ ã�´�
                    // ���� ���� sd������ ���� ������ ���� item ������ �ִ� ������ ���Ͽ�
                    // �� ���̸�ŭ �߰����ش�
                    BuildingInvenSlot slot = base.slots[i] as BuildingInvenSlot;
                    int count = itemPool.Pool.Where(_ => _.boItem.sdBuildItem.index == slot.sd.index).ToList().Count;

                    // ���� ���̰� 0�̰ų� �׺��� ���� ��� �߰��� pool���� ����� ���� �ʿ䰡 ��������
                    // ���� �����ش�
                    if (count <= 0)
                        return;

                    //RigistPool(count);
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], count);

                    PoolInit();
                }


            }

            bool HaveItem()
            {
                foreach (var slot in base.slots)
                {
                    if (slot.Count > 0)
                        return true;
                }

                return false;
            }

            // Ǯ�� �ִ� ������ ��ü �ʱ�ȭ���ִ� �����Լ�
            void PoolInit()
            {
                // ���⼭ ���ʹ� ������ Ǯ�� ����� �Ǿ�����
                // ������ Ǯ���� ������ �����۵鿡 ������ �ʱ�ȭ
                foreach (var item in itemPool.Pool)
                {
                    item.PoolInit();
                }
            }

            List<BuildingInvenSlot> HaveItemList()
            {
                List<BuildingInvenSlot> result = new List<BuildingInvenSlot>();

                foreach (BuildingInvenSlot slot in base.slots)
                {
                    if (slot.sd.index != 0)
                        result.Add(slot);
                }

                return result;
            }
        }
    }


}
