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
        // TODO npc�� ���� bostore�� ���� ������ �޾� �ʱ�ȭ ���� ������ ���� ���� ��Ų�Ŀ� �۵��ϰ� ���ֱ�

        // ���� ���������ϱ�
        var poolManager = PoolManager.Instance;

        var sdStore = boStore.sdStore;

        var sd = GameManager.SD;

        Debug.Log(sdStore.saleItem.Length + "sdstore ���� �Ǹ� ������ �迭 ����");

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

        // ���� ������ uimover�� �κ��丮�� ���� ���� �ݱ� �ǰԲ�
        uiManager.GetUI<InventoryHandler>()?.Open();

        mover.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);

        uiManager = UIManager.Instance;

        // ���� ������ uimover�� �κ��丮�� ���� ���� �ݱ� �ǰԲ�

        uiManager.GetUI<InventoryHandler>()?.Close();

        mover.Close();
    }

}
