using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInvenSlot : SlotBase
{

    /// <summary>
    /// �ش� ������ ���� �Ǿ��ٴ� ���� �����ֱ� ����
    /// ȭ��ǥ �̹����� ���� GameObject
    /// </summary>
    public Image selectImage;

    [SerializeField]
    public SDBuildItem sd;

    /// <summary>
    /// SelectImage�� �ش� slot�� ��Ÿ�Ժ����� �̿���
    /// Ȱ��ȭ ��Ȱ��ȭ���ִ� �Լ�
    /// </summary>
    public void SelectImageOnOff()
    {
        selectImage.color = new Color(selectImage.color.r, selectImage.color.g, selectImage.color.b, IsSelectColor());
    }

    private float IsSelectColor()
    {
        if (isSelect)
            return 1;

        return 0;
    }


    public void AddItem(SDBuildItem sd, int count)
    {
        this.sd = sd;
        this.count += count;
        SlotRefresh();
    }

    public void SlotRefresh()
    {
        if(sd.index == 0)
        {
            itemIconImage.sprite = IngameManager.Instance.buildingInvenSlot;
        }

        itemIconImage.sprite = Resources.Load<Sprite>(sd.resourcePath[0]);
        itemCountText.text = count.ToString();
    }

    /// <summary>
    // ������ �̹��� ���İ� 1�� ������ִ� �Լ�
    /// </summary>
    public void IconImageOn()
    {
        itemIconImage.color = new Color(itemIconImage.color.r, itemIconImage.color.g, itemIconImage.color.b, 1);
    }

    /// <summary>
    /// ���� ������ �������� ������ �ִ��� �ƴ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool IsHaveItem()
    {
        if (sd != null && count != 0)
            return true;

        return false;
    }
}
