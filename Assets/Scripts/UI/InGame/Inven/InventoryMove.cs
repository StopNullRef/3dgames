using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMove : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    Vector2 distance; // ���콺 Ŭ���� ��ġ�� �κ��丮 middleTop �κа��� �Ÿ�

    Vector3 invenOriginPos; // ���� �κ��丮 ��ġ

    public GameObject invenHodler;

    public GameObject inven;

    // Start is called before the first frame update
    void Start()
    {
        invenOriginPos = invenHodler.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SetOriginPosInven();
    }

    // �κ��丮�� �����ٰ� �������� ��ġ�� �ٽ� ����� ó����ġ�� ���ư������ִ� �Լ�
    void SetOriginPosInven()
    {
        if (!inven.activeInHierarchy)
        {
            invenHodler.transform.position = invenOriginPos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 tempvec = invenHodler.transform.position;
        distance = (tempvec - eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�״�� eventData�� �ٷ� ������ �κ��丮 middle top anchor�� �����ǹǷ�
        //Ŭ���� ��ġ�� �������� �־��༭ middle top anchor�� �ƴ� ���� ���콺 Ŭ���� ��ġ �������� �����̰�����
        invenHodler.transform.position = (eventData.position + distance);
    }

}
