using UnityEngine;
using UnityEngine.UI;

namespace Script.MainPage.UserManagement
{
    public class CharacterDetailInfoController : MonoBehaviour
    {
        public Text userNameText;
        public Slider userHpSlider;
        public Slider userExpSlider;
        public Text userLevelText;
        public Text userReleaseNumText;
        public Text userAcceptNumText;
        public Text userAccomplishNumText;
        public Text userFriendNumText;
        
        public void UpdateUserDetailMessage(User user)
        {
            userNameText.text = user.UserName;
            userHpSlider.maxValue = user.MaxHp;
            userHpSlider.value = user.Hp;
            userLevelText.text = "Lv." + user.Level;
            userExpSlider.maxValue = (user.Level + 1) * 10;
            userExpSlider.value = user.Exp;
            userAcceptNumText.text = user.AccomplishTaskNum.ToString();
            userReleaseNumText.text = user.ReceiveTaskNum.ToString();
            userAccomplishNumText.text = user.AccomplishTaskNum.ToString();
        }
    }
}
