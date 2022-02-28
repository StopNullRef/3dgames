using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    /// <summary>
    /// 모든 UI들의 UI들의 base
    /// </summary>
    public class UIBase : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        public CanvasGroup Group 
        {
            get 
            { 
                if(canvasGroup == null)
                {
                    canvasGroup = GetComponent<CanvasGroup>();
                }
                return canvasGroup;
            } 
        }

        /// <summary>
        /// 해당UI가 열려있는지 닫혀있는지 판별하는 불타입 변수
        /// </summary>
        public bool isOpen;

        /// <summary>
        /// 업데이트를 돌려줄지아닌지 체크하는 변수
        /// </summary>
        public bool isUpate;

        public Define.UIType type = Define.UIType.None;

        public virtual void Start()
        {
            UIInit();
        }

        

        /// <summary>
        /// UI를 초기화하는 함수
        /// </summary>
        public virtual void UIInit()
        {
            UIManager.Instance.RegistUI(this);

            if (isUpate)
                UIManager.Instance.RigistUpdate(this);

            if (isOpen)
                Open(true);
            else
                Close(true);
        }

        /// <summary>
        /// 매니저에서 돌릴 업데이트문
        /// </summary>
        public virtual void OnUpate()
        {

        }

        /// <summary>
        /// 해당 UI여는 함수
        /// </summary>
        /// <param name="initialValue">초기화 할때 처음에 열려있어야되는지?</param>
        public virtual void Open(bool initialValue = false)
        {
            // UIManager에 등록후 열어주기
            if(!isOpen || initialValue)
            {
                isOpen = true;
                UIManager.Instance.RegistOpenUI(this);
                SetCanvasGroup(true);
            }
        }

        /// <summary>
        /// UI창 닫는 함수
        /// </summary>
        /// <param name="intialValue">초기에 닫아야되는지?</param>
        public virtual void Close(bool intialValue =false)
        {
            if(isOpen || intialValue)
            {
                isOpen = false;
                UIManager.Instance.RemoveOpenUI(this);
                SetCanvasGroup(false);
            }
        }

        /// <summary>
        /// 캔버스 그룹내 요소들을 설정해주는 함수
        /// </summary>
        /// <param name="isActive"></param>
        private void SetCanvasGroup(bool isActive)
        {
            Group.alpha = Convert.ToInt32(isActive);

            Group.blocksRaycasts = isActive;

            Group.interactable = isActive;
        }
    }
}
