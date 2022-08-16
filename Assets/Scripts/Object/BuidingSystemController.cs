using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Object;
using Project.Inven;
using System;
using System.Linq;

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

    public bool isBuild = false;

    /// <summary>
    /// 하이에라키에 정리용으로 건물 넣어줄거
    /// </summary>
    Transform buildingHolder;

    private CameraController cameraController;

    /// <summary>
    /// 건축물 인벤토리
    /// </summary>
    public BuildingInven inven;

    IngameManager ingame;

    Dictionary<KeyCode, Action> BuildKeyDict = new Dictionary<KeyCode, Action>();

    // 건물에 rigidbody가 없어서 그럼 그냥 해당 범위 체크해서 색변환 및 작업한는게 나을것같다
    // rigidbody가 아닌 overlapbox를 이용
    public void Initialize()
    {
        ingame = IngameManager.Instance;

        buildingHolder ??= new GameObject { name = "BuildingHolder" }.transform;
        buildingHolder.transform.position = new Vector3(0, 0, 0);

        BuildingPoolRigist();

        // selectSlot을 찾는데 null이면 맨 앞에 0번째 아니면 선택된거 찾아서 넣어주기

        BuildingInvenSlot selectSlot = (BuildingInvenSlot)invenSlots.Find(_ => _.isSelect == true) == null
            ? (BuildingInvenSlot)invenSlots[0] : invenSlots.Find(_ => _.isSelect == true) as BuildingInvenSlot;

        cameraController = Camera.main.transform.GetComponent<CameraController>();

        KeyInitialize();

        // sd 정보가 있다면 해당 건물 생성
        if (selectSlot.bo.sdBuildItem.index != 0)
            CreateObject(selectSlot);
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
            Initialize();
        else
            AddPoolObject();

        #region 지역함수

        // 추가로 풀에 넣어야 될때 쓰는 함수
        void AddPoolObject()
        {
            foreach (BuildingInvenSlot slot in invenSlots)
            {

                if (slot.IsHaveItem())
                {
                    // 풀에서 현재 슬롯과 같은 아이템을 가지는 리스트를 만듦
                    var sameItemList = pool.Pool.FindAll(_ => _.boItem.sdBuildItem.index == slot.bo.sdBuildItem.index);

                    // 해당 풀에 같은 슬롯의 아이템의 갯수
                    var poolCount = sameItemList == null ? 0 : sameItemList.Count;

                    // 리소스 매니저를 통해서 가져와야되는 아이템 갯수를 구함 풀에 있는 갯수가 더크면 추가해줄 필요가 없음
                    var count = (int)slot.count != poolCount ? slot.count - poolCount : 0;

                    //// 추가로 등록해줘야 될만큼만 등록을 해준다
                    resourceManager.LoadPoolableObject<BuildItem>(slot.bo.sdBuildItem.resourcePath[1], (int)count);
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
                    resourceManager.LoadPoolableObject<BuildItem>(slot.bo.sdBuildItem.resourcePath[1], (int)slot.Count);
                }
            }
        }
        #endregion
    }

    public void OnUpdate()
    {
        if (currentSelectObject != null && cameraController.cameraState == Define.CameraState.Build)
        {
            BuildingMove();
            if (Input.GetMouseButtonDown(0) && currentSelectObject.GetComponent<BuildItem>().CheckBuildState())
                Build();
        }

        if (Input.anyKeyDown)
        {
            foreach (var dic in BuildKeyDict)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }

    }

    /// <summary>
    /// pool에서 꺼내오는 함수 
    /// </summary>
    /// <param name="sdData"></param>
    public void CreateObject(BuildingInvenSlot slot)
    {
        if (slot.count == 0)
            return;
        // 이전에 있던 건물 지워주고 만듦
        var obj = PoolManager.Instance.GetPool<BuildItem>().GetObject(slot.bo.sdBuildItem);
        // 활성화 비활성화 ??? 말고 알파값을 조정한다?????
        obj.transform.SetParent(buildingHolder);
        obj.SetActive(true);

        currentSelectObject = obj;
        SetAlpha(currentSelectObject, Define.ColorType.Translucent);
        currentSelectObject.transform.position = new Vector3(1, 1, 1);
        PlaneInstantiate(obj.GetComponentInChildren<BoxCollider>(), true);
    }

    public void Build()
    {
        if (inven.currentSelectSlot.Count == 0)
            return;
        //TODO HERE 여기 부분 다시 만들기
        // 인벤토리에서 갯수 차감 -1 해야되고 해당위치에 건물을 지어야함
        // 풀에서 가져올때 하나밖에업성서 못가져옴 풀에서 생성해서 만들어야됨 가져올때 없다면

        // 이미 건축 상태가 될때 cameracontroller에서 초기화를 해주고 있으나 혹시나 안들어와 있을수도 있으므로
        // null이라면 초기화
        inven ??= UIManager.Instance.GetUI<BuildingInven>();


        // 인벤에서 갯수 차감후 더이상 buildingholder에 있지 않고 mapHolder로 부모를 옮겨줌
        inven.currentSelectSlot.Count--;
        Transform mapHolder = SceneManager.Instance.map;
        currentSelectObject.transform.SetParent(mapHolder);

        // 현재 건물에 collCheckPlane이 있으므로 다 가져와서 pool에 다시 넣어준다
        var planes = currentSelectObject.GetComponentsInChildren<ColliderCheckPlane>();

        var pool = PoolManager.Instance.GetPool<ColliderCheckPlane>();
        // 반복돌려서 pool에 다 넣어줌
        for (int i = 0; i < planes.Length; i++)
            pool.PoolReturn(planes[i]);

        // 건물 layer를 처음에 달아주니 건물 자기자신의 layer도 충돌감지를 해서 안되는 경우가 생김
        // 처음에 layer를 가지고있지 않고 건축완료시 layer를 달아줌
        currentSelectObject.layer = LayerMask.NameToLayer("Obj");

        SetAlpha(currentSelectObject, Define.ColorType.Visible);
        currentSelectObject = null;
        Physics.SyncTransforms();
        // 인벤에 템의 갯수가 0이라면 건축x

        CreateObject(inven.currentSelectSlot);
    }



    public void RemoveObject()
    {
        var item = currentSelectObject?.GetComponent<BuildItem>();

        // 현재 선택된 인벤 슬롯이 비어있으면 삭제 시켜줄 필요가 없으므로 함수를 종료 시켜줌
        if (item == null)
            return;

        var poolManager = PoolManager.Instance;

        // 건물 객체 밑에 plane들을 가져옴
        var planes = item.gameObject.GetComponentsInChildren<ColliderCheckPlane>();

        var planePool = poolManager.GetPool<ColliderCheckPlane>();

        // plane객체 들을 전부 다시 pool에 넣어줌
        foreach (var plane in planes)
        {
            planePool.PoolReturn(plane);
        }

        poolManager.GetPool<BuildItem>().PoolReturn(item);

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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
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
    /// 건축 모드일때 카메라 움직여주는 함수
    /// </summary>
    private void BuildCameraMove()
    {
        // 인게임 매니저에서 건축상태인지 판단하는 불타입 변수를 기준으로
        // 건축 상태가 아니라면 이함수는 작동하지 않아야됨
        if (!ingame.isBuilding)
            return;


    }


    public Rect MakeRectRange(Collider coll)
    {
        Rect r = new Rect(0, 0, coll.bounds.size.x, coll.bounds.size.z);
        return r;
    }

    public void PlaneInstantiate(BoxCollider coll, bool able)
    {
        // TODO scale 2보다 작은거에 바닥 플레인 사이즈 맞추기 or 탭키 넘겼을때 다시 풀에 넣어 주고 삭제 시키기 <- 현재 안되고 있음

        // 풀링을 적용 시킬 것이니 미리 풀매니저 가져오기
        var poolManager = PoolManager.Instance;

        // 현재 collider 값을 임의로 수정을 해서 엔진에서 가지고 있는 값과 실제 값과 차이가 생김
        // 좌표값이 맞지 않기 때문에 엔진에 다시 transform에 대한 값을 다시 맞춰줌
        Physics.SyncTransforms();

        // plane 사각 범위를 받기 위해 풀매니저에서 plane 풀을 찾아 객체하나 가져옴
        var plane = poolManager.GetPool<ColliderCheckPlane>().GetObject();
        plane.gameObject.SetActive(true);

        // 사각 plane의 사각 범위를 받아서
        Rect colcheckPlane = MakeRectRange(plane.GetComponent<Collider>());

        var buildingObject = coll.gameObject;

        float endX = colcheckPlane.xMin + colcheckPlane.width;

        float endY = colcheckPlane.yMin + colcheckPlane.height;
        // bounds.center 값이 이상함 collider말고 boxcollider나 meshrender받아서 해보기

        // world space 기준으로 하면 좌표 가 이상해짐 local space를 기준을 계산
        // 어차피 holder를 기준으로 local 좌표를 사용함 
        var extentsX = (coll.bounds.size.x / 2);
        var extentsY = (coll.bounds.size.z / 2);

        // 바닥 플레인 collider 의 값도 더해주고 빼야됨
        var planeXPos = coll.bounds.center.x - extentsX + (endX / 2);
        var planeZPos = coll.bounds.center.z + extentsY - (endY / 2);

        Vector3 planePos = Vector3.zero;

        // Fence가 아닌 barrel house 는 x -90도 만큼 축이 돌아가있어서 y,z 축이 바뀜
        planePos = new Vector3(0, 0.5f, 0);
        Vector3 plaenOriginPos = planePos;

        var planePool = poolManager.GetPool<ColliderCheckPlane>();


        var startX = planePos.x;
        var startY = planePos.z;

        var planeYPos = coll.bounds.min.y + 0.5f;


        if (extentsX < 1)
        {
            AxisZInstantiate(coll.bounds.center.x);
        }
        else if (extentsY < 1)
        {
            AxisXInstantiate(coll.bounds.center.z);
        }
        else
        {
            for (float i = planeXPos; i < coll.bounds.max.x; i += endX)
            {
                for (float j = planeZPos; j > coll.bounds.min.z; j -= endY)
                {
                    PlaneInstantiate(i, j);
                }
            }
        }
        plane.gameObject.SetActive(false);

        // 만든 평면들을 리스트에 담아줌
        buildingObject.transform.parent.GetComponent<BuildItem>().ReigistPlanes();


        // 매개변수로 받은 좌표에 colliderCheckPlane을 생성시켜주는 함수
        void PlaneInstantiate(float x, float z)
        {
            var obj = planePool.GetObject();

            obj.transform.SetParent(buildingObject.transform.parent);
            obj.transform.position = new Vector3(x, planeYPos, z);
            obj.gameObject.SetActive(true);

        }

        // x 축만 colliderCheckPlane을 생성시키는 함수
        void AxisXInstantiate(float z)
        {
            if (coll.bounds.max.x < endX * 2)
                PlaneInstantiate(coll.bounds.center.x, coll.bounds.center.z);
            else
            {
                for (float i = planeXPos; i < coll.bounds.max.x; i += endX)
                {
                    PlaneInstantiate(i, z);
                }
            }
        }

        void AxisZInstantiate(float x)
        {
            if (coll.bounds.max.z < endY * 2)
                PlaneInstantiate(coll.bounds.center.x, coll.bounds.center.z);
            else
            {
                for (float j = planeZPos; j > coll.bounds.min.z; j -= endY)
                {
                    PlaneInstantiate(x, j);
                }
            }
        }
    }

    /// <summary>
    /// keyDict에 키를 등록하는 함수
    /// </summary>
    private void KeyInitialize()
    {
        if (BuildKeyDict.Count != 0)
            return;

        //BuildKeyDict.Add(KeyCode.W,)
        var camera = Camera.main.transform.gameObject;
        /// <summary>
        /// keyDictionary에 등록해주는 함수
        /// </summary>
        /// <param name="keyCode">입력 받을 key값</param>
        /// <param name="action">작동 시켜줄 함수</param>
        void KeyDictRegist(KeyCode keyCode, Action action)
        {
            BuildKeyDict.Add(keyCode, action);
        }


        #region CameraMoveKeyRigist
        KeyDictRegist(KeyCode.W, () =>
        {
            camera.transform.Translate(Vector3.forward, Space.Self);
        });

        KeyDictRegist(KeyCode.A, () =>
        {
            camera.transform.Translate(Vector3.left, Space.Self);
        });

        KeyDictRegist(KeyCode.S, () =>
        {
            camera.transform.Translate(Vector3.back, Space.Self);
        });

        KeyDictRegist(KeyCode.D, () =>
        {
            camera.transform.Translate(Vector3.right, Space.Self);
        });
        #endregion

        KeyDictRegist(KeyCode.Q, () =>
        {
            // Q키를 누를시 현재 buildingHolder에 건물 이있는지 체크후 없다면 생성시켜줌
            if (buildingHolder.childCount != 0)
                return;

            //CreateObject(inv)
            BuildingInvenSlot selectSlot = (BuildingInvenSlot)invenSlots.Find(_ => _.isSelect == true) == null
    ? (BuildingInvenSlot)invenSlots[0] : invenSlots.Find(_ => _.isSelect == true) as BuildingInvenSlot;

            if (selectSlot.bo.sdBuildItem.index != 0)
                CreateObject(selectSlot);


        });

        KeyDictRegist(KeyCode.E, () =>
        {
            // E키를 누를시 현재 마우스 위치에 있는 건물을 회수

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Obj"), QueryTriggerInteraction.Collide))
            {
                var buildingObj = hit.transform.parent.gameObject.GetComponent<BuildItem>();

                var pool = PoolManager.Instance.GetPool<BuildItem>();

                // slots가 slotBase 형태라서 sd데이터를 찾을 수없어서 cast를 해줌
                var slots = inven.slots.Cast<BuildingInvenSlot>();
                var slot = slots.Where(_ => _.bo.sdBuildItem.index == buildingObj.boItem.sdBuildItem.index).SingleOrDefault() == null ?
                slots.Where(_ => _.bo.sdBuildItem.index == 0).FirstOrDefault() : slots.Where(_ => _.bo.sdBuildItem.index == buildingObj.boItem.sdBuildItem.index).SingleOrDefault();

                // 인벤에 해당 아이템과 같은 아이템이 없을경우
                // 없을때는 인벤에 비어있는 슬롯중 가장 앞에 있는 슬롯을 찾아 아이템을 넣어줌
                pool.PoolReturn(buildingObj);
                slot.AddItem(buildingObj.boItem, 1);
            }

        });

    }

}