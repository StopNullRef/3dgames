using Project.SD;
using UnityEngine;

[SerializeField]
public class SDStore : StaticDataBase
{
    public string npcName;
    public int[] saleItem;
    public float[] stagePos;
    public string resourcePath;
    public Define.Scene sceneRef;
}
