using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestButton : MonoBehaviour
{
    GameObject go;
    CameraController cameraController;
    RaycastHit hit;


    /// <summary>
    /// 실제로 지을 object
    /// </summary>
    public GameObject building;

    /// <summary>
    /// 건물 지을수 있는지 체크해주는
    /// 바닥에 collider 체크용
    /// </summary>
    public GameObject plane;

    /// <summary>
    /// 실제로 객체 생성시켜줄
    /// collider 체크용 plane
    /// </summary>
    GameObject colCheck;

    /// <summary>
    /// hit.point에 해당 GameObject의
    /// 높이에 반을 더해서 넣어줄 변수
    /// </summary>
    Vector3 point = Vector3.zero;

    public Mesh colmesh;

    Color redC;
    Color greenC;



    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        redC = new Color(255, 0, 0);
        greenC = new Color(0, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (go == null)
            return;


        if (cameraController.cameraState == Define.CameraState.Build)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, Define.LayerNum.grassLayer))
            {
                Vector3 point = new Vector3(hit.point.x, (go.GetComponent<MeshRenderer>().bounds.size.y / 2) + hit.point.y, hit.point.z);

                go.transform.position = point;
                //colCheck.transform.position = go.transform.position;
                //colCheck.transform.position = new Vector3(go.transform.position.x, hit.point.y + 0.1f, go.transform.position.z);

            }
        }
    }

    public void SetButtonEvent()
    {
        if (cameraController.cameraState != Define.CameraState.Build)
            return;

        go = Instantiate(building);
        go.name = "Object";

        MeshCollider meshCollider = go.GetComponent<MeshCollider>();

        MeshRenderer goMeshRen = go.GetComponent<MeshRenderer>();
        Rect ObjectRect = new Rect(0, 0, goMeshRen.bounds.size.x, goMeshRen.bounds.size.y);


        MeshRenderer buildingMeshRender = building.GetComponent<MeshRenderer>();
        Rect colcheckRect = new Rect(0, 0, buildingMeshRender.bounds.size.x, buildingMeshRender.bounds.size.y);

        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float startX = colcheckRect.xMin;
        float endX = colcheckRect.xMin + colcheckRect.width;

        float startY = colcheckRect.yMin;
        float endY = colcheckRect.yMin + colcheckRect.height;
         
        for (float i = startX; i < colcheckRect.width;  i += endX)
        {
            for(float j = startY; j< colcheckRect.height; j+= endY)
            {
                //go.transform.position.x + 
                Vector3 planePos = new Vector3(go.transform.position.x + i, go.GetComponent<MeshRenderer>().bounds.min.y + 0.01f, go.transform.position.z +j);
                Instantiate(plane, planePos, Quaternion.identity, go.transform);
            }
        }

        //colCheck.name = "ColliderCheckPlane"; 

        Material goMat = go.GetComponent<MeshRenderer>().material;
        Color goColor = goMat.color;
        goColor.a = 0.5f;
        goMat.color = goColor;
    }

}