using Project.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class SDBuildItem : StaticDataBase
{
    /*    건축아이템 sddata
    index int
    name string
    resourcePath string[] 0 : iconpath 1: prefabInstance
    cost int[]  0 : costitemindex 1 : costitemcount*/
    [SerializeField]
    public string name;

    [SerializeField]
    public string[] resourcePath;
    [SerializeField]
    public int[] cost;

}
