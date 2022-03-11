using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// ĳ���� ����
    /// </summary>
    Define.State state = Define.State.Idle;

    /// <summary>
    /// ĳ���Ͱ� ����ִ� ����
    /// </summary>
    public Define.Tool tool = Define.Tool.None; //�ٸ� ������ �����ؼ� �տ� ������մ��� üũ�� ����

    [SerializeField, Tooltip("�̵��ӵ�")]
    float _moveSpeed = 0;

    Animator _anim = null;

    /// <summary>
    /// ĳ���Ͱ� �̵��� ������
    /// </summary>
    Vector3 destination;

    /// <summary>
    /// ĳ���Ͱ� �̵��� ����������
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// ���� ��� ���� ĳ���� �޼�
    /// </summary>
    public Transform leftHand;

    /// <summary>
    /// ���� ��� ���� ĳ���� ������
    /// </summary>
    public Transform RightHand;

    /// <summary>
    /// ����    GameObject
    /// </summary>
    private GameObject axe; 

    /// <summary>
    /// ���  GameObject
    /// </summary>
    private GameObject pickaxe;

    Tool[] toolArr;

    GameObject objectTarget;
    Vector3 objectTargetPos;


    float distance = 0;

    public float objectDestroyTime;

    /// <summary>
    /// animationTransition�� string ���� ������ �ִ� ����Ʈ
    /// </summary>
    List<string> animTransitions = new List<string>();

    /// <summary>
    /// mainCamera�� controller
    /// </summary>
    CameraController cameraController;

    /// <summary>
    /// ���� Ȱ�������� üũ �ϴ��Լ�
    /// </summary>
    bool isAct = false;

    /// <summary>
    /// ä�� �ð� üũ�� ����
    /// </summary>
    const float actionEndTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        axe = leftHand.GetChild(0).gameObject;
        pickaxe = RightHand.GetChild(0).gameObject;
        _anim = gameObject.GetComponent<Animator>();

        //�Ű������� ��Ȱ��ȭ�� ��ü�� GetComponet �Ұ����� ����°�
        //������ ó���� ��Ȱ��ȭ �Ȼ��¿��� Ư��Ű �Է��� �޾� �ٲٴ� ��������
        //�Ű������� true�� ��
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
            //ī�޶� ���� ���·� �ɶ� ��� �ִϸ��̼� ����
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
    void OnClickAction() // Ŭ���ؼ� �׼� �ǰ� ó�����ִ��Լ�
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

            //ĳ���� ��ġ�� Ŭ���� object �� �Ÿ��� ª���� �ٷ� action���·� �ٲ���
            if (Mathf.Approximately((transform.position - objectTarget.transform.position).magnitude, Define.MaxCount.objectToDistance))
            {
                state = Define.State.Action;
            }
            else
            {
                //ĳ���� ��ġ�� object ��ġ�� �ִٸ� �̵����༭ ����� ���Բ� �Ѵ��� ation���� �ٲ���
                state = Define.State.Move;
                _anim.SetBool("IsMoving", true);
            }

            isAct = true;
        }
    }

    // ���콺 Ŭ���� �־����� üũ���ִ� �Լ�
    void OnClickMove() // Ŭ�� �̺�Ʈ �������̶� �̵����ִ°� �Լ� ���ε��� �����ߵȴ�.
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
    void UpdateMove() // ���콺 Ŭ�� ���� ��ǥ�� ĳ���� �̵������ִ� �Լ�
    {

        if (state != Define.State.Move)
            return;

        if (objectTarget == null)
        {
            characterMove(destination);
            if (Mathf.Approximately(distance, 0)) // ��Į�� ���� ��Ȯ�ϰ� �������� �����Ƿ� �ٻ� ġ�� �˻����ִ� �Լ� Mathf.Approximately
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

        distance = (charDestination - transform.position).magnitude; // �������� ĳ���� �Ÿ� ��
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

    void Emotion() // ĳ���� ����ǥ�� �ִϸ��̼� �۵��ϰ� ���ִ� �Լ�
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) // ���� 7Ű�� �������� ���
        {
            _anim.SetBool("IsSalute", true);
        }

        // ����� �̵��ϰų� �ִϸ��̼� ������ ��� �ϰ� Idle�ִϸ��̼� �۵�
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
    /// ĳ���� �տ� ������ ����ִ� �Լ�
    /// </summary>
    void HoldTool()
    {
        // �Ǽ� ��� ����Ȱ� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToolListInit(Define.Tool.None);
        }

        // ��������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToolListInit(Define.Tool.Axe);
        }

        // ��� ����
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToolListInit(Define.Tool.PickAxe);
        }
    }

    /// <summary>
    /// �ش� ������ Ȱ��ȭ ��Ű��
    /// ������ �������� ��Ȱ��ȭ �����ִ� �Լ�
    /// </summary>
    /// <param name="tool">�ٲ��� ����</param>
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
