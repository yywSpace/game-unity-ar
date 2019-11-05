using UnityEngine;
using UnityEngine.UI;

namespace Script.MainPage.UserManagement
{
    public class CharacterInfoController : MonoBehaviour
    {
        public Image userAvatarImage;
        public Text userNameText;
        public Slider userHpSlider;
        public Slider userExpSlider;
        public Text userLevelText;

        public void UpdateUserMessage(User user)
        {
            if (user.AvatarBytes != null)
            {
                Texture2D tx = new Texture2D(260, 260);//512
                tx.LoadImage(user.AvatarBytes);
                userAvatarImage.sprite = Sprite.Create(tx, new Rect(0, 0, 260, 260), new Vector2(0.5f, 0.5f));
            }
            
            userNameText.text = user.UserName;
            userHpSlider.maxValue = user.MaxHp;
            userHpSlider.value = user.Hp;
            userLevelText.text = "Lv." + user.Level;
            userExpSlider.maxValue = (user.Level + 1) * 10;
            userExpSlider.value = user.Exp;
        }
    }
}
