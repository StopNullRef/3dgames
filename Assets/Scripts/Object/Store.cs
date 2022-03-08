using Project.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Object
{
    /// <summary>
    /// 상점 클래스
    /// </summary>
    public class Store : MonoBehaviour
    {
        public BoStore boStore;

        private Collider coll;

        /// <summary>
        /// 유저와 상호작용인지 체크하는 불타입 변수
        /// </summary>
        public bool isInteraction;

        public void Initialize(BoStore boStore)
        {
            this.boStore = boStore;

            var stagePos = boStore.sdStore.stagePos;

            transform.position = new Vector3(stagePos[0], stagePos[1], stagePos[2]);
            transform.eulerAngles = new Vector3(stagePos[3], stagePos[4], stagePos[5]);

            coll ??= GetComponent<Collider>();
        }

        //TODO 03/08여기서 부터 구현하기
        public void OnStore()
        {

        }
    }
}
