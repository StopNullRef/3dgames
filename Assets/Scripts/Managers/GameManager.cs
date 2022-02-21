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
    /// 기획데이터를 갖는 객체
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
        //씬이 변경될때 매니져가 가지고있어야될것들 초기화해주는 부분
        // awake onEnable 이 돌고 난후에 sceneloaded가 돈다
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
