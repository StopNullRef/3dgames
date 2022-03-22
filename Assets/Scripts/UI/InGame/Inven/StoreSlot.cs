using Project.DB;
using Project.Inven;
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
    public class StoreSlot : MonoBehaviour, IPoolableObject, IPointerClickHandler
    {
        // TODO 
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
        /// 구매가능한지 체크해주는 함수
        /// </summary>
        /// <param name="count">구매할 아이템 갯수</param>
        /// <returns></returns>
        bool IsBuy(int count)
        {
            var haveItemCount = HavenItem();
            var costItemCount = Convert.ToInt32(costText.text) * count;

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
        /// 한번만 구매할때 작동하는 함수
        /// </summary>
        private void BuyOne()
        {
            if (IsBuy())
            {
                // 구매가 가능하다면 바로 구매되게끔
                PayItem(saleItem.sdBuildItem.cost[1]);
                //saleItem.sdBuildItem.cost[1]

            }
            else
            {
                // 불가능하다면 구매가 불가능하다고 알림창 뜨게하기
                Debug.Log("아이템 구매를 할수 없습니다");
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
        void PayItem(int count)
        {
            var invenSlots = UIManager.Instance.GetUI<InventoryHandler>().itemSlots;

            // itemslot에 cost아이템이있는 슬롯 리스트를 찾고 해당 리스트에서 cost비용 이상으로
            // cost 0 구매할때 필요한 아이템 코드
            var haveCostItemSlots = invenSlots.Where(_ => _.itemInfo.itemCode == saleItem.sdBuildItem.cost[0]).ToList();

            // 해당 물건 구매하는 데 필요한 아이템 갯수
            int costCount = saleItem.sdBuildItem.cost[1];

            var remain = haveCostItemSlots[0].DeductItemCount(count);

            // remain이 0이라는 것은 첫 슬롯에 차감할수 있는 만큼
            // 가지고있어서 더이상 찾을 필요가 없음
            if (remain == 0)
            {
                UIManager.Instance.GetUI<BuildingInven>().AddBuildItem(saleItem.sdBuildItem, 1);
                return;
            }

            for (int i = 1; i < haveCostItemSlots.Count; i++)
            {
                remain = haveCostItemSlots[i].DeductItemCount(remain);

                if (remain == 0)
                    break;
            }
            UIManager.Instance.GetUI<BuildingInven>().AddBuildItem(saleItem.sdBuildItem, 1);

            // TODO 03/21 buildingInven에서 item Add해주는거 함수 만들어서 넣기
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BuyOne();
            SlotRefresh();
        }
    }
}

