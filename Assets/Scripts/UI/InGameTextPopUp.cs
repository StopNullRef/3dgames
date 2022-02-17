using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTextPopUp : MonoBehaviour
{
    public Text middleButtom;

    Color uiColor;

    float time;

    private void Start()
    {
        uiColor = middleButtom.color;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > 3)
        {
            uiColor.a = Mathf.Lerp(uiColor.a, 0, Time.deltaTime *2f);
            middleButtom.color = uiColor;
        }
    }


}
