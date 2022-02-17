using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour, IDragHandler,IBeginDragHandler
{
    Vector2 distance; // ���콺 Ŭ���� ��ġ�� �κ��丮 middleTop �κа��� �Ÿ�

    Vector3 originPos; // ���� �κ��丮 ��ġ

    public GameObject holder;




    // Start is called before the first frame update
    void Start()
    {
        holder = transform.parent.gameObject;
        originPos = holder.transform.position;
    }


    // �κ��丮�� �����ٰ� �������� ��ġ�� �ٽ� ����� ó����ġ�� ���ư������ִ� �Լ�
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
        //�״�� eventData�� �ٷ� ������ �κ��丮 middle top anchor�� �����ǹǷ�
        //Ŭ���� ��ġ�� �������� �־��༭ middle top anchor�� �ƴ� ���� ���콺 Ŭ���� ��ġ �������� �����̰�����
        holder.transform.position = (eventData.position + distance);
    }

}
