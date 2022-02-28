using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public Define.Scene sceneName = Define.Scene.LoadingScene; // ���� ���� �̸��� �־��� ����

    public int sceneNum; // �ε���� ���ֱ����� ����

    public Transform map; // ���̿���Ű���� ���� �������ִ� GameObejct�� �޴� ����

    protected override void Awake()
    {
        base.Awake();
        //�븮�ڷ� �Լ��� �־ ���� �ٲ� ȣ��ǰ� �� �Ҽ� �ִ�
        Init();
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneChangeInit;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneChangeInit;
    }

    public void Init()
    {
        // ���� �� �̸��� sceneName ������ �־���
        sceneName = (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        sceneNum = (int)sceneName;

        if (!(sceneName == (int)Define.Scene.LoadingScene))
        {
            map = GameObject.Find("Maps").transform.GetChild(0);
            ObjectGiveTagandLayer(Define.TagName.Tree, 10);
            ObjectGiveTagandLayer(Define.TagName.Grass, 11);
        }
    }

    public void OnUpdate()
    {

    }

    public void Clear()
    {

    }

    /// <summary>
    /// ���� ��ȯ�ɶ� �ٽ� �ʱ�ȭ ���ִ� �Լ�
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mod"></param>
    public void SceneChangeInit(Scene scene, LoadSceneMode mod)
    {
        sceneName = (Define.Scene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        // �� �ٲ� üũ ���ְ� �־��ִ� �κ�

        if (sceneName != Define.Scene.LoadingScene)
        {
            //Init();
            map = GameObject.Find("Maps").transform.GetChild(0);
            UIManager.Instance.UIInitialize();
           // UIManager.Instance.dropdown.dropdown.value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
            UIManager.Instance.GetUI<SceneDropDown>().dropdown.value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        }

    }

    // �ʿ� tag �� layer �־��ִ� �Լ�
    // map�� �ִ� GameObject �� �ձ��ں��� �˻縦 �ؼ� ������ tag�� �ش� object�� �ڽĿ��� layer �� �־���
    // �ڽĿ� �ִ� ������ �� �����Ͱ� �ش� ���ӿ�����Ʈ �ڽ��ʿ��� collider�� ������ �־
    public void ObjectGiveTagandLayer(Define.TagName tagName, int layer) // �˻��� �̸�, �־��� layer
    {
        // ó������ �־��ִ� ���� Best ������ �ʿ����ͷ� ���� ������ �־� �ֱⰡ ���� �ڵ�� �־���
        // ���� ���� ����� �ƴ�

        int count = 0;

        if (map.childCount == 0)
        {
            return;
        }
        string objName = tagName.ToString();

        Transform go;

        for (int j = 0; j < map.childCount; j++)
        {
            go = map.GetChild(j);
            for (int i = 0; i < objName.Length; i++)
            {
                if (go.name[i].ToString() == objName[i].ToString())
                {
                    count++;
                    if (count == objName.Length)
                    {
                        go.GetChild(0).gameObject.tag = objName;
                        go.GetChild(0).gameObject.layer = layer;
                    }
                }
            }
            count = 0;
        }
    }

    // ���ε��� ��ȣ�� �޾� ���̸��� �ѱ���� �ٲ��ִ� �Լ�
    /// <summary>
    ///  �� �ε����� �� DropDown�� option�� �� text string �� ��ȯ �Լ�
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public string GetSceneName(int i)
    {
        string _name = null;
        //0 �ε�
        //1 ��
        //2 ����
        //3 ��
        switch (i)
        {
            case 0:
                _name = "�ε�";
                break;
            case 1:
                _name = "��";
                break;
            case 2:
                _name = "����";
                break;
            case 3:
                _name = "��";
                break;
            default:
                break;
        }

        return _name;
    }

    public void ObjectAddComponent()
    {
        for (int i = 0; i < map.childCount; i++)
        {
            if (map.GetChild(i).GetComponent<ObjInfo>() == null)
            {
                map.GetChild(i).GetChild(0).gameObject.AddComponent<ObjInfo>();
            }
        }
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneChangeInit;
    }

}
