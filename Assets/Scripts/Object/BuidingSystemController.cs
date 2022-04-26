using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Object;
using Project.Inven;

public class BuidingSystemController : MonoBehaviour
{
    /// <summary>
    /// 현재 건축 인벤토리 상에 선택된 오브젝트
    /// </summary>
    public GameObject currentSelectObject;

    /// <summary>
    /// 건축 인벤의 슬롯리스트
    /// </summary>
    private List<SlotBase> invenSlots = new List<SlotBase>();

    Transform buildingHolder;

    public void Initialize()
    {
        buildingHolder ??= new GameObject { name = "BuildingHolder" }.transform;

        BuildingPoolRigist();

        BuildingInvenSlot selectSlot = invenSlots.Find(_ => _.isSelect == true) as BuildingInvenSlot;

        // sd 정보가 있다면 해당 건물 생성
        if (selectSlot.sd.index != 0)
        {
            CreateObject(selectSlot);
        }
        //TODO 04/26 현재 건물까지 잘만들어짐 그 이후에 건물 밑에 collcheck타일 생성후 적용 시키기
    }

    /// <summary>
    /// 인벤토리에 가지고 있는 건물 아이템 갯수만큼
    /// 풀에 등록해주는 함수
    /// </summary>
    public void BuildingPoolRigist()
    {
        invenSlots = UIManager.Instance.GetUI<BuildingInven>().slots;
        var pool = PoolManager.Instance.GetPool<BuildItem>();
        var resourceManager = ResourceManager.Instance;


        if (pool == null)
        {
            Initialize();
        }
        else
        {
            AddPoolObject();
        }

        #region 지역함수

        // 추가로 풀에 넣어야 될때 쓰는 함수
        void AddPoolObject()
        {
            foreach (BuildingInvenSlot slot in invenSlots)
            {

                if (slot.IsHaveItem())
                {
                    // 풀에서 현재 슬롯과 같은 아이템을 가지는 리스트를 만듦
                    var sameItemList = pool.Pool.FindAll(_ => _.boItem.sdBuildItem.index == slot.sd.index);

                    // 해당 풀에 같은 슬롯의 아이템의 갯수
                    var poolCount = sameItemList == null ? 0 : sameItemList.Count;

                    // 리소스 매니저를 통해서 가져와야되는 아이템 갯수를 구함 풀에 있는 갯수가 더크면 추가해줄 필요가 없음
                    var count = (int)slot.count != poolCount ? slot.count - poolCount : 0;

                    //// 추가로 등록해줘야 될만큼만 등록을 해준다
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], (int)count);
                }
            }
        }

        // 처음에 풀에 등록해주는 함수
        void Initialize()
        {
            var resourceManager = ResourceManager.Instance;
            foreach (BuildingInvenSlot slot in invenSlots)
            {
                if (slot.IsHaveItem())
                {
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], (int)slot.Count);
                }
            }
        }
        #endregion
    }

    public void OnUpdate()
    {

        if (currentSelectObject != null)
        {
            BuildingMove();
        }

    }

    /// <summary>
    /// pool에서 꺼내오는 함수 
    /// </summary>
    /// <param name="sdData"></param>
    public void CreateObject(BuildingInvenSlot slot)
    {
        // 이전에 있던 건물 지워주고 만듦
        var obj = PoolManager.Instance.GetPool<BuildItem>().GetObject(slot.sd);
        // 활성화 비활성화 ??? 말고 알파값을 조정한다?????
        obj.transform.SetParent(buildingHolder);
        obj.SetActive(true);
        currentSelectObject = obj;
        SetAlpha(currentSelectObject, Define.ColorType.Translucent);
        currentSelectObject.transform.position = new Vector3(1, 1, 1);
    }

    public void RemoveObject()
    {
        var item = currentSelectObject?.GetComponent<BuildItem>();

        if (item != null)
            PoolManager.Instance.GetPool<BuildItem>().PoolReturn(item);

        //currentSelectObject = null;
    }

    public void Build(GameObject go)
    {

    }

    /// <summary>
    /// 건물 이동 함수
    /// </summary>
    private void BuildingMove()
    {
        // 마우승 안따라옴 
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f,1 << LayerMask.NameToLayer("Ground")))
        {
            currentSelectObject.transform.position = hit.point;
        }
    }

    /// <summary>
    /// 건물 오브젝트의 알파값을 조절해주는 함수
    /// </summary>
    /// <param name="building">색을 바꿀 건물</param>
    /// <param name="colorType">어떤 형식으로 바꿀건지 넣어주는 열거형
    /// </param>
    public void SetAlpha(GameObject building, Define.ColorType colorType)
    {
        // 현재 fence 객체가 자식이 material을 각각 들고 있으므로 배열로 받아 적용시켜준다
        var render = building.GetComponentsInChildren<Renderer>();
        // render의 색을 담을 순회용 리스트
        List<Color> colors = new List<Color>();

        foreach (var color in render)
        {
            Debug.Log(color.gameObject.name);
            colors.Add(color.material.color);
        }
        // fence는 하나만 바뀜
        switch (colorType)
        {
            case Define.ColorType.Invisible:
                SetRenderAlpha(0);
                break;
            case Define.ColorType.Translucent:
                SetRenderAlpha(0.5f);
                break;
            case Define.ColorType.Visible:
                SetRenderAlpha(1);
                break;
        }


        // 오브젝트 렌더에 알파값을 조정해주는 지역함수
        void SetRenderAlpha(float alpha)
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.color = new Color(colors[i].r, colors[i].g, colors[i].b, alpha);
            }
        }
    }

    /// <summary>
    /// Grid Building System을 위해 collider체크를 하기 위한
    /// Rect 범위를 반환해주는 함수
    /// </summary>
    /// <param name="meshRenderer">한면의 rect 범위를 반환할 meshRenderer</param>
    /// <returns>meshRenderer를 이용해 한면의 사각범위를 반환</returns>
    public  Rect MakeRectRange(MeshRenderer meshRenderer)
    {
        Rect r = new Rect(0, 0, meshRenderer.bounds.size.x, meshRenderer.bounds.size.y);
        return r;
    }

    /// <summary>
    /// ColliderCheckPlane을 생성시켜주는 함수
    /// </summary>
    /// <param name="buildingObject">지을 건물</param>
    /// <param name="colliderCheckPlane">체크할  plane 바닥</param>
    /// <param name="buildingPos">건물을 지을 위치</param>
    public  void PlaneInstantiate(GameObject buildingObject, GameObject colliderCheckPlane, Vector3 buildingPos)
    {
        // 사각 plane의 사각 범위를 받아서
        Rect colcheckRect = MakeRectRange(colliderCheckPlane.GetComponent<MeshRenderer>());

        //스케일 맞춰주고
        colliderCheckPlane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float startX = colcheckRect.xMin;
        float endX = colcheckRect.xMin + colcheckRect.width;

        float startY = colcheckRect.yMin;
        float endY = colcheckRect.yMin + colcheckRect.height;

        for (float i = startX; i < colcheckRect.width; i += endX)
        {
            for (float j = startY; j < colcheckRect.height; j += endY)
            {
                //TODO 04/26 인스탄티에이트 말구 pool에서 가져오게끔 만들기
                Vector3 planePos = new Vector3(buildingObject.transform.position.x + i, buildingObject.GetComponent<MeshRenderer>().bounds.min.y + 0.01f, buildingObject.transform.position.z + j);
                MonoBehaviour.Instantiate(colliderCheckPlane, planePos, Quaternion.identity, buildingObject.transform);
            }
        }
    }
}