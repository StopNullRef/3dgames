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
    /// Ű �Է��� ������ ��������� �޼��带����ִ� Dictionary
    /// </summary>
    public Dictionary<KeyCode, Action> inputEventDict;

    /// <summary>
    /// �Է� �̺�Ʈ�� ������ ��ųʸ��� ���  ���ִ� �Լ�
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="inputEventMethod"></param>
    public void InputEventRigist(KeyCode keyCode, Action inputEventMethod)
    {
        // ���ӸŴ������� �����ϴ� �Է�Ŭ������ �̹� ��ϵȰ� ������
        // return������
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
    /// �ش� Ű�Է� �̺�Ʈ�� ������Ʈ ���� ��������� �Լ�
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
