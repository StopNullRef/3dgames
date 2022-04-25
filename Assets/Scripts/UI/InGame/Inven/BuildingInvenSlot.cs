using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInvenSlot : SlotBase
{

    /// <summary>
    /// 해당 슬롯이 선택 되었다는 것을 보여주기 위한
    /// 화살표 이미지를 갖는 GameObject
    /// </summary>
    public Image selectImage;

    [SerializeField]
    public SDBuildItem sd;

    /// <summary>
    /// SelectImage를 해당 slot의 불타입변수를 이용해
    /// 활성화 비활성화해주는 함수
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
    // 아이콘 이미지 알파값 1로 만들어주는 함수
    /// </summary>
    public void IconImageOn()
    {
        itemIconImage.color = new Color(itemIconImage.color.r, itemIconImage.color.g, itemIconImage.color.b, 1);
    }

    /// <summary>
    /// 현재 슬롯이 아이템을 가지고 있는지 아닌지 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsHaveItem()
    {
        if (sd != null && count != 0)
            return true;

        return false;
    }
}
