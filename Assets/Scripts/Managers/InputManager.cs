using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public InputManager()
    {
        inputEventDict = new Dictionary<KeyCode, Action>();
    }

    /// <summary>
    /// 키 입력을 받을때 실행시켜줄 메서드를들고있는 Dictionary
    /// </summary>
    public Dictionary<KeyCode, Action> inputEventDict;

    /// <summary>
    /// 입력 이벤트를 가지는 딕셔너리에 등록  해주는 함수
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="inputEventMethod"></param>
    public void InputEventRigist(KeyCode keyCode, Action inputEventMethod)
    {
        // 게임매니저에서 관리하는 입력클래스에 이미 등록된게 있으면
        // return시켜줌
        if (inputEventDict.ContainsKey(keyCode))
        {
            // if (inputEventDict[keyCode] == null)
            inputEventDict.Remove(keyCode);
            inputEventDict.Add(keyCode, inputEventMethod);
        }
        else
            inputEventDict.Add(keyCode, inputEventMethod);
    }

    /// <summary>
    /// 해당 키입력 이벤트를 업데이트 에서 실행시켜줄 함수
    /// </summary>
    public void OnUpdate()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in inputEventDict)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }
    }
}
