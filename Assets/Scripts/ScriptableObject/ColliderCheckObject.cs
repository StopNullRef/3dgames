using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColliderCheckObject", order = 2)]
public class ColliderCheckObject : ScriptableObject
{
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    public Material red;


    /// <summary>
    /// �ʷϻ� ���͸���
    /// </summary>
    [SerializeField]
    public Material green;

    /// <summary>
    /// �ٴ� �ڽ��� mesh
    /// �� �޽��� �̿��Ͽ�
    /// �Ѹ��� ���� ���� ���̸� �̿��Ͽ�
    /// plane�� ����ٰ�
    /// </summary>
    [SerializeField]
    public Mesh colcheckBoxMesh;
}
