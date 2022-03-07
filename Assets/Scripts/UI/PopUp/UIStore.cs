using Project.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIStore : UIBase
{
    public UIMover mover;
    public InventoryHandler inven;
    UIManager uiManager;
    SDStore sd;

    // TODO 0307
    // 상점 구현하고 클릭시 작동하게 끔 만들기

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

    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);

        uiManager = UIManager.Instance;

        uiManager.GetUI<InventoryHandler>()?.Open();
        uiManager.GetUI<UIMover>("Store")?.Open();
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);

        uiManager = UIManager.Instance;

        uiManager.GetUI<InventoryHandler>()?.Close();
        uiManager.GetUI<UIMover>("Store")?.Close();
    }

}
