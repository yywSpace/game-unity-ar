using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.MainPage
{
    public class AnimationController : MonoBehaviour
    {
        public Button button;
        public Button buttonLock;
        public Button buttonOperation;
        
        public Animator leftAnimator; // map
        public Animator rightAnimator; // operation
        public Animator timeAnimator;
        public Animator dateAnimator;

        private void OnEnable()
        {
            leftAnimator.SetBool("slide", false);

        }

        private void Start()
        {
            button.onClick.AddListener(ChangeToNormalMode);
            buttonLock.onClick.AddListener(ChangeToScreenLockMode);
            buttonOperation.onClick.AddListener(ChangeToOperationMode);

        }

        void ChangeToOperationMode()
        {
            // 如果脚本已经激活且动画处于show
            if (timeAnimator.enabled && dateAnimator.enabled && timeAnimator.GetBool("showTime") && dateAnimator.GetBool("showDate"))
            {
                timeAnimator.SetBool("showTime", false);
                dateAnimator.SetBool("showDate", false);
            }
            // 如果脚本未激活则激活脚本（此时会播放第一遍动画）
            if (leftAnimator.enabled != true && rightAnimator.enabled != true)
            {
                leftAnimator.enabled = true;
                rightAnimator.enabled = true;
            }
            else
            {
                leftAnimator.SetBool("showLeft", true);
                rightAnimator.SetBool("showRight", true);
            }
        }

        void ChangeToScreenLockMode()
        {
            // 如果脚本已经激活且动画处于show
            if (leftAnimator.enabled && rightAnimator.enabled && leftAnimator.GetBool("showLeft") &&rightAnimator.GetBool("showRight"))
            {
                if (leftAnimator.GetBool("slide"))
                    leftAnimator.SetBool("slide", false);
                leftAnimator.SetBool("showLeft", false);
                rightAnimator.SetBool("showRight", false);
            }

            // 如果脚本未激活则激活脚本（此时会播放第一遍动画）
            if (timeAnimator.enabled != true && dateAnimator.enabled != true)
            {
                timeAnimator.enabled = true;
                dateAnimator.enabled = true;
            }
            // 否则播放动画
            else
            {
                timeAnimator.SetBool("showTime",true);
                dateAnimator.SetBool("showDate",true);
            }
        }

        void ChangeToNormalMode()
        {
            if (leftAnimator.enabled)
            {
                if (leftAnimator.GetBool("slide"))
                    leftAnimator.SetBool("slide", false);
                leftAnimator.SetBool("showLeft", false);
                
            }
            if (rightAnimator.enabled)
                rightAnimator.SetBool("showRight", false);
            if (timeAnimator.enabled)
                timeAnimator.SetBool("showTime", false);
            if (dateAnimator.enabled)
                dateAnimator.SetBool("showDate", false);
        }
    }
}
