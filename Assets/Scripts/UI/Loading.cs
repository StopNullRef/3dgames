using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Tooltip("fillamount ���� �־��� �̹��� ����")]
    public Image fillImage; // fillamount ���� �־��� �̹��� ����

    [Tooltip("fillamount ���� ���� �����̴� ĳ����")]
    public GameObject character; // fillamount ���� ���� �����̴� ĳ����

    [Tooltip("fillamount ���� ������ GameObject�� Transform")]
    public Transform childFill; // fillamount ���� ������ GameObject�� Transform

    [Tooltip("�ε�â ĵ����")]
    public Canvas loadingCanvas; // �ε�â ĵ����

    float time; // �񵿱� �� ����ȯ�� �Ҷ� ���� ������ ���� ��Ű�� ���� �ð�����

    float textTime; // �ε��� �ؽ�Ʈ�� Ÿ���� ȿ���� �ֱ����� �ð��� ��� ����

    string loadingTextStr; // �ε� �ؽ�Ʈ�� �� ����

    string dot; // �ε� �ؽ�Ʈ�� �� ����

    const int loadingDelayTime = 8; // �ε� ���������� �ð�
    const int loadingTextResetTime = 6; // �ε��� �ؽ�Ʈ ���� �ð�
    const int dotCycle = 1; // .�� �ؽ�Ʈ�� ���� �ֱ�

    RectTransform fillRect;

    public Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        dot = ".";
        loadingTextStr = "�ε���.";

        fillImage = childFill.GetComponent<Image>();

        //// fillImage ������ RectTransform�� �޾� ���̸� �̿��ؼ� ĳ���͸� �̵������ֱ����� ����
        fillRect = fillImage.GetComponent<RectTransform>();

        // �� �Ŵ����� �ִ� sceneNum�� ������ ���� �ε�â �� ���� �ٲ���
        SubRoutine(SceneManager.Instance.sceneNum);
        //SubRoutine(UIManager.Instance.dropdown.dropdown.value+1);
    }

    public void SubRoutine(int i)
    {
        StartCoroutine(LoadingScene(i));
    }

    IEnumerator LoadingScene(string sceneName) // ���ε� �ϸ鼭 �ε� ������ ä���ְ� ĳ���� �����̴� �Լ� 
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            yield return null;

            time += Time.deltaTime;
            textTime += Time.deltaTime;

            // ao �� progress�� 0.9�� �ִ�ġ�� 0.99�� �ǰԲ� ������
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, (ao.progress + 0.09f), Time.deltaTime);

            // ĳ������ ���� �������� �ش� fillamount�� Rect�� ���̸� �̿��ؼ� lerp�� �ɾ� ĳ���͸� �̵�
            character.transform.localPosition =
                new Vector3(Mathf.Lerp((-fillRect.rect.width / 2), (fillRect.rect.width / 2), fillImage.fillAmount),
                character.transform.localPosition.y, character.transform.localPosition.z);

            //ó���̰ų� ���ڿ��� ���̰� 6�� �Ѿ����� �ٽ� ó������ �ʱ�ȭ
            if (textTime < 0.3 || (loadingText.text.Length > loadingTextResetTime))
            {
                loadingText.text = loadingTextStr;
            }
            // 1�ʰ� ���������� �ؽ�Ʈ�� . �� �߰� ������
            else if (textTime >= dotCycle)
            {
                loadingText.text += dot;
                textTime = 0.4f;
            }

            // �񵿱�ȭ �ε��� �Ϸᰡ �Ǿ �Ϻη� ���� �̵��Ƚ����ְ� ���������ִ� �κ�
            if (time >= loadingDelayTime)
            {
                ao.allowSceneActivation = true;
            }

        }
    }

    IEnumerator LoadingScene(int indexNum)// ���ε� �ϸ鼭 �ε� ������ ä���ְ� ĳ���� �����̴� �Լ� 
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(indexNum);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {

            time += Time.deltaTime;
            textTime += Time.deltaTime;

            // ao �� progress�� 0.9�� �ִ�ġ�� 0.99�� �ǰԲ� ������
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, (ao.progress + 0.09f), Time.deltaTime);

            // ĳ������ ���� �������� �ش� fillamount�� Rect�� ���̸� �̿��ؼ� lerp�� �ɾ� ĳ���͸� �̵�
            character.transform.localPosition =
                new Vector3(Mathf.Lerp((-fillRect.rect.width / 2), (fillRect.rect.width / 2), fillImage.fillAmount),
                character.transform.localPosition.y, character.transform.localPosition.z);

            //ó���̰ų� ���ڿ��� ���̰� 6�� �Ѿ����� �ٽ� ó������ �ʱ�ȭ
            if (Mathf.Approximately(textTime,0) || (loadingText.text.Length > loadingTextResetTime))
            {
                loadingText.text = loadingTextStr;
            }
            // 1�ʰ� ���������� �ؽ�Ʈ�� . �� �߰� ������
            else if (textTime >= dotCycle)
            {
                loadingText.text += dot;
                textTime = 0.1f;
            }

            // �񵿱�ȭ �ε��� �Ϸᰡ �Ǿ �Ϻη� ���� �̵��Ƚ����ְ� ���������ִ� �κ�
            if (time >= loadingDelayTime)
            {
                SceneManager.Instance.sceneNum = indexNum;
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
