using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemScriptableObj itemInfo;

    public Image itemImage;

    public Text itemCountText;

    public int itemCount;

    public Color itemColor; //슬롯에 있는 아이템이미지 보이게 하는 컬러값

    public Color itemNoView;

    

    private void OnEnable()
    {
        itemInfo = ItemManager.Instance.itemList[(int)Define.ScriptableItem.None];

        itemCount = 0;

        itemColor = new Color(255, 255, 255, 255);

        itemNoView = new Color(255, 255, 255, 0);
    }

    /// <summary>
    /// 해당 슬롯에 매개 변수로 받은
    /// 아이템정보와
    /// 갯수를 넣어 주는 함수
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void SlotRefresh(ItemScriptableObj item, int count)
    {
        itemImage.sprite = item.sprite;

        // 현재 슬롯 자식으로 아이템 아이콘 이미지를 올려주는데 처음에 알파값 0으로 해놓고
        // 이미지가 안보이게끔했다가 아이템이 들어왔다면 알파 값을 올려서 아이콘을 보이게 해줌
        itemImage.color = itemColor;

        itemCountText.text = count.ToString();
    }

    /// <summary>
    /// 슬롯에 이미지 텍스트 바꿔주는 함수
    /// </summary>
    public void SlotRefresh()
    {
        if (itemInfo.itemCode == 0)
        {
            itemImage.sprite = itemInfo.sprite;
            itemImage.color = itemNoView;
            itemCount = 0;

            itemCountText.text = "";
            return;
        }

        itemImage.sprite = itemInfo.sprite;

        // 현재 슬롯 자식으로 아이템 아이콘 이미지를 올려주는데 처음에 알파값 0으로 해놓고
        // 이미지가 안보이게끔했다가 아이템이 들어왔다면 알파 값을 올려서 아이콘을 보이게 해줌
        itemImage.color = itemColor;

        itemCountText.text = itemCount.ToString();
    }

    public ItemSlot Copy()
    {
        ItemSlot clone = (ItemSlot)this.MemberwiseClone();
        return clone;
    }

    public void SlotInitialize()
    {
        itemInfo = ItemManager.Instance.itemList[(int)Define.ScriptableItem.None];

        itemCount = 0;
    }

    /// <summary>
    /// 아이템 드랍되는거 추가해서 설정해주는 함수
    /// </summary>
    /// <param name="addItem"></param>
    /// <param name="dropCount"></param>
    public void SlotInitialize(ObjInfo addItem,int dropCount)
    {
        this.itemInfo = addItem.ItemDrop;
        this.itemCount = dropCount;
        this.itemInfo = addItem.ItemDrop;
    }

    public void SlotInitialize(int dropCount)
    {
        this.itemCount += dropCount;
    }

}
