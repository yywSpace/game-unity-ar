using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour, IPointerClickHandler
{
    public GameObject ConfirmDialog;
    public string taskDesc = "此任务不需要耗费很大的力气 只需要你在适当的时间内到达指定的地点 完成指定的任务就可以了";
    public Camera cam;
    private Text showText;
    public int index;
    private string[] words;

    void OnEnable()
    {
        if (showText != null)
        {
            showText.text = words[0];
        }
    }
    void Start()
    {
        words = SplitSentence(taskDesc);
        showText = gameObject.GetComponentInChildren<Text>();
        showText.text = words[0];
        print("starat");
     }

    public void SetTaskDesc(string sentence)
    {
        taskDesc = sentence;
    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
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
        if (index >= words.Length - 1)
        {
            ConfirmDialog.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
        index++;
        print(index);
        showText.text = words[index];
    }
    void OnDisable()
    {
        index = 0;
    }
}
