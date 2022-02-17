using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using Define;
using System;

public class InGameButton : MonoBehaviour
{
    /// <summary>
    /// save 버튼
    /// </summary>
    Button saveButton;

    /// <summary>
    /// 로드 버튼
    /// </summary>
    Button loadButton;

    public void Start()
    {
        saveButton = transform.GetChild(0).GetComponent<Button>();
        loadButton = transform.GetChild(1).GetComponent<Button>();
        OnClickInvenSave();
    }


    public void OnClickInvenSave()
    {

        saveButton.onClick.AddListener(() =>
        {
            DataManager.Instance.InvenSave();

        }
        );

        loadButton.onClick.AddListener(() =>
        {
            DataManager.Instance.InvenLoad();
        });
    }

}
