using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTabs : MonoBehaviour
{
    public int TabNumbers;
    public List<Button> TabButtons;
    public List<GameObject> TabContents;
    // Start is called before the first frame update
    void Start()
    {
        TabButtons = new List<Button>();
        RectTransform rectTransfrom = GetComponent<RectTransform>();
        float width = rectTransfrom.rect.width;
        float height = rectTransfrom.rect.height;
        float tabWidth = width / TabNumbers;
        for (int i = 0; i < TabNumbers; i++)
        {
            GameObject go = Instantiate(Resources.Load("TabButton")) as GameObject;
            go.GetComponentInChildren<Text>().fontSize = 40;
            RectTransform buttonRect = go.GetComponent<RectTransform>();
            Button button = go.GetComponent<Button>();
            TabButtons.Add(button);
            buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tabWidth);
            buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            go.transform.parent = transform;

        }
        TabButtons[0].onClick.AddListener(()=>  {
            for (int i = 0; i < TabContents.Count; i++)
            {
                if (i == 0)
                    TabContents[i].SetActive(true);
                else
                    TabContents[i].SetActive(false);
            }
        });
        TabButtons[1].onClick.AddListener(() => {
            for (int i = 0; i < TabContents.Count; i++)
            {
                if (i == 1)
                    TabContents[i].SetActive(true);
                else
                    TabContents[i].SetActive(false);
            }
        });
    }

    public List<Button> GetTabButtons()
    {
        return TabButtons;
    }
}
