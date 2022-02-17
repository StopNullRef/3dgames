using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : ObjInfo
{
    private void Start()
    {
        ObjectInit();
    }
    
    void ObjectInit()
    {
        poolType = Define.PoolType.Object;
        dropItem = ItemManager.Instance.itemList[(int)Define.ScriptableItem.Wood];
    }

}
