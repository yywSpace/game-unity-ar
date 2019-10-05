using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class InteractionWithAndroid : MonoBehaviour
    {
        public void LoadModelScanScene()
        {
            SceneManager.LoadScene("ModelScanScene");
        }

        public void LoadModelPutScene()
        {
            SceneManager.LoadScene("ModelPutScene");
        }  
        public void LoadMainPageScene()
        {
            SceneManager.LoadScene("MainPage");
        }
    }
}
