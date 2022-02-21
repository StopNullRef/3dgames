using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Project.SD;

public class GameManager : Singleton<GameManager>
{
    public bool isBuilding = false;

    InputManager inputEvent = new InputManager();

    public InputManager InputEvent { get => inputEvent; set => inputEvent = value; }

    Func<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> sceneInit;

    /// <summary>
    /// ��ȹ�����͸� ���� ��ü
    /// </summary>
    [SerializeField]
    private StaticDataModule sd = new StaticDataModule();
    public StaticDataModule SD => Instance.sd;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
            return;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SD.Initialize();
        //���� ����ɶ� �Ŵ����� �������־�ߵɰ͵� �ʱ�ȭ���ִ� �κ�
        // awake onEnable �� ���� ���Ŀ� sceneloaded�� ����
        SceneInitialize(UIManager.Instance.SceneChangeInit);
        SceneInitialize(SceneManager.Instance.SceneChangeInit);
        SceneInitialize(PoolManager.Instance.Init);
        SceneInitialize(DataManager.Instance.InvenDataPass);
    }


    private void Update()
    {
        inputEvent.OnUpdate();
    }

    private void SceneInitialize(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> methodName)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += methodName;
    }


}
