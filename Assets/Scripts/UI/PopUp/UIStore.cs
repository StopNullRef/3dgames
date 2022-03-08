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

    private void Initialize(Store store)
    {
        this.boStore = store.boStore;
        // TODO npc�� ���� bostore�� ���� ������ �޾� �ʱ�ȭ ���� ������ ���� ���� ��Ų�Ŀ� �۵��ϰ� ���ֱ�
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
