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


        public Animator leftAnimator; // map
        public Animator rightAnimator; // operation
        public Animator timeAnimator;
        public Animator dateAnimator;
        public Animator platformAnimator;

        public GameObject role;
        public GameObject platform;
        public Camera cam;
        
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
                    if (Mathf.Abs(x) > Mathf.Abs(y)) // 水平
                        role.transform.Rotate(role.transform.up, (float)(x*1.5), Space.World);
                });
            // 平台旋转
            platform.GetComponent<PlatformRotate>()
                .onSlide
                .AddListener((x, y) =>
                {
                    // 颜色代表当前状态，yellow:0,red:1,green:2
                    // 如果同向滑动则相应状态+1或-1表示同一方向状态切换
                    // 如果不同方向滑动则播放当前状态的反向动画，并对状态进行+1或-1处理
                    // 关键是在滑动后保证模型状态和_platformStatus对应
                    if (x > 0)
                    {
                        platformAnimator.SetTrigger("status_" + _platformStatus);
                        _platformStatus++;
                        _platformStatus %= 3;
                    }
                    else
                    {
                        platformAnimator.SetTrigger("status_m" + _platformStatus);
                        _platformStatus--;
                        _platformStatus = _platformStatus < 0 ? 2 : _platformStatus;
                    }
                    // 切换状态
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
                });
        }

        void ChangeToOperationMode()
        {
            // 如果脚本已经激活且动画处于show
            if (timeAnimator.enabled && dateAnimator.enabled && timeAnimator.GetBool("showTime") &&
                dateAnimator.GetBool("showDate"))
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
            if (leftAnimator.enabled && rightAnimator.enabled && leftAnimator.GetBool("showLeft") &&
                rightAnimator.GetBool("showRight"))
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
                timeAnimator.SetBool("showTime", true);
                dateAnimator.SetBool("showDate", true);
            }
        }

        void ChangeToNormalMode()
        {
            // 如果处于播放滑动动画状态则设置滑动状态为false播放隐藏动画，并设置showLeft为false防止播放展示动画
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
