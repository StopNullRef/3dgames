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
    /// Json���� �޾Ƽ� �־���
    /// Dictionary
    /// </summary>
    Dictionary<int, Define.ItemSaveInfo> invendict = new Dictionary<int, Define.ItemSaveInfo>();

    Dictionary<int, Define.ItemSaveInfo> invenLoadDict = new Dictionary<int, Define.ItemSaveInfo>();


    /// <summary>
    /// inventory itemSlot��
    /// </summary>
    List<ItemSlot> itemSlots;

    /// <summary>
    /// ������ �� ���
    /// </summary>
    List<ItemScriptableObj> itemList;

    ItemSaveInfo saveItem = new ItemSaveInfo();

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        itemSlots = UIManager.Instance.inven.itemSlots;
        invenLoadDict = FromJson<Dictionary<int, Define.ItemSaveInfo>>("Assets/Json/UserInvenData.json");
    }

    /// <summary>
    /// json���Ϸ� ����� �Լ�
    /// </summary>
    /// <param name="path">json������ ���</param>
    /// <param name="json">json���Ϸ� �ٲ� ����</param>
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
    /// json ������ ������ͼ� TŸ������
    /// ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T">��ȯ�� Ÿ��</typeparam>
    /// <param name="path">������ json���� ���</param>
    /// <returns></returns>
    public T FromJson<T>(string path)
    {
        string formToJson = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(formToJson);
    }

    /// <summary>
    /// ����ȯ�ɶ� �κ��丮 ����
    /// �����ϰ� �ٷ� �κ��� �Ѱ��ִ� �Լ�
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void InvenDataPass(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        //TODO 0224 �κ��� �ʱ�ȭ �ɶ� null�̴� �κ� �ʱ�ȭ�ϴ°��� getui�� �ٲٱ�

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            UIManager.Instance.inven.SlotInitialize();
            InvenSave();
            InvenLoad();
        }
    }


    /// <summary>
    /// json���Ϸ� ����� �κ��丮 ������ �־��ִ� �Լ�
    /// </summary>
    public void InvenLoad()
    {

        itemList = ItemManager.Instance.itemList;

        //itemSlots = UIManager.Instance.inven.itemSlots;

            UIManager.Instance.inven.SlotInitialize();
            itemSlots = UIManager.Instance.inven.itemSlots;
        

        invenLoadDict = FromJson<Dictionary<int, Define.ItemSaveInfo>>("Assets/Json/UserInvenData.json");
        for (int i = 0; i < Define.MaxCount.invenSlotCount; i++)
        {

            //Debug.Log(i + $" : " + ((Define.ScriptableItem)invenLoadDict[i].itemInfo).ToString() + "\n������ ���� : " + invenLoadDict[i].Count);

            //Debug.Log(itemSlots[i].name);

        }

        for (int j = 0; j < MaxCount.invenSlotCount; j++)
        {
            itemSlots[j].itemCount = invenLoadDict[j].Count;
            itemSlots[j].itemInfo = ItemManager.Instance.itemList[(int)invenLoadDict[j].itemInfo];
        }

        UIManager.Instance.inven.InvenRefresh();
    }



    /// <summary>
    /// �κ��丮 ���� ����
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
