using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInfo : MonoBehaviour
{
    //ItemBase dropItem;
    protected ItemScriptableObj dropItem;
    protected int dropCount;
    public Define.PoolType poolType = Define.PoolType.None;

    public ItemScriptableObj ItemDrop
    {
        get => this.dropItem;
    }

    public int DropCount
    {
        get
        {
            dropCount = Random.Range(800, Define.MaxCount.ObjectMaxDrop);
            return dropCount;
        }
    }

    


}
