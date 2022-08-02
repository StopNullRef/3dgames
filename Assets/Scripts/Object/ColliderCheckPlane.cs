using Project.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckPlane : MonoBehaviour, IPoolableObject
{
    /// <summary>
    /// �ش� plane�� meshRenderer�� ���� ����
    /// </summary>
    MeshRenderer meshRenderer;

    /// <summary>
    /// plaen�� �ö��̴�
    /// </summary>
    Collider coll;

    /// <summary>
    /// �ǹ��� ������ �ִ��� üũ�ϴ� boolŸ�� ����
    /// </summary>
    public bool canCurrentBuilding = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();

        // ó������ ������ �ʷϻ��� ���డ���ϰ� ����
        SetPlane(buildObjectColor.green, true);
    }

    public ColliderCheckObject buildObjectColor;

    public bool CanRecycle { get; set; }

    public void OnUpdate()
    {
        // �浹�� �Ͼ �� üũ������ 
        // �浹�� �Ͼ�ٸ� 
        if (CollisionCheck())
            SetPlane(buildObjectColor.red, false);
        else
            SetPlane(buildObjectColor.green, true);
    }


    /// <summary>
    /// �浹�� üũ�ؼ� �۵��ϴ� �Լ�
    /// </summary>
    private bool CollisionCheck()
    {
        // �ǹ� layer�� ó���� �޾��ִ� �ǹ� �ڱ��ڽ��� layer�� �浹������ �ؼ� �ȵǴ� ��찡 ����
        // ó���� layer�� ���������� �ʰ� ����Ϸ�� layer�� �޾���
        Physics.SyncTransforms();
        int layer = (1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Obj")));

        bool result = false;
        var collArray = Physics.OverlapBox(transform.position, coll.bounds.extents,Quaternion.identity, layer);

        // �ڱ��ڽ��� �������� �Ѱ� �̻����� üũ��
        result = collArray.Length > 1 ? true : false;

        return result;
    }

    private void SetPlane(Material mat, bool buildingable)
    {
        meshRenderer.material = mat;
        this.canCurrentBuilding = buildingable;
    }


    public void PoolInit()
    {

    }
}
