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
        mover ??= GetComponentInChildren<UIMover>();
        base.Start();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
                Close();
            else
                Open();
        }
    }

    public void Initialize(BoStore boStore)
    {
        this.boStore = boStore;
        // TODO npc로 부터 bostore에 대한 정보를 받아 초기화 시켜 슬롯을 동적 생성 시킨후에 작동하게 해주기

        // 슬롯 생성부터하기
        var poolManager = PoolManager.Instance;

        var sdStore = boStore.sdStore;

        var sd = GameManager.SD;

        Debug.Log(sdStore.saleItem.Length + "sdstore 상점 판매 아이템 배열 길이");

        for (int i = 0; i < sdStore.saleItem.Length; i++)
            storeSlots.Add(poolManager.GetPool<StoreSlot>().GetObject());

        for (int j = 0; j < sdStore.saleItem.Length; j++)
        {
            storeSlots[j].transform.SetParent(content);
            storeSlots[j].Initialize(new BoBuilditem(sd.sdBuildItems.Where(_ => _.index == sdStore.saleItem[j]).SingleOrDefault()));
        }
    }

    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);

        uiManager = UIManager.Instance;

        // 열고 닫을때 uimover랑 인벤토리랑 같이 열고 닫기 되게끔
        uiManager.GetUI<InventoryHandler>()?.Open();

        mover.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);

        uiManager = UIManager.Instance;

        // 열고 닫을때 uimover랑 인벤토리랑 같이 열고 닫기 되게끔

        uiManager.GetUI<InventoryHandler>()?.Close();

        mover.Close();
    }

}
