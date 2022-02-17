using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Tooltip("fillamount 값을 넣어줄 이미지 변수")]
    public Image fillImage; // fillamount 값을 넣어줄 이미지 변수

    [Tooltip("fillamount 값을 따라 움직이는 캐릭터")]
    public GameObject character; // fillamount 값을 따라 움직이는 캐릭터

    [Tooltip("fillamount 값을 가지는 GameObject의 Transform")]
    public Transform childFill; // fillamount 값을 가지는 GameObject의 Transform

    [Tooltip("로딩창 캔버스")]
    public Canvas loadingCanvas; // 로딩창 캔버스

    float time; // 비동기 식 씬전환을 할때 고의 적으로 지연 시키기 위한 시간변수

    float textTime; // 로딩중 텍스트에 타이핑 효과를 주기위해 시간을 재는 변수

    string loadingTextStr; // 로딩 텍스트에 들어갈 변수

    string dot; // 로딩 텍스트에 들어갈 변수

    const int loadingDelayTime = 8; // 로딩 딜레이해줄 시간
    const int loadingTextResetTime = 6; // 로딩중 텍스트 리셋 시간
    const int dotCycle = 1; // .이 텍스트에 들어가는 주기

    RectTransform fillRect;

    public Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        dot = ".";
        loadingTextStr = "로딩중.";

        fillImage = childFill.GetComponent<Image>();

        //// fillImage 변수에 RectTransform를 받아 넓이를 이용해서 캐릭터를 이동시켜주기위해 받음
        fillRect = fillImage.GetComponent<RectTransform>();

        // 씬 매니저에 있는 sceneNum의 변수에 따라 로딩창 후 씬을 바꿔줌
        SubRoutine(SceneManager.Instance.sceneNum);
        //SubRoutine(UIManager.Instance.dropdown.dropdown.value+1);
    }

    public void SubRoutine(int i)
    {
        StartCoroutine(LoadingScene(i));
    }

    IEnumerator LoadingScene(string sceneName) // 씬로드 하면서 로딩 게이지 채워주고 캐릭터 움직이는 함수 
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            yield return null;

            time += Time.deltaTime;
            textTime += Time.deltaTime;

            // ao 의 progress가 0.9가 최대치라 0.99가 되게끔 더해줌
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, (ao.progress + 0.09f), Time.deltaTime);

            // 캐릭터의 로컬 포지션을 해당 fillamount의 Rect의 넓이를 이용해서 lerp를 걸어 캐릭터를 이동
            character.transform.localPosition =
                new Vector3(Mathf.Lerp((-fillRect.rect.width / 2), (fillRect.rect.width / 2), fillImage.fillAmount),
                character.transform.localPosition.y, character.transform.localPosition.z);

            //처음이거나 문자열의 길이가 6이 넘었을때 다시 처음으로 초기화
            if (textTime < 0.3 || (loadingText.text.Length > loadingTextResetTime))
            {
                loadingText.text = loadingTextStr;
            }
            // 1초가 지날때마다 텍스트에 . 을 추가 시켜줌
            else if (textTime >= dotCycle)
            {
                loadingText.text += dot;
                textTime = 0.4f;
            }

            // 비동기화 로딩이 완료가 되어도 일부러 씬을 이동안시켜주고 지연시켜주는 부분
            if (time >= loadingDelayTime)
            {
                ao.allowSceneActivation = true;
            }

        }
    }

    IEnumerator LoadingScene(int indexNum)// 씬로드 하면서 로딩 게이지 채워주고 캐릭터 움직이는 함수 
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(indexNum);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {

            time += Time.deltaTime;
            textTime += Time.deltaTime;

            // ao 의 progress가 0.9가 최대치라 0.99가 되게끔 더해줌
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, (ao.progress + 0.09f), Time.deltaTime);

            // 캐릭터의 로컬 포지션을 해당 fillamount의 Rect의 넓이를 이용해서 lerp를 걸어 캐릭터를 이동
            character.transform.localPosition =
                new Vector3(Mathf.Lerp((-fillRect.rect.width / 2), (fillRect.rect.width / 2), fillImage.fillAmount),
                character.transform.localPosition.y, character.transform.localPosition.z);

            //처음이거나 문자열의 길이가 6이 넘었을때 다시 처음으로 초기화
            if (Mathf.Approximately(textTime,0) || (loadingText.text.Length > loadingTextResetTime))
            {
                loadingText.text = loadingTextStr;
            }
            // 1초가 지날때마다 텍스트에 . 을 추가 시켜줌
            else if (textTime >= dotCycle)
            {
                loadingText.text += dot;
                textTime = 0.1f;
            }

            // 비동기화 로딩이 완료가 되어도 일부러 씬을 이동안시켜주고 지연시켜주는 부분
            if (time >= loadingDelayTime)
            {
                SceneManager.Instance.sceneNum = indexNum;
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
