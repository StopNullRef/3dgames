using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    Color originColor; // 처음 선택 안될때 배경 컬러
    Color selectColor; // 선택 될때 바꿔줄 배경 컬러

    CharacterController character; // 캐릭터 컨트롤러에 tool 을 보고 바꿔줌

    public Dictionary<Define.Tool, Image> toolSlotIcons = new Dictionary<Define.Tool, Image>();

    // Start is called before the first frame update
    void Start()
    {
        originColor = new Color(255, 255, 255); // 흰색
        selectColor = new Color(0, 255, 0);     // 초록색

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
