using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.MainPage
{
    public class UiController : MonoBehaviour
    {
        public GameObject mapPanel;
        public Button buttonFriend;
        public Button buttonTask;
        public Button buttonCourse;
        public Button buttonFeedback;
        public Button buttonScan;
        public Button buttonPut;

        // Start is called before the first frame update
        void Start()
        {
            mapPanel.GetComponent<OnMapButtonDrag>().dragEndEvent.AddListener(() =>
            {
                SceneManager.LoadScene("HugeMapScene");
            });
            buttonFriend.onClick.AddListener(StartFriendActivity);
            buttonTask.onClick.AddListener(StartTaskActivity);
            buttonCourse.onClick.AddListener(StartCourseActivity);
            buttonFeedback.onClick.AddListener(StartFeedbackActivity);
            buttonScan.onClick.AddListener(() => { SceneManager.LoadScene("ModelScanScene"); });
            buttonPut.onClick.AddListener(() => { SceneManager.LoadScene("ModelPutScene"); });
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



        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
