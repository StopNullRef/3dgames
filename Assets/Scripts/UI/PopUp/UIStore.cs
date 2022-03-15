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

        for (int i = 0; i < sdStore.saleItem.Length; i++)
            storeSlots.Add(poolManager.GetPool<StoreSlot>().GetObject());

        for (int j = 0; j < sdStore.saleItem.Length; j++)
        {
            storeSlots[j].transform.SetParent(content);
            storeSlots[j].gameObject.SetActive(true);
            storeSlots[j].Initialize(new BoBuilditem(sd.sdBuildItems.Where(_ => _.index == sdStore.saleItem[j]).SingleOrDefault()));
        }
    }

    public override void Open(bool initialValue = false)
    {
        // ¿­°í ´ÝÀ»¶§ uimover¶û ÀÎº¥Åä¸®¶û °°ÀÌ ¿­°í ´Ý±â µÇ°Ô²û
        uiManager = UIManager.Instance;
        uiManager.GetUI<InventoryHandler>()?.Open();

        mover.Open();

        base.Open(initialValue);
    }

    public override void Close(bool intialValue = false)
    {

        uiManager = UIManager.Instance;

        // ¿­°í ´ÝÀ»¶§ uimover¶û ÀÎº¥Åä¸®¶û °°ÀÌ ¿­°í ´Ý±â µÇ°Ô²û

        uiManager.GetUI<InventoryHandler>()?.Close();

        mover.Close();
        base.Close(intialValue);
    }

}
