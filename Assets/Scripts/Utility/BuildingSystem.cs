using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BuildingSystem
{
    /// <summary>
    /// ����ý����� �����ִ� Util Ŭ����
    /// </summary>
    public static class BuildingUtil
    {
        /// <summary>
        /// Grid Building System�� ���� colliderüũ�� �ϱ� ����
        /// Rect ������ ��ȯ���ִ� �Լ�
        /// </summary>
        /// <param name="meshRenderer">�Ѹ��� rect ������ ��ȯ�� meshRenderer</param>
        /// <returns>meshRenderer�� �̿��� �Ѹ��� �簢������ ��ȯ</returns>
        public static Rect MakeRectRange(MeshRenderer meshRenderer)
        {
            Rect r = new Rect(0, 0, meshRenderer.bounds.size.x, meshRenderer.bounds.size.y);
            return r;
        }

        /// <summary>
        /// ColliderCheckPlane�� ���������ִ� �Լ�
        /// </summary>
        /// <param name="buildingObject">���� �ǹ�</param>
        /// <param name="colliderCheckPlane">üũ��  plane �ٴ�</param>
        /// <param name="buildingPos">�ǹ��� ���� ��ġ</param>
        public static void PlaneInstantiate(GameObject buildingObject, GameObject colliderCheckPlane, Vector3 buildingPos)
        {
            // �簢 plane�� �簢 ������ �޾Ƽ�
            Rect colcheckRect = MakeRectRange(colliderCheckPlane.GetComponent<MeshRenderer>());

            //������ �����ְ�
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
