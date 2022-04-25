using Define;
using Project.Inven;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    /// <summary>
    /// Player
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 캐릭터 기준으로 카메라 위치 넣어줄 벡터 변수
    /// </summary>
    Vector3 cameraPos;

    /// <summary>
    /// 카메라가 움직이는 속도
    /// </summary>
    [SerializeField]
    float _cameraMoveSpeed;

    public Define.CameraState cameraState = Define.CameraState.None;

    /// <summary>
    /// build상태가 될때 옮겨줄 위치
    /// </summary>
    Vector3 destination = Vector3.zero;

    /// <summary>
    /// 키입력 받을 Dictionary
    /// </summary>
    public Dictionary<KeyCode, Action> keyDict = new Dictionary<KeyCode, Action>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        KeyInitialize();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDict)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }

        switch (cameraState)
        {
            case CameraState.Build:
                OnUpdateBuild();
                break;
            case CameraState.None:
                FollowToPlayer();
                break;
        }
    }
    //캐릭터 기준 카메라 위치
    //    x y   z
    //pos  0   5   7.5
    //rot   30 180 0

    /// <summary>
    /// 캐릭터 기준 카메라를 움직여주는 함수
    /// </summary>
    void FollowToPlayer()
    {
        cameraPos.x = player.transform.position.x;
        cameraPos.y = player.transform.position.y + 5;
        cameraPos.z = player.transform.position.z + 7.5f;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraPos,
            _cameraMoveSpeed * Time.deltaTime);

        //gameObject.transform.rotation = Quaternion.Euler(30, 180, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30, 180, 0), _cameraMoveSpeed * Time.deltaTime);
    }


    /// <summary>
    /// 카메라 상태에 따라 key입력 받는 이벤트 초기화
    /// </summary>
    void KeyInitialize()
    {

        /// <summary>
        /// keyDictionary에 등록해주는 함수
        /// </summary>
        /// <param name="keyCode">입력 받을 key값</param>
        /// <param name="action">작동 시켜줄 함수</param>
        void KeyDictRegist(KeyCode keyCode, Action action)
        {
            keyDict.Add(keyCode, action);
        }

        KeyDictRegist(KeyCode.F1, () =>
        {
            cameraState = CameraState.Build;

            var ingame = IngameManager.Instance;

            ingame.canCusorChange = false;

            var uiManager = UIManager.Instance;
            uiManager.CameraStateToSetCanvas(cameraState);
            ingame.BuildingSystem.Initialize();
            //uiManager.GetUI<BuildingInven>().BuildPoolRigist();
        });

        KeyDictRegist(KeyCode.Escape, () =>
        {
            if (cameraState == CameraState.Build)
                IngameManager.Instance.canCusorChange = true;
            else
                return;

            cameraState = CameraState.None;

            UIManager.Instance.CameraStateToSetCanvas(cameraState);
        });

    }



    /// <summary>
    /// 카메라 state가
    /// 빌드 상태일때 카메라 위치 
    /// 설정해주는 함수
    /// </summary>
    void OnUpdateBuild()
    {
        //pos 0 20 7.5
        //rot 60 180 0
        destination.x = transform.position.x;
        destination.y = 20f;
        destination.z = 7.5f;

        transform.position = Vector3.MoveTowards(transform.position, destination, _cameraMoveSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(60, 180, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(60, 180, 0), _cameraMoveSpeed * Time.deltaTime);
    }
}
