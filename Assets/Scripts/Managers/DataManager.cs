using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using Define;

public class DataManager : Singleton<DataManager>
{

    /// <summary>
    /// Json파일 받아서 넣어줄
    /// Dictionary
    /// </summary>
    Dictionary<int, Define.ItemSaveInfo> invendict = new Dictionary<int, Define.ItemSaveInfo>();

    Dictionary<int, Define.ItemSaveInfo> invenLoadDict = new Dictionary<int, Define.ItemSaveInfo>();


    /// <summary>
    /// inventory itemSlot들
    /// </summary>
    List<ItemSlot> itemSlots;

    /// <summary>
    /// 아이템 총 목록
    /// </summary>
    List<ItemScriptableObj> itemList;

    ItemSaveInfo saveItem = new ItemSaveInfo();

    protected override void Awake()
    {
        base.Awake();
        invenLoadDict = FromJson<Dictionary<int, Define.ItemSaveInfo>>("Assets/Json/UserInvenData.json");

    }

    private void Start()
    {
        //itemSlots = UIManager.Instance.GetUI<InventoryHandler>().itemSlots;
    }

    /// <summary>
    /// json파일로 만드는 함수
    /// </summary>
    /// <param name="path">json저장할 경로</param>
    /// <param name="json">json파일로 바꿀 변수</param>
    /// <param name="isIndented">pretty print?</param>
    public void ToJson(string path, object json, bool isIndented)
    {
        if (isIndented)
        {
            string objToJson = JsonConvert.SerializeObject(json, Formatting.Indented);
            File.WriteAllText(path, objToJson);
        }
        else
        {
            string objToJson = JsonConvert.SerializeObject(json, Formatting.None);
            File.WriteAllText(path, objToJson);
        }
    }



    /// <summary>
    /// json 파일을 가지고와서 T타입으로
    /// 반환하는 함수
    /// </summary>
    /// <typeparam name="T">반환할 타입</typeparam>
    /// <param name="path">가져올 json파일 경로</param>
    /// <returns></returns>
    public T FromJson<T>(string path)
    {
        string formToJson = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(formToJson);
    }

    /// <summary>
    /// 씬전환될때 인벤토리 정보
    /// 저장하고 바로 인벤에 넘겨주는 함수
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void InvenDataPass(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
    {

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            UIManager.Instance.inven?.SlotInitialize();
            InvenSave();
            //InvenLoad();
        }
    }


    /// <summary>
    /// json파일로 저장된 인벤토리 정보를 넣어주는 함수
    /// </summary>
    public void InvenLoad()
    {
        itemList = ItemManager.Instance.itemList;

        itemSlots = UIManager.Instance.GetUI<InventoryHandler>().itemSlots;

        invenLoadDict = FromJson<Dictionary<int, Define.ItemSaveInfo>>("Assets/Json/UserInvenData.json");

        for (int j = 0; j < itemSlots.Count; j++)
        {
            itemSlots[j].itemCount = invenLoadDict[j].Count;
            itemSlots[j].itemInfo = ItemManager.Instance.itemList[(int)invenLoadDict[j].itemInfo];
        }

        UIManager.Instance.GetUI<InventoryHandler>().InvenRefresh();
    }



    /// <summary>
    /// 인벤토리 정보 저장
    /// </summary>
    public void InvenSave()
    {
        invendict.Clear();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            saveItem.itemInfo = itemSlots[i].itemInfo.ScriptableItem;
            saveItem.Count = itemSlots[i].itemCount;
            invendict.Add(i, saveItem);
        }

        ToJson("Assets/Json/UserInvenData.json", invendict, true);
    }

}
