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
    /// 시스템 메세지를 띄워주는 함수
    /// </summary>
    /// <param name="message">띄워줄 메세지 내용</param>
    /// <param name="sec">띄워줄 시간</param>
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
