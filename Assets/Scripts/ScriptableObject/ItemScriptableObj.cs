using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObj", order = 1)]
public class ItemScriptableObj : ScriptableObject
{
    [SerializeField,Tooltip("아이템이름")]
    public string itemName;
    [SerializeField, Tooltip("아이템코드")]
    public int itemCode;
    [SerializeField,Tooltip("아이템타입")]
    public Define.ItemType itemKind;
    [SerializeField, Tooltip("아이템 아이콘 이미지")]
    public Texture image;
    [SerializeField, Tooltip("아이템 설명")]
    public string description;
    [SerializeField, Tooltip("아이템 Sprite")]
    public Sprite sprite;

    /// <summary>
    /// 이게 무슨 아이템인지
    /// </summary>
    [SerializeField]
    public Define.ScriptableItem ScriptableItem;
}

