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
    /// plaen의 컬라이더
    /// </summary>
    Collider coll;

    /// <summary>
    /// 건물을 지을수 있는지 체크하는 bool타입 변수
    /// </summary>
    public bool canCurrentBuilding = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();

        // 처음에는 무조건 초록색에 건축가능하게 설정
        SetPlane(buildObjectColor.green, true);
    }

    public ColliderCheckObject buildObjectColor;

    public bool CanRecycle { get; set; }

    public void OnUpdate()
    {
        // 충돌이 일어난 지 체크를한후 
        // 충돌이 일어났다면 
        if (CollisionCheck())
            SetPlane(buildObjectColor.red, false);
        else
            SetPlane(buildObjectColor.green, true);
    }


    /// <summary>
    /// 충돌을 체크해서 작동하는 함수
    /// </summary>
    private bool CollisionCheck()
    {
        // 건물 layer를 처음에 달아주니 건물 자기자신의 layer도 충돌감지를 해서 안되는 경우가 생김
        // 처음에 layer를 가지고있지 않고 건축완료시 layer를 달아줌
        Physics.SyncTransforms();
        int layer = (1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Obj")));

        bool result = false;
        var collArray = Physics.OverlapBox(transform.position, coll.bounds.extents,Quaternion.identity, layer);

        // 자기자신이 들어옴으로 한개 이상으로 체크함
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
