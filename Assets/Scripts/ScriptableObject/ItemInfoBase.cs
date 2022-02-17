using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoBase : ScriptableObject
{
    [SerializeField, Tooltip("�������̸�")]
    public string itemName;
    [SerializeField, Tooltip("������ ����")]
    public string description;
    [SerializeField, Tooltip("������ Sprite")]
    public Sprite sprite;
    [SerializeField, Tooltip("�������ڵ�")]
    public int itemCode;
}
