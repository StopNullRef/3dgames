using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BuildingSystem
{
    /// <summary>
    /// 건축시스템을 도와주는 Util 클래스
    /// </summary>
    public static class BuildingUtil
    {
        /// <summary>
        /// Grid Building System을 위해 collider체크를 하기 위한
        /// Rect 범위를 반환해주는 함수
        /// </summary>
        /// <param name="meshRenderer">한면의 rect 범위를 반환할 meshRenderer</param>
        /// <returns>meshRenderer를 이용해 한면의 사각범위를 반환</returns>
        public static Rect MakeRectRange(MeshRenderer meshRenderer)
        {
            Rect r = new Rect(0, 0, meshRenderer.bounds.size.x, meshRenderer.bounds.size.y);
            return r;
        }

        /// <summary>
        /// ColliderCheckPlane을 생성시켜주는 함수
        /// </summary>
        /// <param name="buildingObject">지을 건물</param>
        /// <param name="colliderCheckPlane">체크할  plane 바닥</param>
        /// <param name="buildingPos">건물을 지을 위치</param>
        public static void PlaneInstantiate(GameObject buildingObject, GameObject colliderCheckPlane, Vector3 buildingPos)
        {
            // 사각 plane의 사각 범위를 받아서
            Rect colcheckRect = MakeRectRange(colliderCheckPlane.GetComponent<MeshRenderer>());

            //스케일 맞춰주고
            colliderCheckPlane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            float startX = colcheckRect.xMin;
            float endX = colcheckRect.xMin + colcheckRect.width;

            float startY = colcheckRect.yMin;
            float endY = colcheckRect.yMin + colcheckRect.height;

            for (float i = startX; i < colcheckRect.width; i += endX)
            {
                for (float j = startY; j < colcheckRect.height; j += endY)
                {
                    Vector3 planePos = new Vector3(buildingObject.transform.position.x + i, buildingObject.GetComponent<MeshRenderer>().bounds.min.y + 0.01f, buildingObject.transform.position.z + j);
                    MonoBehaviour.Instantiate(colliderCheckPlane, planePos, Quaternion.identity, buildingObject.transform);
                }
            }
        }
    }

}
