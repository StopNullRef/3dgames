using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoBase : ScriptableObject
{
    [SerializeField, Tooltip("아이템이름")]
    public string itemName;
    [SerializeField, Tooltip("아이템 설명")]
    public string description;
    [SerializeField, Tooltip("아이템 Sprite")]
    public Sprite sprite;
    [SerializeField, Tooltip("아이템코드")]
    public int itemCode;
}
