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

        public UIStore uiStore;

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
            uiStore ??= UIManager.Instance.GetUI<UIStore>();
        }

        public void OnUpdate()
        {
            CheckInteraction();
        }

        //현재 유저와 상호작용인지 체크하는함수
        private void CheckInteraction()
        {
            var hits = Physics.OverlapBox(transform.position, coll.bounds.extents, Quaternion.identity, 1 << LayerMask.NameToLayer("Player"));

            // 여기에 들어왔다는것은 유저가 store 상호작용 가능 범위를 벗어났거나
            // 들어오지 않았다는것
            if (hits.Length < 1)
            {
                boStore.interaction = false;
                uiStore?.Close();
            }
            // 위조건이 아니라면 창을 열어준다
            else
            {
               OnStore();
            }
        }

        /// <summary>
        /// 상점 열었을때 UI정보 초기화해주는 함수
        /// </summary>
        public void OnStore()
        {
            //npc가 유저와 상호작용중이 아니며 E키를 눌렀을때
            if ((!boStore.interaction) && Input.GetKeyDown(KeyCode.E))
            {
                uiStore ??= UIManager.Instance.GetUI<UIStore>();

                uiStore.Initialize(boStore);

                boStore.interaction = true;

                uiStore.Open();
            }
        }
    }
}
