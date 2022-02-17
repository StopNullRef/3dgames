using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObj", order = 1)]
public class ItemScriptableObj : ScriptableObject
{
    [SerializeField,Tooltip("�������̸�")]
    public string itemName;
    [SerializeField, Tooltip("�������ڵ�")]
    public int itemCode;
    [SerializeField,Tooltip("������Ÿ��")]
    public Define.ItemType itemKind;
    [SerializeField, Tooltip("������ ������ �̹���")]
    public Texture image;
    [SerializeField, Tooltip("������ ����")]
    public string description;
    [SerializeField, Tooltip("������ Sprite")]
    public Sprite sprite;

    /// <summary>
    /// �̰� ���� ����������
    /// </summary>
    [SerializeField]
    public Define.ScriptableItem ScriptableItem;
}

