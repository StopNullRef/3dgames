using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField]
    public Dictionary<int, string> textDict = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
        textDict.Add(1000, "Àß¸øµÈ ");
        DataManager.Instance.ToJson("Assets/Json/TestJson.json", textDict,true);


        //JsonUtility.ToJson(DataManager.Instance.FromJson<object>("Assets/Json/TestJson.json"), true);
        //JsonUtility.ToJson(DataManager.Instance.PrettyToJson(textDict), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


