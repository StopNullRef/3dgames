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
    /// ĳ���� �������� ī�޶� ��ġ �־��� ���� ����
    /// </summary>
    Vector3 cameraPos;

    /// <summary>
    /// ī�޶� �����̴� �ӵ�
    /// </summary>
    [SerializeField]
    float _cameraMoveSpeed;

    public Define.CameraState cameraState = Define.CameraState.None;

    /// <summary>
    /// build���°� �ɶ� �Ű��� ��ġ
    /// </summary>
    Vector3 destination = Vector3.zero;

    /// <summary>
    /// Ű�Է� ���� Dictionary
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
    //ĳ���� ���� ī�޶� ��ġ
    //    x y   z
    //pos  0   5   7.5
    //rot   30 180 0

    /// <summary>
    /// ĳ���� ���� ī�޶� �������ִ� �Լ�
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
    /// ī�޶� ���¿� ���� key�Է� �޴� �̺�Ʈ �ʱ�ȭ
    /// </summary>
    void KeyInitialize()
    {

        /// <summary>
        /// keyDictionary�� ������ִ� �Լ�
        /// </summary>
        /// <param name="keyCode">�Է� ���� key��</param>
        /// <param name="action">�۵� ������ �Լ�</param>
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
    /// ī�޶� state��
    /// ���� �����϶� ī�޶� ��ġ 
    /// �������ִ� �Լ�
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
