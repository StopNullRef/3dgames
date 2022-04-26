using Project.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckPlane : MonoBehaviour, IPoolableObject
{
    /// <summary>
    /// 해당 plane의 meshRenderer를 받을 변수
    /// </summary>
    MeshRenderer meshRenderer;

    /// <summary>
    /// 건물을 지을수 있는지 체크하는 bool타입 변수
    /// </summary>
    public bool canCurrentBuilding = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // 처음에는 무조건 초록색에 건축가능하게 설정
        SetPlane(buildObjectColor.green, true);
    }

    public ColliderCheckObject buildObjectColor;

    public bool CanRecycle { get ; set; }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌이 일어나면 건물을 지을수 없으므로
        // 빨간색으로 바꿔주고 buildingable을 false로 설정
        SetPlane(buildObjectColor.red, false);
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌이 일어났다가 취소 되었을때
        // 초록색으로 바꿔주고 buildingable을 true로 설정
        SetPlane(buildObjectColor.green, true);
    }

    /// <summary>
    /// colliderCheckPlane을 설정해주는 함수
    /// </summary>
    /// <param name="mat">바꿔줄 Material</param>
    /// <param name="buildingable">건축이 가능하게 할건지?</param>
    private void SetPlane(Material mat, bool buildingable)
    {
        meshRenderer.material = mat;
        this.canCurrentBuilding = buildingable;
    }

    public void PoolInit()
    {
        throw new System.NotImplementedException();
    }
}
