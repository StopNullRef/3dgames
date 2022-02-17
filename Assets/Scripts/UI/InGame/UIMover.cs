using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour, IDragHandler,IBeginDragHandler
{
    Vector2 distance; // 마우스 클릭된 위치랑 인벤토리 middleTop 부분과의 거리

    Vector3 originPos; // 원래 인벤토리 위치

    public GameObject holder;




    // Start is called before the first frame update
    void Start()
    {
        holder = transform.parent.gameObject;
        originPos = holder.transform.position;
    }


    // 인벤토리가 꺼졋다가 켜졌을때 위치를 다시 제대로 처음위치로 돌아가게해주는 함수
    void SetOriginPosInven()
    {
        if (!gameObject.activeInHierarchy)
        {
            holder.transform.position = originPos;
        }
    }

    private void OnDisable()
    {
        SetOriginPosInven();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 tempvec = holder.transform.position;
        distance = (tempvec - eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //그대로 eventData를 바로 넣으면 인벤토리 middle top anchor에 고정되므로
        //클릭된 위치를 더해준후 넣어줘서 middle top anchor가 아닌 위쪽 마우스 클릭된 위치 기준으로 움직이게해줌
        holder.transform.position = (eventData.position + distance);
    }

}
