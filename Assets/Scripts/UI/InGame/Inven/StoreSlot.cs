using Project.DB;
using Project.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    public class StoreSlot : MonoBehaviour, IPoolableObject, IPointerEnterHandler
    {
        // TODO 03/14 버튼클릭으로 구현하지 말고
        // 마우스가 해당 범위안에 들어왔을때
        // 마우스 우클릭이냐 좌클릭이냐 에따라 다르게 처리
        // ex) 좌클릭시 한번만 구매 우클릭이 임이의 창이 떠서 몇개 구매할건지 입력후 구매
        // homeScene현재 발판 위치 안맞는거 수정
        // 마우스 커서 포인트 위치 안맞는거 수정
        // 상점 UI를 WorldUICanvas 만들어서 따로 작동하게끔 바꿔야됨

        public Image saleImage;
        public Text saleItemName;

        public Image haveImage;
        public Text haveText;

        public Image costImage;
        public Text costText;

        public bool CanRecycle { get; set; }

        BoBuilditem saleItem;

        public void Initialize(BoBuilditem buildItem)
        {
            saleItem = buildItem;
            SetSlot(buildItem);
        }

        public void SlotRefresh()
        {
            var sdItem = saleItem.sdBuildItem;

            var resourceManager = ResourceManager.Instance;

            saleImage = resourceManager.ReourceLoad<Image>(sdItem.resourcePath[0]);
            saleItemName.text = sdItem.name;

            // 재료 이미지를 어디서 가지고있나? 해당 정보 넣어주기
            costImage = resourceManager.ReourceLoad<Image>(sdItem.resourcePath[2]);
            costText.text = sdItem.cost[1].ToString();
            SetHaveCostColor(HavenItem().ToString());
        }

        public void SetSlot(BoBuilditem buildItem)
        {
            var sdItem = buildItem.sdBuildItem;

            var resourceManager = ResourceManager.Instance;

            var inven = UIManager.Instance.GetUI<InventoryHandler>();

            var sprite = Resources.Load<Sprite>(sdItem.resourcePath[0]);

            saleImage.sprite = sprite;
            saleItemName.text = sdItem.name;

            // 재료 이미지를 어디서 가지고있나? 해당 정보 넣어주기
            // 0 판매하는 실제 아이템 아이콘
            // 1 판매되는 아이템 실제 프리팹 객체
            // 2 구매할수있는 재료 아이템 아이콘
            costImage.sprite = Resources.Load<Sprite>(sdItem.resourcePath[2]);
            costText.text = sdItem.cost[1].ToString();

            haveImage.sprite = costImage.sprite;
            SetHaveCostColor(HavenItem().ToString());
        }
        // 유저가 보유한 아이템갯수를 리턴해주는 함수
        int HavenItem()
        {
            //TODO 03/15 현재 제대로 못찾음
            var sdItem = saleItem.sdBuildItem;
            var inven = UIManager.Instance.GetUI<InventoryHandler>();
            var haveCostItem = inven.itemSlots.Where(_ => _.itemInfo.itemCode == sdItem.cost[0]).ToList();
            int count = 0;

            foreach (var item in haveCostItem)
            {
                count += item.itemCount;
            }

            return count;
        }
        /// <summary>
        /// 구매가능한지 체크해주는 함수
        /// </summary>
        /// <returns></returns>
        bool IsBuy()
        {
            int haveItemCount = HavenItem();
            int costItemCount = Convert.ToInt32(costText.text);

            if (haveItemCount > costItemCount)
                return true;

            return false;
        }

        /// <summary>
        /// 가지고 있는 갯수 텍스트의 색을 바꿔주는 함수
        /// </summary>
        void SetHaveCostColor(string text)
        {
            // #ff0000ff 빨강
            // #000000ff 검정
            // 구매가 가능하면 텍스트 색깔을 검정 불가능하면 빨강
            if (IsBuy())
                haveText.text = $"<color=#000000ff>{text}</color>";
            else
                haveText.text = $"<color=#ff0000ff>{text}</color>";

        }

        /// <summary>
        /// 마우스 위치에 해당 UI가 있을때 들어오는 이벤트 함수
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 0 좌 1 중앙 2 우
            if (Input.GetMouseButtonDown(0))
            {
                BuyOne();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                BuyMany();
            }
        }


        /// <summary>
        /// 한번만 구매할때 작동하는 함수
        /// </summary>
        private void BuyOne()
        {
            if (IsBuy())
            {
                // 구매가 가능하다면 바로 구매되게끔


            }
            else
            {
                // 불가능하다면 구매가 불가능하다고 알림창 뜨게하기
            }
        }

        /// <summary>
        /// 여러개 구매할때 작동하는 함수
        /// </summary>
        private void BuyMany()
        {

        }


        /// <summary>
        /// 아이템 지불하기
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerator PayItem(int count)
        {

            var invenSlots = UIManager.Instance.GetUI<InventoryHandler>().itemSlots;

            // itemslot에 cost아이템이있는 슬롯 리스트를 찾고 해당 리스트에서 cost비용 이상으로
            // cost 0 구매할때 필요한 아이템 코드
            var haveCostItemSlots = invenSlots.Where(_ => _.itemInfo.itemCode == saleItem.sdBuildItem.cost[0]).ToList();

            // 해당 물건 구매하는 데 필요한 아이템 갯수
            int costCount = saleItem.sdBuildItem.cost[1];

            // 차감된수
            int deductionCount =0;

            //TODO 03/16 슬롯에서 차감하는 함수 써서 작동하게 하기
            while (count != deductionCount)
            {
                //var haveSlot = haveCostItemSlots.




                yield return null;
            }

        }

    }
}

