using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInvenButton : MonoBehaviour
{
    /// <summary>
    /// ���� �ý��ۿ� ��� �ϴ� �κ��丮
    /// ���� �ݴ� ��ư
    /// </summary>
    Button buildingInvenButton;

    /// <summary>
    /// �κ��丮 ��ư�� �ִ� ȭ��ǥ
    /// </summary>
    public Transform arrow;

    /// <summary>
    /// ���� �κ��丮 ��׶��� RectTransform
    /// </summary>
    public RectTransform invenBackGround;

    /// <summary>
    /// ���� �κ��丮�� �۾�������??
    /// �����ִ���
    /// </summary>
    bool isOpening = false;

    /// <summary>
    /// ȭ��ǥ �̹��� �� �������
    /// ���ִ� ȸ����
    /// </summary>
    Quaternion arrowRot = Quaternion.Euler(180, 0, 0);

    /// <summary>
    /// ���� �κ��丮 Ȧ��
    /// </summary>
    Transform invenHolder;

    public void Start()
    {
        Initialize();
    }

    /// <summary>
    /// �ʱ�ȭ ���ִ� �Լ�
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
    /// Ŭ�� �̺�Ʈ�� �޾�����
    /// �κ��丮�� �������� �Լ�
    /// </summary>
    /// <param name="isOpening">�κ��� Ȱ��ȭ�Ǿ��մ��� �ƴ��� üũ�ϴ� ��Ÿ�Ժ���</param>
    void InvenMove()
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

}