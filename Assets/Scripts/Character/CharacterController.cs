using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// 캐릭터 상태
    /// </summary>
    Define.State state = Define.State.Idle;

    /// <summary>
    /// 캐릭터가 들고있는 도구
    /// </summary>
    public Define.Tool tool = Define.Tool.None; //다른 곳에서 접근해서 손에 뭘들고잇는지 체크할 변수

    [SerializeField, Tooltip("이동속도")]
    float _moveSpeed = 0;

    Animator _anim = null;

    /// <summary>
    /// 캐릭터가 이동할 목적지
    /// </summary>
    Vector3 destination;

    /// <summary>
    /// 캐릭터가 이동할 목적지방향
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// 도구 들고 있을 캐릭터 왼손
    /// </summary>
    public Transform leftHand;

    /// <summary>
    /// 도구 들고 있을 캐릭터 오른손
    /// </summary>
    public Transform RightHand;

    /// <summary>
    /// 도끼    GameObject
    /// </summary>
    private GameObject axe; 

    /// <summary>
    /// 곡괭이  GameObject
    /// </summary>
    private GameObject pickaxe;

    Tool[] toolArr;

    GameObject objectTarget;
    Vector3 objectTargetPos;


    float distance = 0;

    public float objectDestroyTime;

    /// <summary>
    /// animationTransition의 string 값을 가지고 있는 리스트
    /// </summary>
    List<string> animTransitions = new List<string>();

    /// <summary>
    /// mainCamera의 controller
    /// </summary>
    CameraController cameraController;

    /// <summary>
    /// 현재 활동중인지 체크 하는함수
    /// </summary>
    bool isAct = false;

    /// <summary>
    /// 채집 시간 체크용 변수
    /// </summary>
    const float actionEndTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        axe = leftHand.GetChild(0).gameObject;
        pickaxe = RightHand.GetChild(0).gameObject;
        _anim = gameObject.GetComponent<Animator>();

        //매개변수는 비활성화된 객체도 GetComponet 할것인지 물어보는것
        //지금은 처음에 비활성화 된상태에서 특정키 입력을 받아 바꾸는 것임으로
        //매개변수에 true를 줌
        toolArr = transform.GetComponentsInChildren<Tool>(true);

        for (CharacerAnimTransition i = 0; i < CharacerAnimTransition.End; i++)
        {
            animTransitions.Add(i.ToString());
        }

        cameraController = Camera.main.GetComponent<CameraController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cameraController.cameraState != Define.CameraState.None)
        {
            //카메라가 빌드 상태로 될때 모든 애니메이션 종료
            foreach (string str in animTransitions)
            {
                _anim.SetBool(str, false);
            }
            return;
        }

        switch (state)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Move:

                UpdateMove();
                break;
            case Define.State.Action:
                UpdateAction();
                break;
        }
        OnClickAction();
        Emotion();
        OnClickMove();
        HoldTool();
    }

    #region OnClick
    void OnClickAction() // 클릭해서 액션 되게 처리해주는함수
    {
        if (EventSystem.current.IsPointerOverGameObject() || (isAct == true))
            return;


        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;

            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, Define.LayerNum.objLayer);

            if (hit.transform?.gameObject == null)
                return;


            OnClickChangeTool(hit);

            objectTarget = hit.transform.gameObject;
            objectTargetPos = objectTarget.transform.position;

            //캐릭터 위치랑 클릭된 object 랑 거리가 짧으면 바로 action상태로 바꿔줌
            if (Mathf.Approximately((transform.position - objectTarget.transform.position).magnitude, Define.MaxCount.objectToDistance))
            {
                state = Define.State.Action;
            }
            else
            {
                //캐릭터 위치랑 object 위치랑 멀다면 이동해줘서 가까워 지게끔 한다음 ation으로 바꿔줌
                state = Define.State.Move;
                _anim.SetBool("IsMoving", true);
            }

            isAct = true;
        }
    }

    // 마우스 클릭이 있었는지 체크해주는 함수
    void OnClickMove() // 클릭 이벤트 받은곳이랑 이동해주는곳 함수 따로따로 만들어야된다.
    {
        // 0 left 1 right 2 middle
        if (Input.GetMouseButton(1))
        {
            if (objectTarget != null)
                objectTarget = null;

            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

            destination = hit.point;
            destination.y = 0;

            direction = (destination - gameObject.transform.position);
            direction.y = 0;

            _anim.SetBool("IsMoving", true);

            state = Define.State.Move;
        }

    }

    #endregion

    void UpdateIdle()
    {
        _anim.SetBool("IsMoving", false);

        if (isAct == true)
        {
            isAct = false;
        }

    }


    #region Move
    void UpdateMove() // 마우스 클릭 받은 좌표로 캐릭터 이동시켜주는 함수
    {

        if (state != Define.State.Move)
            return;

        if (objectTarget == null)
        {
            characterMove(destination);
            if (Mathf.Approximately(distance, 0)) // 스칼라 값은 정확하게 떨어지지 않으므로 근사 치를 검사해주는 함수 Mathf.Approximately
            {
                state = Define.State.Idle;
            }
        }
        else
        {
            characterMove(objectTargetPos);
            if (distance < 0.5f)
            {
                state = Define.State.Action;
                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsWood", true);
                StartCoroutine(ObjectAction());
            }
        }
    }

    void characterMove(Vector3 charDestination)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, charDestination, _moveSpeed * Time.deltaTime);

        gameObject.transform.LookAt(charDestination);

        distance = (charDestination - transform.position).magnitude; // 목적지랑 캐릭터 거리 값
    }
    #endregion


    #region Action
    void UpdateAction()
    {
        if (Input.GetMouseButton(1))
        {
            _anim.SetBool("IsWood", false);
            _anim.SetBool("IsMoving", true);
            objectTarget = null;
            state = Define.State.Move;
        }
    }

    void Emotion() // 캐릭터 감정표현 애니메이션 작동하게 해주는 함수
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) // 숫자 7키를 눌렀을때 경례
        {
            _anim.SetBool("IsSalute", true);
        }

        // 경례중 이동하거나 애니메이션 끝나면 취소 하고 Idle애니메이션 작동
        if ((_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f) || Input.GetMouseButton(1))
        {
            _anim.SetBool("IsSalute", false);
        }
    }

    IEnumerator ObjectAction()
    {

        var inven = UIManager.Instance.GetUI<InventoryHandler>();

        var poolManager = PoolManager.Instance;

        while (true)
        {
            if (state != State.Action)
            {
                objectDestroyTime = 0f;
                yield break;
            }

            objectDestroyTime += Time.deltaTime;
            if (objectDestroyTime > actionEndTime)
            {
                inven.AddItem(objectTarget.transform.parent.GetComponent<ObjInfo>());
                poolManager.PoolListAdd(objectTarget);
                foreach (var anim in animTransitions)
                {
                    _anim.SetBool(anim, false);
                }

                objectDestroyTime = 0f;
                state = Define.State.Idle;
                yield break;
            }

            yield return null;
        }
    }


    #endregion

    #region Tool
    /// <summary>
    /// 캐릭터 손에 도구를 쥐어주는 함수
    /// </summary>
    void HoldTool()
    {
        // 맨손 모든 착용된걸 해제함
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToolListInit(Define.Tool.None);
        }

        // 도끼착용
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToolListInit(Define.Tool.Axe);
        }

        // 곡괭이 착용
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToolListInit(Define.Tool.PickAxe);
        }
    }

    /// <summary>
    /// 해당 도구를 활성화 시키고
    /// 나머지 도구들은 비활성화 시켜주는 함수
    /// </summary>
    /// <param name="tool">바꿔줄 도구</param>
    void ToolListInit(Define.Tool tool)
    {
        if (tool == Define.Tool.None)
        {
            axe.SetActive(false);
            pickaxe.SetActive(false);
            this.tool = tool;
            return;
        }
        for (int i = 0; i < toolArr.Length; i++)
        {
            if (toolArr[i].tool == tool)
            {
                this.tool = tool;

                toolArr[i].gameObject.SetActive(true);

            }
            else
            {
                toolArr[i].gameObject.SetActive(false);
            }
        }
    }

    void OnClickChangeTool(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Tree"))
        {
            ToolListInit(Define.Tool.Axe);
        }
    }
    #endregion
}
