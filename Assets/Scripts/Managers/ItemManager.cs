using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱클톤 형식으로 모든 ScriptableObject를 가지고있어 아이템에 참조해서 넣어주기 위해 만듬
public class ItemManager : Singleton<ItemManager>
{
    //모든 스크립터블 오브젝트를 가지고 있는 것

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
    /// 코드로 ScriptableObject를 다 받아서 넣어주는 함수
    /// </summary>
    public void ItemInit()
    {
        // 스크립터블 오브젝트 양이적어서 그냥 퍼블릭으로 열어서
        // 인스펙터 창에 넣어줬음 
        // 리소스 로드 안하고


        resources = Resources.LoadAll<ItemScriptableObj>("Prefabs/Item");

        for (int i = 0; i < resources.Length; i++)
        {
            itemList.Add(resources[i]);
        }

        /*        foreach (ItemScriptableObj itemScriptable in itemList)
                {
                    Debug.Log($"리소스 이름 : " + itemScriptable.name);
                    Debug.Log(itemScriptable.itemName);
                }*/
    }
}
