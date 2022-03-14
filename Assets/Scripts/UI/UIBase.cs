using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    /// <summary>
    /// ��� UI���� UI���� base
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
        /// �ش�UI�� �����ִ��� �����ִ��� �Ǻ��ϴ� ��Ÿ�� ����
        /// </summary>
        public bool isOpen;

        /// <summary>
        /// escŰ�� uiâ�� �ݴ� ���� �������� üũ�ϴ� ��Ÿ�� ����
        /// </summary>
        public bool isEscClose;

        public Define.UIType type = Define.UIType.None;

        public virtual void Start()
        {
            UIInit();
        }

        

        /// <summary>
        /// UI�� �ʱ�ȭ�ϴ� �Լ�
        /// </summary>
        public virtual void UIInit()
        {
            UIManager.Instance.RegistUI(this);


            if (isOpen)
                Open(true);
            else
                Close(true);
        }

        /// <summary>
        /// �Ŵ������� ���� ������Ʈ��
        /// </summary>
        public virtual void OnUpate()
        {

        }

        /// <summary>
        /// �ش� UI���� �Լ�
        /// </summary>
        /// <param name="initialValue">�ʱ�ȭ �Ҷ� ó���� �����־�ߵǴ���?</param>
        public virtual void Open(bool initialValue = false)
        {
            // UIManager�� ����� �����ֱ�
            if(!isOpen || initialValue)
            {
                isOpen = true;
                UIManager.Instance.RegistOpenUI(this);
                SetCanvasGroup(true);
            }
        }

        /// <summary>
        /// UIâ �ݴ� �Լ�
        /// </summary>
        /// <param name="intialValue">�ʱ⿡ �ݾƾߵǴ���?</param>
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
        /// ĵ���� �׷쳻 ��ҵ��� �������ִ� �Լ�
        /// </summary>
        /// <param name="isActive"></param>
        protected void SetCanvasGroup(bool isActive)
        {
            Group.alpha = Convert.ToInt32(isActive);

            Group.blocksRaycasts = isActive;

            Group.interactable = isActive;
        }
    }
}
