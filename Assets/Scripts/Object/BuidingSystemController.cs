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
    /// ���� ���� �κ��丮 �� ���õ� ������Ʈ
    /// </summary>
    public GameObject currentSelectObject;

    /// <summary>
    /// ���� �κ��� ���Ը���Ʈ
    /// </summary>
    private List<SlotBase> invenSlots = new List<SlotBase>();

    public bool isBuild = false;

    /// <summary>
    /// ���̿���Ű�� ���������� �ǹ� �־��ٰ�
    /// </summary>
    Transform buildingHolder;

    private CameraController cameraController;

    /// <summary>
    /// ���๰ �κ��丮
    /// </summary>
    public BuildingInven inven;

    IngameManager ingame;

    Dictionary<KeyCode, Action> BuildKeyDict = new Dictionary<KeyCode, Action>();

    // �ǹ��� rigidbody�� ��� �׷� �׳� �ش� ���� üũ�ؼ� ����ȯ �� �۾��Ѵ°� �����Ͱ���
    // rigidbody�� �ƴ� overlapbox�� �̿�
    public void Initialize()
    {
        ingame = IngameManager.Instance;

        buildingHolder ??= new GameObject { name = "BuildingHolder" }.transform;
        buildingHolder.transform.position = new Vector3(0, 0, 0);

        BuildingPoolRigist();

        // selectSlot�� ã�µ� null�̸� �� �տ� 0��° �ƴϸ� ���õȰ� ã�Ƽ� �־��ֱ�

        BuildingInvenSlot selectSlot = (BuildingInvenSlot)invenSlots.Find(_ => _.isSelect == true) == null
            ? (BuildingInvenSlot)invenSlots[0] : invenSlots.Find(_ => _.isSelect == true) as BuildingInvenSlot;

        cameraController = Camera.main.transform.GetComponent<CameraController>();

        KeyInitialize();

        // sd ������ �ִٸ� �ش� �ǹ� ����
        if (selectSlot.bo.sdBuildItem.index != 0)
            CreateObject(selectSlot);
    }

    /// <summary>
    /// �κ��丮�� ������ �ִ� �ǹ� ������ ������ŭ
    /// Ǯ�� ������ִ� �Լ�
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

        #region �����Լ�

        // �߰��� Ǯ�� �־�� �ɶ� ���� �Լ�
        void AddPoolObject()
        {
            foreach (BuildingInvenSlot slot in invenSlots)
            {

                if (slot.IsHaveItem())
                {
                    // Ǯ���� ���� ���԰� ���� �������� ������ ����Ʈ�� ����
                    var sameItemList = pool.Pool.FindAll(_ => _.boItem.sdBuildItem.index == slot.bo.sdBuildItem.index);

                    // �ش� Ǯ�� ���� ������ �������� ����
                    var poolCount = sameItemList == null ? 0 : sameItemList.Count;

                    // ���ҽ� �Ŵ����� ���ؼ� �����;ߵǴ� ������ ������ ���� Ǯ�� �ִ� ������ ��ũ�� �߰����� �ʿ䰡 ����
                    var count = (int)slot.count != poolCount ? slot.count - poolCount : 0;

                    //// �߰��� �������� �ɸ�ŭ�� ����� ���ش�
                    resourceManager.LoadPoolableObject<BuildItem>(slot.bo.sdBuildItem.resourcePath[1], (int)count);
                }
            }
        }

        // ó���� Ǯ�� ������ִ� �Լ�
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
    /// pool���� �������� �Լ� 
    /// </summary>
    /// <param name="sdData"></param>
    public void CreateObject(BuildingInvenSlot slot)
    {
        if (slot.count == 0)
            return;
        // ������ �ִ� �ǹ� �����ְ� ����
        var obj = PoolManager.Instance.GetPool<BuildItem>().GetObject(slot.bo.sdBuildItem);
        // Ȱ��ȭ ��Ȱ��ȭ ??? ���� ���İ��� �����Ѵ�?????
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
        //TODO HERE ���� �κ� �ٽ� �����
        // �κ��丮���� ���� ���� -1 �ؾߵǰ� �ش���ġ�� �ǹ��� �������
        // Ǯ���� �����ö� �ϳ��ۿ������� �������� Ǯ���� �����ؼ� �����ߵ� �����ö� ���ٸ�

        // �̹� ���� ���°� �ɶ� cameracontroller���� �ʱ�ȭ�� ���ְ� ������ Ȥ�ó� �ȵ��� �������� �����Ƿ�
        // null�̶�� �ʱ�ȭ
        inven ??= UIManager.Instance.GetUI<BuildingInven>();


        // �κ����� ���� ������ ���̻� buildingholder�� ���� �ʰ� mapHolder�� �θ� �Ű���
        inven.currentSelectSlot.Count--;
        Transform mapHolder = SceneManager.Instance.map;
        currentSelectObject.transform.SetParent(mapHolder);

        // ���� �ǹ��� collCheckPlane�� �����Ƿ� �� �����ͼ� pool�� �ٽ� �־��ش�
        var planes = currentSelectObject.GetComponentsInChildren<ColliderCheckPlane>();

        var pool = PoolManager.Instance.GetPool<ColliderCheckPlane>();
        // �ݺ������� pool�� �� �־���
        for (int i = 0; i < planes.Length; i++)
            pool.PoolReturn(planes[i]);

        // �ǹ� layer�� ó���� �޾��ִ� �ǹ� �ڱ��ڽ��� layer�� �浹������ �ؼ� �ȵǴ� ��찡 ����
        // ó���� layer�� ���������� �ʰ� ����Ϸ�� layer�� �޾���
        currentSelectObject.layer = LayerMask.NameToLayer("Obj");

        SetAlpha(currentSelectObject, Define.ColorType.Visible);
        currentSelectObject = null;
        Physics.SyncTransforms();
        // �κ��� ���� ������ 0�̶�� ����x

        CreateObject(inven.currentSelectSlot);
    }



    public void RemoveObject()
    {
        var item = currentSelectObject?.GetComponent<BuildItem>();

        // ���� ���õ� �κ� ������ ��������� ���� ������ �ʿ䰡 �����Ƿ� �Լ��� ���� ������
        if (item == null)
            return;

        var poolManager = PoolManager.Instance;

        // �ǹ� ��ü �ؿ� plane���� ������
        var planes = item.gameObject.GetComponentsInChildren<ColliderCheckPlane>();

        var planePool = poolManager.GetPool<ColliderCheckPlane>();

        // plane��ü ���� ���� �ٽ� pool�� �־���
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
    /// �ǹ� �̵� �Լ�
    /// </summary>
    private void BuildingMove()
    {
        // ����� �ȵ���� 
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
        {
            currentSelectObject.transform.position = hit.point;
        }
    }

    /// <summary>
    /// �ǹ� ������Ʈ�� ���İ��� �������ִ� �Լ�
    /// </summary>
    /// <param name="building">���� �ٲ� �ǹ�</param>
    /// <param name="colorType">� �������� �ٲܰ��� �־��ִ� ������
    /// </param>
    public void SetAlpha(GameObject building, Define.ColorType colorType)
    {
        // ���� fence ��ü�� �ڽ��� material�� ���� ��� �����Ƿ� �迭�� �޾� ��������ش�
        var render = building.GetComponentsInChildren<Renderer>();
        // render�� ���� ���� ��ȸ�� ����Ʈ
        List<Color> colors = new List<Color>();

        foreach (var color in render)
        {
            colors.Add(color.material.color);
        }
        // fence�� �ϳ��� �ٲ�
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


        // ������Ʈ ������ ���İ��� �������ִ� �����Լ�
        void SetRenderAlpha(float alpha)
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.color = new Color(colors[i].r, colors[i].g, colors[i].b, alpha);
            }
        }
    }

    /// <summary>
    /// ���� ����϶� ī�޶� �������ִ� �Լ�
    /// </summary>
    private void BuildCameraMove()
    {
        // �ΰ��� �Ŵ������� ����������� �Ǵ��ϴ� ��Ÿ�� ������ ��������
        // ���� ���°� �ƴ϶�� ���Լ��� �۵����� �ʾƾߵ�
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
        // TODO scale 2���� �����ſ� �ٴ� �÷��� ������ ���߱� or ��Ű �Ѱ����� �ٽ� Ǯ�� �־� �ְ� ���� ��Ű�� <- ���� �ȵǰ� ����

        // Ǯ���� ���� ��ų ���̴� �̸� Ǯ�Ŵ��� ��������
        var poolManager = PoolManager.Instance;

        // ���� collider ���� ���Ƿ� ������ �ؼ� �������� ������ �ִ� ���� ���� ���� ���̰� ����
        // ��ǥ���� ���� �ʱ� ������ ������ �ٽ� transform�� ���� ���� �ٽ� ������
        Physics.SyncTransforms();

        // plane �簢 ������ �ޱ� ���� Ǯ�Ŵ������� plane Ǯ�� ã�� ��ü�ϳ� ������
        var plane = poolManager.GetPool<ColliderCheckPlane>().GetObject();
        plane.gameObject.SetActive(true);

        // �簢 plane�� �簢 ������ �޾Ƽ�
        Rect colcheckPlane = MakeRectRange(plane.GetComponent<Collider>());

        var buildingObject = coll.gameObject;

        float endX = colcheckPlane.xMin + colcheckPlane.width;

        float endY = colcheckPlane.yMin + colcheckPlane.height;
        // bounds.center ���� �̻��� collider���� boxcollider�� meshrender�޾Ƽ� �غ���

        // world space �������� �ϸ� ��ǥ �� �̻����� local space�� ������ ���
        // ������ holder�� �������� local ��ǥ�� ����� 
        var extentsX = (coll.bounds.size.x / 2);
        var extentsY = (coll.bounds.size.z / 2);

        // �ٴ� �÷��� collider �� ���� �����ְ� ���ߵ�
        var planeXPos = coll.bounds.center.x - extentsX + (endX / 2);
        var planeZPos = coll.bounds.center.z + extentsY - (endY / 2);

        Vector3 planePos = Vector3.zero;

        // Fence�� �ƴ� barrel house �� x -90�� ��ŭ ���� ���ư��־ y,z ���� �ٲ�
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

        // ���� ������ ����Ʈ�� �����
        buildingObject.transform.parent.GetComponent<BuildItem>().ReigistPlanes();


        // �Ű������� ���� ��ǥ�� colliderCheckPlane�� ���������ִ� �Լ�
        void PlaneInstantiate(float x, float z)
        {
            var obj = planePool.GetObject();

            obj.transform.SetParent(buildingObject.transform.parent);
            obj.transform.position = new Vector3(x, planeYPos, z);
            obj.gameObject.SetActive(true);

        }

        // x �ุ colliderCheckPlane�� ������Ű�� �Լ�
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
    /// keyDict�� Ű�� ����ϴ� �Լ�
    /// </summary>
    private void KeyInitialize()
    {
        if (BuildKeyDict.Count != 0)
            return;

        //BuildKeyDict.Add(KeyCode.W,)
        var camera = Camera.main.transform.gameObject;
        /// <summary>
        /// keyDictionary�� ������ִ� �Լ�
        /// </summary>
        /// <param name="keyCode">�Է� ���� key��</param>
        /// <param name="action">�۵� ������ �Լ�</param>
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
            // QŰ�� ������ ���� buildingHolder�� �ǹ� ���ִ��� üũ�� ���ٸ� ����������
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
            // EŰ�� ������ ���� ���콺 ��ġ�� �ִ� �ǹ��� ȸ��

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << LayerMask.NameToLayer("Obj"), QueryTriggerInteraction.Collide))
            {
                var buildingObj = hit.transform.parent.gameObject.GetComponent<BuildItem>();

                var pool = PoolManager.Instance.GetPool<BuildItem>();

                // slots�� slotBase ���¶� sd�����͸� ã�� ����� cast�� ����
                var slots = inven.slots.Cast<BuildingInvenSlot>();
                var slot = slots.Where(_ => _.bo.sdBuildItem.index == buildingObj.boItem.sdBuildItem.index).SingleOrDefault() == null ?
                slots.Where(_ => _.bo.sdBuildItem.index == 0).FirstOrDefault() : slots.Where(_ => _.bo.sdBuildItem.index == buildingObj.boItem.sdBuildItem.index).SingleOrDefault();

                // �κ��� �ش� �����۰� ���� �������� �������
                // �������� �κ��� ����ִ� ������ ���� �տ� �ִ� ������ ã�� �������� �־���
                pool.PoolReturn(buildingObj);
                slot.AddItem(buildingObj.boItem, 1);
            }

        });

    }

}