using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ŭ�� �������� ��� ScriptableObject�� �������־� �����ۿ� �����ؼ� �־��ֱ� ���� ����
public class ItemManager : Singleton<ItemManager>
{
    //��� ��ũ���ͺ� ������Ʈ�� ������ �ִ� ��

    public List<ItemScriptableObj> itemList = new List<ItemScriptableObj>();

    ItemScriptableObj[] resources;

    protected override void Awake()
    {
        base.Awake();

        if (itemList.Count == 0)
        {
            ItemInit();
        }
    }

    /// <summary>
    /// �ڵ�� ScriptableObject�� �� �޾Ƽ� �־��ִ� �Լ�
    /// </summary>
    public void ItemInit()
    {
        // ��ũ���ͺ� ������Ʈ ������� �׳� �ۺ����� ���
        // �ν����� â�� �־����� 
        // ���ҽ� �ε� ���ϰ�


        resources = Resources.LoadAll<ItemScriptableObj>("Prefabs/Item");

        for (int i = 0; i < resources.Length; i++)
        {
            itemList.Add(resources[i]);
        }

        /*        foreach (ItemScriptableObj itemScriptable in itemList)
                {
                    Debug.Log($"���ҽ� �̸� : " + itemScriptable.name);
                    Debug.Log(itemScriptable.itemName);
                }*/
    }
}
