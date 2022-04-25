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
        if(selectSlot.sd.index != 0)
        {
            CreateObject(selectSlot);
        }

        // TODO 건물 생성은 잘되나 알파값이 0.5f가 안들어감

        //CreateObject((BuildingInvenSlot)invenSlots[0]);
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
        if (currentSelectObject == null)
        {
            // UIManager에서 받아서 매개변수 넣고 돌리게끔
            // CreateObject()
        }

    }

    /// <summary>
    /// pool에서 꺼내오는 함수 
    /// </summary>
    /// <param name="sdData"></param>
    public void CreateObject(BuildingInvenSlot slot)
    {
        if (currentSelectObject != null || slot.sd.name == null)
        {
            RemoveObject();
        }

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
        PoolManager.Instance.GetPool<BuildItem>().PoolReturn(currentSelectObject.GetComponent<BuildItem>());
        currentSelectObject = null;
    }

    public void Build(GameObject go)
    {

    }

    /// <summary>
    /// 건물 이동 함수
    /// </summary>
    private void BuildingMove()
    {
        if (currentSelectObject != null)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
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
            colors.Add(color.material.color);
        }

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
                render[i].material.color = new Color(colors[i].r, colors[i].g, colors[i].b, i);
            }
        }
    }
}