using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColliderCheckObject", order = 2)]
public class ColliderCheckObject : ScriptableObject
{
    /// <summary>
    /// 빨간색
    /// </summary>
    [SerializeField]
    public Material red;


    /// <summary>
    /// 초록색 매터리얼
    /// </summary>
    [SerializeField]
    public Material green;

    /// <summary>
    /// 바닥 박스의 mesh
    /// 이 메쉬를 이용하여
    /// 한면의 가로 세로 길이를 이용하여
    /// plane을 깔아줄것
    /// </summary>
    [SerializeField]
    public Mesh colcheckBoxMesh;
}
