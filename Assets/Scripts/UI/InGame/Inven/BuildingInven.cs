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
                currentSelectSlot = (BuildingInvenSlot)slots[0];

            GameManager.InputEvent.InputEventRigist(KeyCode.Tab, () => SelectSlot());

            contentOrginPos = contentPos = contentRect.anchoredPosition;

            //선택된 슬롯을 찾는다
            var selectSlot = slots.Where(_ => _.isSelect == true).SingleOrDefault();

            // 만약 없을 경우 제일 처음에 있는 슬롯을 선택된 슬롯을 설정
            if (selectSlot == null)
                slots[0].isSelect = true;

            foreach (BuildingInvenSlot slot in slots)
                slot.SelectImageOnOff();

            SlotRefresh();
        }

        /// <summary>
        /// 슬롯리스트 등록하는 함수
        /// </summary>
        protected void SlotListRegist()
        {
            transform.GetComponentsInChildren(slots);
        }

        void AllSelectOnOff()
        {
            // 전체 슬롯에서 선택된 슬롯만 활성화 시켜줌
            foreach (BuildingInvenSlot slot in slots)
            {
                slot.SelectImageOnOff();
            }
        }

        /// <summary>
        /// 슬롯을 선택하게 해주는 함수
        /// </summary>
        void SelectSlot()
        {
            var selectSlot = slots.Where(_ => _.isSelect == true).SingleOrDefault();
            selectSlot.isSelect = false;

            // 슬롯이 마지막 번호라면 다시 처음 인벤슬롯을 선택시켜준다
            // 인덱스 값을 기준으로 하므로 listCount에 1을 빼준다
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

                // 현재 선택된 슬롯이 비어있지 않다면 슬롯에 있는 건물을 생성시켜줌
                if(currentSelectSlot.sd.index != 0)
                {
                    ingame.BuildingSystem.RemoveObject();
                    ingame.BuildingSystem.CreateObject(currentSelectSlot);
                }
                else
                {
                    // 여기에 들어왔다는것은 슬롯이 비어있단것이므로 현재 게임에 있는 건물을 다시 pool에 넣어줌
                    ingame.BuildingSystem.RemoveObject();
                }
            }
        }


        /// <summary>
        /// 인벤에 아이템을 추가해주는 함수
        /// </summary>
        /// <param name="buildItem"></param>
        public void AddBuildItem(SDBuildItem buildItem, int count)
        {
            var slots = base.slots.Cast<BuildingInvenSlot>();    //as List<BuildingInvenSlot>;

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

            invenButton.SetOriginPos();
            //base.Close(intialValue);
        }

        public override void Open(bool initialValue = false)
        {
            // 자식이라서 buildinginven 닫을때 alpha값이 0이되지만
            // 다시 열때는 slot들은 uibase를 상속받지 않으므로 alpha값을 다시 1로 바꿔줘야된다
            base.Open(initialValue);
            foreach (BuildingInvenSlot slot in slots)
                slot.IconImageOn();
        }

        /// <summary>
        /// 건물 오브젝트를 현재 내가 가지고 있는 아이템 만큼
        /// 풀에 등록 하는 함수
        /// </summary>
        public void BuildPoolRigist()
        {
            var poolManager = PoolManager.Instance;
            var itemPool = poolManager.GetPool<BuildItem>();
            var resourceManager = ResourceManager.Instance;

            // 건물 인벤이 비어있으면 풀에 등록해줄 필요가 없으므로
            // 리턴 시켜준다.
            // TODO 04/13 여기 리턴시켜주는 조건 수정
            // 건물은 pool에서 잘있음 이제 짓는거 구현해야됨
            // 슬롯이 비어있다는걸 어떻게 체크?
            if (!HaveItem())
                return;

            var haveItemSlots = HaveItemList();

            // 만약 풀을 찾았는데 없다면 만들어 준다
            // 풀이 없다는 것은 건물 객체를 부른적이 없다는 것임으로
            // 풀을 만들어주고 각 객체에 초기화를 해줌
            if (itemPool == null)
            {
                // 로드풀러블 오브젝트로 가져오는데 구별을 어떻게 할것인가..?
                // 건축물 구별법..

                //RigistPool(slotList.Count);

                for (int i = 0; i < haveItemSlots.Count; i++)
                {
                    //slotlist가 slotbase 라서 buildinginvenSlot으로 변환 후
                    BuildingInvenSlot slot = haveItemSlots[i];
                    // 그정보로 loadpoolableobject를 통해서 풀 등록 해줌

                    // 아이템이 비어져 있는 곳에도 리소스 패스 들어가서 안됨 슬롯 리스트 갯수만큼 하는 것이 아닌 가지고 있을대
                    // 가져오게끔 만들어야됨
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], (int)haveItemSlots[i].count);
                }

                itemPool = poolManager.GetPool<BuildItem>();

                PoolInit();

            }
            else
            {
                // 여기에 들어왔다는 것은 이미 처음에 등록을 했음
                // 처음에 등록했으니 부족한 만큼만 불러주면 됨
                for (int i = 0; i < base.slots.Count; i++)
                {
                    // 풀에서 슬롯과 같은 sd정보를 가지고 있는 객체의 갯수를 찾는다
                    // 그후 현재 sd정보가 같은 갯수랑 실제 item 가지고 있는 갯수를 비교하여
                    // 그 차이만큼 추가해준다
                    BuildingInvenSlot slot = base.slots[i] as BuildingInvenSlot;
                    int count = itemPool.Pool.Where(_ => _.boItem.sdBuildItem.index == slot.sd.index).ToList().Count;

                    // 갯수 차이가 0이거나 그보다 작을 경우 추가로 pool에서 등록을 해줄 필요가 없음으로
                    // 리턴 시켜준다
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

            // 풀에 있는 아이템 객체 초기화해주는 지역함수
            void PoolInit()
            {
                // 여기서 부터는 무조건 풀이 등록이 되어있음
                // 아이템 풀에서 가져온 아이템들에 정보를 초기화
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
