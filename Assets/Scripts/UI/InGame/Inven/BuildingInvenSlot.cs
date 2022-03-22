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
    public GameObject selectImage;

    [SerializeField]
    public SDBuildItem sd;


    /// <summary>
    /// SelectImage�� �ش� slot�� ��Ÿ�Ժ����� �̿���
    /// Ȱ��ȭ ��Ȱ��ȭ���ִ� �Լ�
    /// </summary>
    public void SelectImageOnOff()
    {
        selectImage.SetActive(isSelect);
    }

    public void AddItem(SDBuildItem sd, int count)
    {
        this.sd = sd;
        this.count += count;
        SlotRefresh();
    }

    public void SlotRefresh()
    {
        itemIconImage.sprite = Resources.Load<Sprite>(sd.resourcePath[0]);
        itemCountText.text = count.ToString();
    }

}
