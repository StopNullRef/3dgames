using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckPlane : MonoBehaviour
{
    /// <summary>
    /// �ش� plane�� meshRenderer�� ���� ����
    /// </summary>
    MeshRenderer meshRenderer;

    /// <summary>
    /// �ǹ��� ������ �ִ��� üũ�ϴ� boolŸ�� ����
    /// </summary>
    public bool canCurrentBuilding = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // ó������ ������ �ʷϻ��� ���డ���ϰ� ����
        SetPlane(buildObjectColor.green, true);
    }

    public ColliderCheckObject buildObjectColor;

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� �Ͼ�� �ǹ��� ������ �����Ƿ�
        // ���������� �ٲ��ְ� buildingable�� false�� ����
        SetPlane(buildObjectColor.red, false);
    }

    private void OnTriggerExit(Collider other)
    {
        // �浹�� �Ͼ�ٰ� ��� �Ǿ�����
        // �ʷϻ����� �ٲ��ְ� buildingable�� true�� ����
        SetPlane(buildObjectColor.green, true);
    }

    /// <summary>
    /// colliderCheckPlane�� �������ִ� �Լ�
    /// </summary>
    /// <param name="mat">�ٲ��� Material</param>
    /// <param name="buildingable">������ �����ϰ� �Ұ���?</param>
    private void SetPlane(Material mat, bool buildingable)
    {
        meshRenderer.material = mat;
        this.canCurrentBuilding = buildingable;
    }

}
