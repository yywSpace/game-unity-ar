using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.ARScene
{
    public class DialogueBoxController : MonoBehaviour, IPointerClickHandler
    {
        public GameObject confirmDialog;
        public string taskDesc = "此任务不需要耗费很大的力气 只需要你在适当的时间内到达指定的地点 完成指定的任务就可以了";
        private Text _showText;
        public int index;
        private string[] _words;

        void OnEnable()
        {
            if (_showText != null)
            {
                _showText.text = _words[0];
            }
        }
        void Start()
        {
            _words = SplitSentence(taskDesc);
            _showText = gameObject.GetComponentInChildren<Text>();
            _showText.text = _words[0];
            print("start");
        }

        public void SetTaskDesc(string sentence)
        {
            taskDesc = sentence;
        }
        

        /// <summary>
        /// 拆分文字，让其适应消息框的大小
        /// </summary>
        /// <returns>The sentence.</returns>
        /// <param name="sentence">Sentence.</param>
        string[] SplitSentence(string sentence)
        {
            if (sentence == null)
            {
                return "".Split(' ');
            }
            return sentence.Split(' ');
        }

        public void OnPointerClick(PointerEventData eventData)
        {
  
            if (index >= _words.Length - 1)
            {
                confirmDialog.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
            index++;
            print(index);
            _showText.text = _words[index];
        }
        void OnDisable()
        {
            index = 0;
        }

    }
}
