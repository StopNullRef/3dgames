using Project.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SDBuildItem : StaticDataBase
{
    /*    건축아이템 sddata
    index int
    name string
    resourcePath string[] 0 : iconpath 1: prefabInstance
    cost int[]  0 : costitemindex 1 : costitemcount*/
    // 0 판매하는 실제 아이템 아이콘
    // 1 판매되는 아이템 실제 프리팹 객체
    // 2 구매할수있는 재료 아이템 아이콘
    [SerializeField]
    public string name;

    [SerializeField]
    public string[] resourcePath;
    [SerializeField]
    public int[] cost;

}
