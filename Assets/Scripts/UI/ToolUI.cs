using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    Color originColor; // ó�� ���� �ȵɶ� ��� �÷�
    Color selectColor; // ���� �ɶ� �ٲ��� ��� �÷�

    CharacterController character; // ĳ���� ��Ʈ�ѷ��� tool �� ���� �ٲ���

    public Dictionary<Define.Tool, Image> toolSlotIcons = new Dictionary<Define.Tool, Image>();

    // Start is called before the first frame update
    void Start()
    {
        originColor = new Color(255, 255, 255); // ���
        selectColor = new Color(0, 255, 0);     // �ʷϻ�

        character = GameObject.FindObjectOfType<CharacterController>();

        for (int i = 0; i < transform.childCount; i++)
        {
            toolSlotIcons.Add((Define.Tool)i, transform.GetChild(i).GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetUIColor();
    }

    void SetUIColor()
    {
        foreach (var icon in toolSlotIcons)
            icon.Value.color = originColor;

        toolSlotIcons[character.tool].color = selectColor;
    }
}
