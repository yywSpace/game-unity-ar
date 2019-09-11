using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 模型上方对话气泡，实现的不好
/// </summary>
[Obsolete]
public class MessageBubblesShow : MonoBehaviour
{
    public GameObject MessageBubbles;
    public string taskDesc;
    public Camera cam;
    private Button previousSentence;
    private Button nextSentence;
    private Text showText;
    private int index;
    private string[] words;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Object messageBubblesModel = ArUtils.LoadModel("Model/MessageBubbles");
        var messageBubbles = Instantiate(messageBubblesModel, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
        MessageBubbles = messageBubbles;
        
        MessageBubbles.transform.parent = GameObject.Find("Canvas").transform;
        //MessageBubbles.GetComponent<RectTransform>()
            //.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ARUtils.GetObjectSize(gameObject).x*100); ;
        print(" ARUtils.GetObjectSize(gameObject).x:" + ArUtils.GetObjectSize(gameObject).x);
        words = SplitSentence(taskDesc);
        MessageBubbles.SetActive(false);
        Button[] buttons = MessageBubbles.GetComponentsInChildren<Button>();
        previousSentence = buttons[0];
        nextSentence = buttons[1];
        showText = MessageBubbles.GetComponentInChildren<Text>();
        previousSentence.onClick.AddListener(() =>
        {
            if (index <= 0)
                return;
            index--;
            showText.text = words[index];
        });

        nextSentence.onClick.AddListener(() =>
        {
            if (index >= words.Length - 1)
                return;
            index++;
            showText.text = words[index];
        });
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

    public GameObject GetMessageBubbles()
    {
        return MessageBubbles;
    }

    public void SetTaskDesc(string sentence)
    {
        taskDesc = sentence;
    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
    }
    public void SetMessageBubbles(GameObject messageBubbles)
    {
        MessageBubbles = messageBubbles;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < 0 && GetComponent<MeshRenderer>().isVisible == false)
        {
            MessageBubbles.SetActive(false);
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {

            // 如果点击物体
            if (Input.GetMouseButtonDown(0) && hitInfo.transform.name == transform.name)
            {
                MessageBubbles.SetActive(!MessageBubbles.activeSelf);
                showText.text = words[index];
            }
        }
        if (MessageBubbles.activeSelf)
        {
            float distance = Vector3.Distance(transform.position, cam.transform.position);
            print(distance);
            // 消息框随模型移动缩放
            float scale = 20 / distance;
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.position + new Vector3(0, ArUtils.GetObjectSize(gameObject).y, 0));
            MessageBubbles.transform.position = screenPoint;
            MessageBubbles.transform.localScale = Vector3.one * scale;
        }
    }
}
