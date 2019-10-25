using Script.Client.clients;
using Script.Event;
using Script.MainPage.Social;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace Script.MainPage
{
    /// <summary>
    /// 通过获取到的通信人的姓名，进行同话
    /// </summary>
    public class ChatRoomController : MonoBehaviour
    {
        public GameObject chatPanel;
        public InputField chatInput;
        public Text chatText;
        public ScrollRect scrollRect;
        public Button submitButton;
        public GameObject quietImage;
        public Button friendBtn;
        public Button squareBtn;
        public GameObject chatBubblePrefab;
        public GameObject role;

        private GameClient _gameClient;
        public GameClientController clientController;
        /// <summary>
        /// 通信人姓名，通过聊天人列表传入
        /// </summary>
        public string otherName;
        string username = "USERNAME";
        // Use this for initialization
        void Start()
        {
            _gameClient = ClientLab.GetGameClient();
            
            chatPanel.SetActive(false);
            
            friendBtn.onClick.AddListener(() =>
            {
                chatPanel.SetActive(true);
            });
            squareBtn.onClick.AddListener(()=>
            {
                print(ArUtils.GetObjectSizeByCollider(role));
                Vector3 chatBubblePosition =
                    Camera.main
                        .WorldToScreenPoint(
                            role.transform.position + 
                            new Vector3(
                                0,
                                ArUtils.GetObjectSizeByCollider(role).y/2,
                                0)) + 
                    new Vector3(0,100,0);
                var chatBubble = Instantiate(chatBubblePrefab, chatBubblePosition, Quaternion.Euler(new Vector3())) as GameObject;
                chatBubble.transform.SetParent(GameObject.Find("Canvas").transform);
                BubbleController bubbleController =   chatBubble.GetComponent<BubbleController>();
                bubbleController.SetMessage("hello\nhello\bhello");
                bubbleController.SetTimeDelay(1);
            });
            quietImage.GetComponent<PointerClickEventTrigger>()
                .onPointerClick
                .AddListener(() =>
                {
                    chatPanel.SetActive(false);
                });
            
            submitButton.onClick.AddListener(() =>
            {
                if (chatInput.text != "")
                {
                    _gameClient.ChatToUser(otherName,chatInput.text);
                }
            });       
        }
     
        // Update is called once per frame
        void Update()
        {
            if (clientController.hasReceiveMessage)
            {
                //找到你要聊天的对象，并将你们的对话打印
                string message = "";
                Friend friend =  clientController.friends.Find(f =>  f.UserName == otherName);
                foreach (var userName in friend.MessageList.Keys)
                {
                    message += username + ":" + friend.MessageList[userName] + "\n";
                }
                ChangeText(message);
                clientController.hasReceiveMessage = false;
            }
        }
        private void ChangeText(string message)
        {
            chatText.text = message;
            chatInput.text = "";
            Canvas.ForceUpdateCanvases();       //关键代码
            scrollRect.verticalNormalizedPosition = 0f;  //关键代码
            Canvas.ForceUpdateCanvases();   //关键代码
        }
    }
}
