using Project.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInvenButton : UIBase
{
    /// <summary>
    /// 건축 시스템에 사용 하는 인벤토리
    /// 열고 닫는 버튼
    /// </summary>
    Button buildingInvenButton;

    /// <summary>
    /// 인벤토리 버튼에 있는 화살표
    /// </summary>
    public Transform arrow;

    /// <summary>
    /// 건축 인벤토리 백그라운드 RectTransform
    /// </summary>
    public RectTransform invenBackGround;

    /// <summary>
    /// 현재 인벤토리로 작업중인지??
    /// 켜져있는지
    /// </summary>
    public bool isOpening = false;

    /// <summary>
    /// 화살표 이미지 ↑ 모양으로
    /// 해주는 회전값
    /// </summary>
    Quaternion arrowRot = Quaternion.Euler(180, 0, 0);

    /// <summary>
    /// 건축 인벤토리 홀더
    /// </summary>
    Transform invenHolder;

    public Vector3 orginPos = new Vector3();

    public override void Start()
    {
        base.Start();
        Initialize();
        orginPos = invenHolder.transform.position;
    }

    /// <summary>
    /// 초기화 해주는 함수
    /// </summary>
    private void Initialize()
    {
        buildingInvenButton = transform.GetComponent<Button>();
        invenHolder = transform.parent;

        buildingInvenButton.onClick.AddListener(() =>
        {
            InvenMove();
        });
    }

    /// <summary>
    /// 클릭 이벤트를 받았을때
    /// 인벤토리를 움직여줄 함수
    /// </summary>
    /// <param name="isOpening">인벤이 활성화되어잇는지 아닌지 체크하는 불타입변수</param>
    public void InvenMove()
    {
        if (!isOpening)
        {
            isOpening = !isOpening;
            arrow.rotation = Quaternion.identity;
            invenHolder.transform.localPosition = new Vector3(invenHolder.transform.localPosition.x, invenHolder.transform.localPosition.y + invenBackGround.rect.height,
            invenHolder.transform.localPosition.z);
        }
        else
        {
            isOpening = !isOpening;
            arrow.rotation = arrowRot;
            invenHolder.transform.localPosition = new Vector3(invenHolder.transform.localPosition.x, invenHolder.transform.localPosition.y - (invenBackGround.rect.height),
            invenHolder.transform.localPosition.z);
        }
    }

    /// <summary>
    /// TODO 03/22 이함수 적용하기 
    /// </summary>
    public void SetOriginPos()
    {
        isOpening = false;
        arrow.rotation = arrowRot;
        invenHolder.transform.localPosition = orginPos;
    }


}
