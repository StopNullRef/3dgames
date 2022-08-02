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

    /// <summary>
    /// sd�����͸� ���Կ� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="sd">�߰��� StaticData</param>
    /// <param name="count">�߰��� �������� ����</param>
    public void AddItem(SDBuildItem sd, int count)
    {
        this.sd = sd;
        this.count += count;
        SlotRefresh();
    }

    /// <summary>
    /// ������ ���Կ� �ִ� ������ ���� �̹����� 
    /// ������ ���� text�� UI�� �������ִ� �Լ� 
    /// </summary>
    public void SlotRefresh()
    {
        if (sd == null || sd.index == 0)
        {
            // ���⿡ ���Դٴ°��� �ش� ���Կ� �������� ����ִٴ°�
            // �ش� ���Կ� ����ִ� ���� �̹����� �־���
            itemIconImage.sprite = IngameManager.Instance.buildingInvenSlot;
            itemCountText.text = count.ToString();
        }
        else
        {
            // ���⿡ ���Դٴ� ���� �ص� ���Կ� �������� ���� �Ѵٴ� ��
            // �ش� ������ ������ �°� �����̹����� �־���
            itemIconImage.sprite = Resources.Load<Sprite>(sd.resourcePath[0]);
            itemCountText.text = count.ToString();
        }
    }

    /// <summary>
    /// ���Կ� ���� ������ ����ִ� �Լ�
    /// </summary>
    private void SlotClear()
    {
        // ���Կ� count�� 0�϶� �������� �������� �����Ƿ� �ش� ���Կ� ���� SDData�� ����ִ� �Լ�

        

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
                //���� ������ ���� 0���϶� ���� ����ִ°� �ٽ� �����ϱ�
                //this.sd.index = 0;
                SlotRefresh();
            }
        }
    }

}
