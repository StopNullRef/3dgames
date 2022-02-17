using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneObject : ObjInfo
{
    void ObjectInit()
    {
        poolType = Define.PoolType.Object;
        dropItem = ItemManager.Instance.itemList[(int)Define.ScriptableItem.Stone];
    }
}
