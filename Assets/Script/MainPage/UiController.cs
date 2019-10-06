using Script.Event;
using Script.PutModelScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.MainPage
{
    public class UiController : MonoBehaviour
    {
        public GameObject role;
        public GameObject mapPanel;
        
        public GameObject characterInfo;
        public GameObject characterDetailInfo;
        public GameObject moreInfo;
        
        public Button buttonFriend;
        public Button buttonTask;
        public Button buttonCourse;
        public Button buttonPut;
        
        void Start()
        {
            moreInfo.GetComponent<PointerClickEventTrigger>()
                .onPointerClick
                .AddListener((() =>
                {
                    StartFeedbackActivity();
                    print("more information");
                }));
            
            characterDetailInfo.SetActive(false);
            LongPressEventTrigger characterInfoLongPress = characterInfo.GetComponent<LongPressEventTrigger>();
            characterInfoLongPress.onLongPress.AddListener(() =>
            {
                characterDetailInfo.SetActive(!characterDetailInfo.activeSelf);
            });
            
            DoubleClickEventTrigger roleDoubleClick = role.GetComponent<DoubleClickEventTrigger>();
            roleDoubleClick.SetCamera(Camera.main);
            roleDoubleClick
                .onDoubleClick
                .AddListener(() =>
                {
                    if (!ArUtils.IsPointerOverUiObject())
                        SceneManager.LoadScene("ModelScanScene");
                });
            mapPanel.GetComponent<OnMapButtonDrag>().dragEndEvent.AddListener(() =>
            {
                SceneManager.LoadScene("HugeMapScene");
            });
            buttonFriend.onClick.AddListener(StartFriendActivity);
            buttonTask.onClick.AddListener(StartTaskActivity);
            buttonCourse.onClick.AddListener(StartCourseActivity);
            buttonPut.onClick.AddListener(() => { SceneManager.LoadScene("ModelPutScene"); });
        }

        
        // Update is called once per frame
        void Update()
        {
            // 如果控制状态详细信息面板已经显示，在点击其他地方后让其消失
            if (Input.GetMouseButtonDown(0) && characterDetailInfo.activeSelf && !ArUtils.IsPointerOverUiObject())
            {
                characterDetailInfo.SetActive(false);
            }
            
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        
        
        void StartFriendActivity()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("startFriendActivity");
        }

        void StartTaskActivity()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("startTaskActivity");
        }

        void StartCourseActivity()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("startCourseActivity");
        }

        void StartFeedbackActivity()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("startFeedbackActivity");
        }
    }
}
