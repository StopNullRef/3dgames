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

    public Color itemColor; //���Կ� �ִ� �������̹��� ���̰� �ϴ� �÷���

    public Color itemNoView;

    

    private void OnEnable()
    {
        itemInfo = ItemManager.Instance.itemList[(int)Define.ScriptableItem.None];

        itemCount = 0;

        itemColor = new Color(255, 255, 255, 255);

        itemNoView = new Color(255, 255, 255, 0);
    }

    /// <summary>
    /// �ش� ���Կ� �Ű� ������ ����
    /// ������������
    /// ������ �־� �ִ� �Լ�
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void SlotRefresh(ItemScriptableObj item, int count)
    {
        itemImage.sprite = item.sprite;

        // ���� ���� �ڽ����� ������ ������ �̹����� �÷��ִµ� ó���� ���İ� 0���� �س���
        // �̹����� �Ⱥ��̰Բ��ߴٰ� �������� ���Դٸ� ���� ���� �÷��� �������� ���̰� ����
        itemImage.color = itemColor;

        itemCountText.text = count.ToString();
    }

    /// <summary>
    /// ���Կ� �̹��� �ؽ�Ʈ �ٲ��ִ� �Լ�
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

        // ���� ���� �ڽ����� ������ ������ �̹����� �÷��ִµ� ó���� ���İ� 0���� �س���
        // �̹����� �Ⱥ��̰Բ��ߴٰ� �������� ���Դٸ� ���� ���� �÷��� �������� ���̰� ����
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
    /// ������ ����Ǵ°� �߰��ؼ� �������ִ� �Լ�
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
