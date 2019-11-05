using System;
using Script.Event;
using Script.MainPage.UserManagement;
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

        public Button buttonTask;
        public Button buttonCourse;
        public Button buttonPut;

        private bool _hasQueryUser = false;
        private bool _hasGetUser;
        private string _userAccount = "1095204049";

        void Start()
        {
//            CallAndroidMethod.StartLoginActivity();

            moreInfo.GetComponent<PointerClickEventTrigger>()
                .onPointerClick
                .AddListener((() =>
                {
                    CallAndroidMethod.StartFeedbackActivity();
                    print("more information");
                }));

            characterDetailInfo.SetActive(false);
            LongPressEventTrigger characterInfoLongPress = characterInfo.GetComponent<LongPressEventTrigger>();
            characterInfoLongPress.onLongPress.AddListener(() =>
            {
                if (characterDetailInfo.activeSelf == false)
                {
                    characterDetailInfo.SetActive(true);
                    CharacterDetailInfoController infoController =
                        characterDetailInfo.GetComponent<CharacterDetailInfoController>();
                    infoController.UpdateUserDetailMessage(UserLab.CurrentUser);
                }
                else
                    characterDetailInfo.SetActive(false);
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
            mapPanel.GetComponent<OnMapButtonDrag>().dragEndEvent.AddListener(CallAndroidMethod.StartMapActivity);
            buttonTask.onClick.AddListener(CallAndroidMethod.StartTaskActivity);
            buttonCourse.onClick.AddListener(CallAndroidMethod.StartCourseActivity);
            buttonPut.onClick.AddListener(() => { SceneManager.LoadScene("ModelPutScene"); });
        }


        // Update is called once per frame
        void Update()
        {
            if (_userAccount != "" && !_hasQueryUser)
            {
                System.Threading.Tasks.Task
                    .Run(() =>
                    {
                        print(_userAccount);
//                            CallAndroidMethod.Toast(_userAccount);
                        UserLab.CurrentUser = UserLab.GetUser(_userAccount);
                        _hasGetUser = true;
                    });
                _hasQueryUser = true;
            }

            if (_hasGetUser)
            {
                print(UserLab.CurrentUser.UserName);
                CharacterInfoController cInfoController = characterInfo.GetComponent<CharacterInfoController>();
                cInfoController.UpdateUserMessage(UserLab.CurrentUser);
                _hasGetUser = false;
            }

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

        /// <summary>
        /// 由android方调用，获取当前登录者账户
        /// </summary>
        /// <param name="account"></param>
        public void GetUserAccount(string account)
        {
            _userAccount = account;
        }
    }
}
