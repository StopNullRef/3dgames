using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Object;
using Project.Inven;

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

    Transform buildingHolder;

    public void Initialize()
    {
        buildingHolder ??= new GameObject { name = "BuildingHolder" }.transform;

        BuildingPoolRigist();

        BuildingInvenSlot selectSlot = invenSlots.Find(_ => _.isSelect == true) as BuildingInvenSlot;

        // sd ������ �ִٸ� �ش� �ǹ� ����
        if(selectSlot.sd.index != 0)
        {
            CreateObject(selectSlot);
        }

        // TODO �ǹ� ������ �ߵǳ� ���İ��� 0.5f�� �ȵ�

        //CreateObject((BuildingInvenSlot)invenSlots[0]);
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
        {
            Initialize();
        }
        else
        {
            AddPoolObject();
        }

        #region �����Լ�

        // �߰��� Ǯ�� �־�� �ɶ� ���� �Լ�
        void AddPoolObject()
        {
            foreach (BuildingInvenSlot slot in invenSlots)
            {

                if (slot.IsHaveItem())
                {
                    // Ǯ���� ���� ���԰� ���� �������� ������ ����Ʈ�� ����
                    var sameItemList = pool.Pool.FindAll(_ => _.boItem.sdBuildItem.index == slot.sd.index);

                    // �ش� Ǯ�� ���� ������ �������� ����
                    var poolCount = sameItemList == null ? 0 : sameItemList.Count;

                    // ���ҽ� �Ŵ����� ���ؼ� �����;ߵǴ� ������ ������ ���� Ǯ�� �ִ� ������ ��ũ�� �߰����� �ʿ䰡 ����
                    var count = (int)slot.count != poolCount ? slot.count - poolCount : 0;

                    //// �߰��� �������� �ɸ�ŭ�� ����� ���ش�
                    resourceManager.LoadPoolableObject<BuildItem>(slot.sd.resourcePath[1], (int)count);
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
            // UIManager���� �޾Ƽ� �Ű����� �ְ� �����Բ�
            // CreateObject()
        }

    }

    /// <summary>
    /// pool���� �������� �Լ� 
    /// </summary>
    /// <param name="sdData"></param>
    public void CreateObject(BuildingInvenSlot slot)
    {
        if (currentSelectObject != null || slot.sd.name == null)
        {
            RemoveObject();
        }

        var obj = PoolManager.Instance.GetPool<BuildItem>().GetObject(slot.sd);
        // Ȱ��ȭ ��Ȱ��ȭ ??? ���� ���İ��� �����Ѵ�?????
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
    /// �ǹ� �̵� �Լ�
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
                render[i].material.color = new Color(colors[i].r, colors[i].g, colors[i].b, i);
            }
        }
    }
}