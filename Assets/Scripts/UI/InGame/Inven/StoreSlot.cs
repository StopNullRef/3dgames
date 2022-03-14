using Project.DB;
using Project.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class StoreSlot : MonoBehaviour, IPoolableObject
    {
        // TODO 03/08 버튼클릭으로 구현하지 말고
        // 마우스가 해당 범위안에 들어왔을때
        // 마우스 우클릭이냐 좌클릭이냐 에따라 다르게 처리
        // ex) 좌클릭시 한번만 구매 우클릭이 임이의 창이 떠서 몇개 구매할건지 입력후 구매

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
            //TODO 03/14 구현하기
            //현재 이코드자체가 작동하시않음
            // store에 uistore가 들어가지않음 
            // 아이템
            // 인덱스 아이템이름 리소스 패스 0,1 0 아이콘 1 프리팹오브젝트위치 cost 재료 아이템, 갯수

            Debug.Log("StoreSlotInitil");

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
        }

        public void SetSlot(BoBuilditem buildItem)
        {
            Debug.Log("SetStoreSlot");
            var sdItem = buildItem.sdBuildItem;

            var resourceManager = ResourceManager.Instance;

            var inven = UIManager.Instance.GetUI<InventoryHandler>();

            saleImage = resourceManager.ReourceLoad<Image>(sdItem.resourcePath[0]);
            saleItemName.text = sdItem.name;

            // 재료 이미지를 어디서 가지고있나? 해당 정보 넣어주기
            costImage = resourceManager.ReourceLoad<Image>(sdItem.resourcePath[2]);
            costText.text = sdItem.cost[1].ToString();

            haveImage = costImage;
            haveText.text = HavenItem().ToString();


            // 유저가 보유한 아이템갯수를 리턴해주는 함수
            int HavenItem()
            {
                var haveCostItem = inven.itemSlots.Where(_ => _.itemInfo.itemCode == sdItem.index).ToList();
                int count = 0;

                foreach(var item in haveCostItem)
                {
                    count += item.itemCount;
                }

                return count;
            }

        }
    }
}
