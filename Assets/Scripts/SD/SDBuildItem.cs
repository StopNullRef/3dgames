using Project.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDBuildItem : StaticDataBase
{
    /*    ��������� sddata
    index int
    name string
    resourcePath string[] 0 : iconpath 1: prefabInstance
    cost int[]  0 : costitemindex 1 : costitemcount*/
    [SerializeField]
    string name;

    [SerializeField]
    string[] resourcePath;
    [SerializeField]
    int[] cost;

}
