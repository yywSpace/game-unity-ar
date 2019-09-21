using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.MainPage
{
    
    /// <summary>
    ///  0 1 2
    /// 
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
       public Button button;
//        public Button buttonLock;
//        public Button buttonOperation;
        
        public Animator leftAnimator; // map
        public Animator rightAnimator; // operation
        public Animator timeAnimator;
        public Animator dateAnimator;
        public Animator platformAnimator;
        
        public GameObject role;
        public GameObject platform;
        private int _platformStatus;
        
        private void OnEnable()
        {
            // 默认为正常模式
            ChangeToNormalMode();
            _platformStatus = 0;
            leftAnimator.SetBool("slide", false);
            // 角色旋转
            role.GetComponent<RoleRotate>()
                .onSlide
                .AddListener((x, y) =>
                {
                    if (Mathf.Abs(x) > Mathf.Abs(y))// 水平
                        role.transform.Rotate(role.transform.up, -x, Space.World); 
                });
            // 平台旋转
            platform.GetComponent<PlatformRotate>()
                .onSlide
                .AddListener((x,y) =>
                {
                    print("slide");

                    if (x > 0)
                        platformAnimator.SetTrigger("status_"+(_platformStatus+1));
                    else //右
                        platformAnimator.SetTrigger("status_m"+(_platformStatus+1));

                    switch (_platformStatus)
                    {
                        case 0:
                            ChangeToNormalMode();
                            break;
                        case 1:
                            ChangeToScreenLockMode();
                            break;
                        case 2:
                            ChangeToOperationMode();
                            break;
                    }
                    _platformStatus++;
                    _platformStatus %= 3;
                });
        }

        private void Start()
        {
           button.onClick.AddListener(() =>
           {
               _platformStatus++;
               _platformStatus %= 3;
               platformAnimator.SetTrigger("status_"+(_platformStatus+1));
           });
//            buttonLock.onClick.AddListener(ChangeToScreenLockMode);
//            buttonOperation.onClick.AddListener(ChangeToOperationMode);

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
