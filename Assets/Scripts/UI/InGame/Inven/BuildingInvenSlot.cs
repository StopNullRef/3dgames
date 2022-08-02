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

    /// <summary>
    /// sd데이터를 슬롯에 추가하는 함수
    /// </summary>
    /// <param name="sd">추가할 StaticData</param>
    /// <param name="count">추가할 아이템의 갯수</param>
    public void AddItem(SDBuildItem sd, int count)
    {
        this.sd = sd;
        this.count += count;
        SlotRefresh();
    }

    /// <summary>
    /// 아이템 슬롯에 있는 정보에 따라 이미지와 
    /// 아이템 갯수 text를 UI에 설정해주는 함수 
    /// </summary>
    public void SlotRefresh()
    {
        if (sd == null || sd.index == 0)
        {
            // 여기에 들어왔다는것은 해당 슬롯에 아이템이 비어있다는것
            // 해당 슬롯에 비어있는 슬롯 이미지를 넣어줌
            itemIconImage.sprite = IngameManager.Instance.buildingInvenSlot;
            itemCountText.text = count.ToString();
        }
        else
        {
            // 여기에 들어왔다는 것은 해등 슬롯에 아이템이 존재 한다는 것
            // 해당 아이템 정도에 맞게 슬롯이미지를 넣어줌
            itemIconImage.sprite = Resources.Load<Sprite>(sd.resourcePath[0]);
            itemCountText.text = count.ToString();
        }
    }

    /// <summary>
    /// 슬롯에 대한 정보를 비워주는 함수
    /// </summary>
    private void SlotClear()
    {
        // 슬롯에 count가 0일때 아이템이 존재하지 않으므로 해당 슬롯에 대한 SDData를 비워주는 함수

        

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

    public override float Count
    {
        get => base.Count;
        set
        {
            base.Count = value;
            if (Count == 0)
            {
                //TODO 08/01
                //
                //
                //여기 아이템 갯수 0개일때 슬롯 비워주는거 다시 설정하기
                //this.sd.index = 0;
                SlotRefresh();
            }
        }
    }

}
