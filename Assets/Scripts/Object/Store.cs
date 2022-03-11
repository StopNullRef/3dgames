using Project.DB;
using Project.UI;
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

        //현재 유저와 상호작용인지 체크하는함수
        private void CheckInteraction()
        {
            var hits = Physics.OverlapBox(transform.position, coll.bounds.extents / 2, Quaternion.identity,1<<LayerMask.NameToLayer("Player"));
            //TODO 0312 여기서부터 구현하기


        }

        /// <summary>
        /// 상점 열었을때 UI정보 초기화해주는 함수
        /// </summary>
        public void OnStore()
        {
            var storeUI = UIManager.Instance.GetUI<UIStore>();

            storeUI.Initialize(this);
        }
    }
}
