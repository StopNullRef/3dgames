using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBase : MonoBehaviour
{
    /// <summary>
    /// 아이템에 대한 정보를 갖는 ScriptableObject
    /// </summary>
    public ItemInfoBase itemInfo;

    /// <summary>
    /// 아이템 아이콘 이미지
    /// </summary>
    public Image itemIconImage;

    /// <summary>
    /// 아이템 갯수를 나타내는 text
    /// </summary>
    public Text itemCountText;

    /// <summary>
    /// 자기가 선택되었는지 체크하는 불타입 변수
    /// </summary>
    public bool isSelect = false;

    /// <summary>
    /// 해당 슬롯 아이콘이 보이는 색상
    /// </summary>
    protected Color visible = new Color(255, 255, 255, 255);

    /// <summary>
    /// 해당 슬롯 아이콘이 안보이는 색상
    /// </summary>
    protected Color invisible = new Color(255, 255, 255, 0);

    /// <summary>
    /// 아이템 갯수
    /// </summary>
    public float count;

    /// <summary>
    /// 슬롯이미지를 설정해주는 함수
    /// </summary>
    public void SetSlotImage()
    {
        // 아이템 정보가 없다면 아이템이 없다는 것이므로 
        // 아이콘을 안보이게끔 만들어준다
        if (itemInfo == null)
            itemIconImage.color = invisible;
        // 아이템 정보가 있을때는 보이게 설정
        // 아이템 아이콘 이미지도 보이게 하기
        else
        {
            itemIconImage.color = visible;
            itemIconImage.sprite = itemInfo.sprite;
        }

    }

    public virtual float Count
    {
        get => count;
        set
        {
            count = value;
            itemCountText.text = count.ToString();
        }
    }
}
