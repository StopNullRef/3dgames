using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBase : MonoBehaviour
{
    /// <summary>
    /// �����ۿ� ���� ������ ���� ScriptableObject
    /// </summary>
    public ItemInfoBase itemInfo;

    /// <summary>
    /// ������ ������ �̹���
    /// </summary>
    public Image itemIconImage;

    /// <summary>
    /// ������ ������ ��Ÿ���� text
    /// </summary>
    public Text itemCountText;

    /// <summary>
    /// �ڱⰡ ���õǾ����� üũ�ϴ� ��Ÿ�� ����
    /// </summary>
    public bool isSelect = false;

    /// <summary>
    /// �ش� ���� �������� ���̴� ����
    /// </summary>
    protected Color visible = new Color(255, 255, 255, 255);

    /// <summary>
    /// �ش� ���� �������� �Ⱥ��̴� ����
    /// </summary>
    protected Color invisible = new Color(255, 255, 255, 0);

    /// <summary>
    /// ������ ����
    /// </summary>
    public float count;

    /// <summary>
    /// �����̹����� �������ִ� �Լ�
    /// </summary>
    public void SetSlotImage()
    {
        // ������ ������ ���ٸ� �������� ���ٴ� ���̹Ƿ� 
        // �������� �Ⱥ��̰Բ� ������ش�
        if (itemInfo == null)
            itemIconImage.color = invisible;
        // ������ ������ �������� ���̰� ����
        // ������ ������ �̹����� ���̰� �ϱ�
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
