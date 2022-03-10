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
        base.Start();

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mover = UIManager.Instance.GetUI<UIMover>("Store");
            if (isOpen)
                Close();
            else
                Open();
        }
    }

    public void Initialize(Store store)
    {
        this.boStore = store.boStore;
        // TODO npc�� ���� bostore�� ���� ������ �޾� �ʱ�ȭ ���� ������ ���� ���� ��Ų�Ŀ� �۵��ϰ� ���ֱ�

        // ���� ���������ϱ�
        var poolManager = PoolManager.Instance;

        var sdStore = store.boStore.sdStore;

        var sd = GameManager.Instance.SD;


        for(int i=0; i < sdStore.saleItem.Length; i++)
            storeSlots.Add(poolManager.GetPool<StoreSlot>().GetObject());

        for (int j = 0; j < sdStore.saleItem.Length; j++)
        {
            storeSlots[j].transform.SetParent(content);
            storeSlots[j].Initilaize(new BoBuilditem(sd.sdBuildItems.Where(_=>_.index == sdStore.saleItem[j]).SingleOrDefault()));
        }
    }

    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);

        uiManager = UIManager.Instance;

        // ���� ������ uimover�� �κ��丮�� ���� ���� �ݱ� �ǰԲ�
        uiManager.GetUI<InventoryHandler>()?.Open();
        uiManager.GetUI<UIMover>("Store")?.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);

        uiManager = UIManager.Instance;

        // ���� ������ uimover�� �κ��丮�� ���� ���� �ݱ� �ǰԲ�

        uiManager.GetUI<InventoryHandler>()?.Close();
        uiManager.GetUI<UIMover>("Store")?.Close();
    }

}
