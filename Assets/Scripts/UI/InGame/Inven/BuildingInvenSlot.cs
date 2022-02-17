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
    public GameObject selectImage;


    /// <summary>
    /// SelectImage를 해당 slot의 불타입변수를 이용해
    /// 활성화 비활성화해주는 함수
    /// </summary>
    public void SelectImageOnOff()
    {
        selectImage.SetActive(isSelect);
    }

}
