using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SceneDropDown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Dropdown dropdown; // 씬이동에 사용할 드롭다운

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.value = (int)SceneManager.Instance.sceneName -1;
        dropdown.options.Clear();
        dropdown.captionText.text = "맵 이동";

        // 해당 열거형  안에있는 갯수 구하는거
        //System.Enum.GetValues(typeof(Define.Scene)).Length;

        // 씬 열거형의 갯수 만큼 드롭다운에 옵션을 생성하고 옵션에 이름 넣어주는 부분
        for (int i = 1; i < System.Enum.GetValues(typeof(Define.Scene)).Length; i++)
        {
            Dropdown.OptionData opdata = new Dropdown.OptionData();
            opdata.text = SceneManager.Instance.GetSceneName(i);
            dropdown.options.Add(opdata);
        }
        // 드롭다운을 이용해서 로딩씬으로 넘어가고 해당 드롭다운 벨류 값을 로드씬 인덱스에서 넣어서 씬전환
        dropdown.onValueChanged.AddListener((int i) => 
        {
            i = dropdown.value;
            DataManager.Instance.InvenSave();

            //드롭다운을 통해 씬이동을 해주는데 선택된 부분이 같을경우 씬을 이동시켜주지 않는다
            if (i+1 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                return;
            }
            // 드롭 다운 인덱스 번호 가 0번부터인데 씬은 0번이 로딩씬이라서 1번부터 사용하려고 i+1을해줌
            SceneManager.Instance.sceneNum = i+1;

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        }
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 해당 드랍다운에 마우스가 들어오면 커서 기본 커서로 나오게끔
        CursorManager.Instance.canCusorChange = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 나갔으면 나갔다고 체크
        ((IPointerExitHandler)dropdown).OnPointerExit(eventData);
        CursorManager.Instance.canCusorChange = false;
    }
}
