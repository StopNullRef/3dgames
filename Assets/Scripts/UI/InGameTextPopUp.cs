using Project.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTextPopUp :  UIBase
{
    public Text middleButtom;

    Color uiColor;


    public override void Start()
    {
        isOpen = false;
        base.Start();
        uiColor = middleButtom.color;
    }

    private void Update()
    {

    }


    /// <summary>
    /// �ý��� �޼����� ����ִ� �Լ�
    /// </summary>
    /// <param name="message">����� �޼��� ����</param>
    /// <param name="sec">����� �ð�</param>
    /// <returns></returns>
    public IEnumerator SendSystemMessage(string message,int sec = 3)
    {
        float time = 0;
        middleButtom.text = message;
        isOpen = true;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;

            if (time > sec)
            {
                uiColor.a = Mathf.Lerp(uiColor.a, 0, Time.deltaTime * 2f);
                middleButtom.color = uiColor;
            }
        }
    }

}
