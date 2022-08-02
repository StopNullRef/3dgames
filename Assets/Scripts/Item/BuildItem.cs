using Project.DB;
using Project.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Object
{
    public class BuildItem : MonoBehaviour, IPoolableObject
    {
        public BoBuildItem boItem;

        public float count;

        public bool CanRecycle { get; set; }

        // 건물 밑에 컬라이더 체크용 평면 바닥들 리스트
        public List<ColliderCheckPlane> planes = new List<ColliderCheckPlane>();

        public void Init()
        {
            boItem = new BoBuildItem(GameManager.SD.sdBuildItems.Find(_ => _.name == gameObject.name));
        }

        /// <summary>
        /// 바닥플레인들을 리스트에 담는 함수
        /// </summary>
        public void ReigistPlanes()
        {
            // 자식으로부터 collcheckplane을 받아 등록해준다
            planes = transform.GetComponentsInChildren<ColliderCheckPlane>().ToList();
        }

        public void Update()
        {
            foreach (var plane in planes)
                plane.OnUpdate();
        }

        /// <summary>
        /// 바닥 플레인들을 담은 리스트를 비워주는 함수
        /// </summary>
        public void ClearPlanes()
        {
            planes.Clear();
        }

        /// <summary>
        /// 평면바닥들 전부다 풀에 리턴하는 함수
        /// </summary>
        public void PlanesPoolReturn()
        {
            var pool = PoolManager.Instance.GetPool<ColliderCheckPlane>();

            foreach (var plane in planes)
            {
                pool.PoolReturn(plane);
            }
        }

        // 현재 건물이 건축할수 있는지 없는지 체크하는 함수
        public bool CheckBuildState()
        {
            // 플레인 리스트를 순회하는데 타일에 collider충돌이 있으면 바로 false를 리턴
            // 없으면 건물을 지을수 있다는 것임으로 true리턴
            foreach (var plane in planes)
            {
                if (plane.canCurrentBuilding == false)
                    return false;
            }

            return true;
        }


        public void PoolInit()
        {
            Init();
        }
    }
}
