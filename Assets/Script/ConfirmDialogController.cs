using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class ConfirmDialogController : MonoBehaviour
    {
        public Button confirmButton;
        public Button cancelButton;

        // Start is called before the first frame update
        void Start()
        {
            confirmButton.onClick.AddListener(() =>
            {
                print("接受任务成功");
                gameObject.SetActive(false);
            });
            cancelButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
