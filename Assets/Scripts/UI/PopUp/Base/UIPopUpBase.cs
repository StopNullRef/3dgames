using Project.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpBase : UIBase
{
    protected Transform okay;
    protected Transform cancel;

    protected Button okayButton;
    protected Button cancelButton;


    [SerializeField,Tooltip("팝업창에 설명 해주는 부분")]
    public Text description;


    public override void Start()
    {
        base.Start();
    }

    protected virtual void Initialize()
    {
        okay = transform.GetChild(0);
        okayButton = okay.GetComponent<Button>();
        cancel = transform.GetChild(1);
        cancelButton = cancel.GetComponent<Button>();
    }

}
