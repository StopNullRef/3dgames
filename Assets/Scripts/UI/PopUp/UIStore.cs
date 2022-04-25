using Project.DB;
using Project.Object;
using Project.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : UIBase
{
    public UIMover mover;
    public InventoryHandler inven;
    UIManager uiManager;

    public BoStore boStore;

    public Transform content;

    public List<StoreSlot> storeSlots = new List<StoreSlot>();

    public override void Start()
    {
        isOpen = false;
        isEscClose = true;
        mover ??= GetComponentInChildren<UIMover>();
        base.Start();
    }

    public void Initialize(BoStore boStore)
    {
        this.boStore = boStore;

        var poolManager = PoolManager.Instance;

        var sdStore = boStore.sdStore;

        var sd = GameManager.SD;
        // 무조건 추가 생성 초기화 가 아닌 현재 가지고있는거 체크 하고 나머지만큼 생성밑 초기화 시키기
        // 다시 초기화 할때 버그생김

        // 내가 필요한 슬롯 카운트를 센다
        var needSlotCount = boStore.sdStore.saleItem.Length - storeSlots.Count;

        // 0 보다 클경우 내가 풀에서 가져와서 추가해준다
        if (needSlotCount > 0)
        {
            for (int i = 0; i < needSlotCount; i++)
                storeSlots.Add(poolManager.GetPool<StoreSlot>().GetObject());
        }
        else if (needSlotCount < 0)
        {
            //0 보다 작은경우 storeSlots 이 상점 판매 목록 보다 크므로 풀에 다시 넣어준다
            needSlotCount = Mathf.Abs(needSlotCount);

            for (int i = needSlotCount; i <= 0; i--)
            {
                storeSlots.RemoveAt(i);
                poolManager.GetPool<StoreSlot>().PoolReturn(storeSlots[i]);
            }
        }

        for (int j = 0; j < sdStore.saleItem.Length; j++)
        {
            storeSlots[j].transform.SetParent(content);
            storeSlots[j].gameObject.SetActive(true);
            storeSlots[j].Initialize(new BoBuildItem(sd.sdBuildItems.Where(_ => _.index == sdStore.saleItem[j]).SingleOrDefault()));
        }
    }

    public override void Open(bool initialValue = false)
    {
        // 열고 닫을때 uimover랑 인벤토리랑 같이 열고 닫기 되게끔
        uiManager = UIManager.Instance;
        uiManager.GetUI<InventoryHandler>()?.Open();

        mover.Open();

        base.Open(initialValue);
    }

    public override void Close(bool intialValue = false)
    {

        uiManager = UIManager.Instance;

        // 열고 닫을때 uimover랑 인벤토리랑 같이 열고 닫기 되게끔

        uiManager.GetUI<InventoryHandler>()?.Close();

        var pool = PoolManager.Instance.GetPool<StoreSlot>();

        // 닫을 때 슬롯을 상점 판매 목록을 다시 리턴 시켜줌


        mover.Close();
        base.Close(intialValue);
        if (storeSlots.Count != 0)
        {
            for (int i = storeSlots.Count - 1; i <= 0; i--)
            {
                pool.PoolReturn(storeSlots[i]);
                storeSlots.RemoveAt(i);
            }
        }
    }

}
