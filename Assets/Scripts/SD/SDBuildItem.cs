using Project.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SDBuildItem : StaticDataBase
{
    /*    ��������� sddata
    index int
    name string
    resourcePath string[] 0 : iconpath 1: prefabInstance
    cost int[]  0 : costitemindex 1 : costitemcount*/
    // 0 �Ǹ��ϴ� ���� ������ ������
    // 1 �ǸŵǴ� ������ ���� ������ ��ü
    // 2 �����Ҽ��ִ� ��� ������ ������
    [SerializeField]
    public string name;

    [SerializeField]
    public string[] resourcePath;
    [SerializeField]
    public int[] cost;

}
