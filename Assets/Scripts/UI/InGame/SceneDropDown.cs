
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Project.UI;

public class SceneDropDown : UIBase, IPointerEnterHandler, IPointerExitHandler
{
    public Dropdown dropdown; // ���̵��� ����� ��Ӵٿ�

    // Start is called before the first frame update
    public override void Start()
    {
        isOpen = true;
        base.Start();
        Init();
    }

    public void Init()
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.value = (int)SceneManager.Instance.sceneName -1;
        dropdown.options.Clear();
        dropdown.captionText.text = "�� �̵�";

        // �� �������� ���� ��ŭ ��Ӵٿ �ɼ��� �����ϰ� �ɼǿ� �̸� �־��ִ� �κ�
        for (int i = 1; i < System.Enum.GetValues(typeof(Define.Scene)).Length; i++)
        {
            Dropdown.OptionData opdata = new Dropdown.OptionData();
            opdata.text = SceneManager.Instance.GetSceneName(i);
            dropdown.options.Add(opdata);
        }
        // ��Ӵٿ��� �̿��ؼ� �ε������� �Ѿ�� �ش� ��Ӵٿ� ���� ���� �ε�� �ε������� �־ ����ȯ
        dropdown.onValueChanged.AddListener((int i) => 
        {
            i = dropdown.value;
            SceneManager.Instance.stores.Clear();
            DataManager.Instance.InvenSave();

            //��Ӵٿ��� ���� ���̵��� ���ִµ� ���õ� �κ��� ������� ���� �̵��������� �ʴ´�
            if (i+1 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                return;
            }
            // ��� �ٿ� �ε��� ��ȣ �� 0�������ε� ���� 0���� �ε����̶� 1������ ����Ϸ��� i+1������
            SceneManager.Instance.sceneNum = i+1;

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        }
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �ش� ����ٿ ���콺�� ������ Ŀ�� �⺻ Ŀ���� �����Բ�
        IngameManager.Instance.canCusorChange = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �������� �����ٰ� üũ
        ((IPointerExitHandler)dropdown).OnPointerExit(eventData);
        IngameManager.Instance.canCusorChange = false;
    }

    public override void Close(bool intialValue = false)
    {
        base.Close(intialValue);
    }
    public override void Open(bool initialValue = false)
    {
        base.Open(initialValue);
    }
}
