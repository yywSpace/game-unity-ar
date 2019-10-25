using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.MainPage.Social
{
    public class BubbleController : MonoBehaviour
    {
        
        // 内部text控制文本
        public Text chatText;
        // 外部text控制大小，两者必须同步
        private Text _outsideText;
        private Animator _animator;
        
        /// <summary>
        /// 消失动画延迟播放的时间
        /// </summary>
        public float delayTime = 1;

        private float _time;

        private void OnEnable()
        {
            _outsideText = GetComponent<Text>();
            _animator = GetComponent<Animator>();
        }

        public void SetMessage(string message)
        {
            _outsideText.text = "\n"+message+"\n";
            chatText.text = "\n"+message+"\n";
        }

        public void SetTimeDelay(float time)
        {
            delayTime = time;
        }


        public void AnimationExit()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= delayTime)
            {
                print("_animator");
                _animator.SetBool("play", true);
            }
        }
    }
}
