using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

// todo 修改chart数据
public class SummarisePanelEvent : MonoBehaviour
{
    public Text SummaryTitle;
    public Text Desc1, Desc2;
    public RadarChart Chart;
    public Dropdown TimeDropdown;
    private string selectedType;
    private int completeMissionCount;
    private int releasedMissionCount;
    private int phoneUseTime;
    private int excellentScoreCount;
   
    // Start is called before the first frame update
    void Start()
    {
        TimeDropdown.onValueChanged.AddListener((int i) =>
        {
            print(i);
            switch (i)
            {
                case 0:
                    selectedType = "Week";
                    completeMissionCount = 10;
                    releasedMissionCount = 10;
                    phoneUseTime = 10;
                    excellentScoreCount = 10;
                    break;
                case 1:
                    selectedType = "Month";
                    completeMissionCount = 100;
                    releasedMissionCount = 100;
                    phoneUseTime = 100;
                    excellentScoreCount = 100;
                    break; ;
                case 2:
                    selectedType = "Year";
                    completeMissionCount = 1000;
                    releasedMissionCount = 1000;
                    phoneUseTime = 1000;
                    excellentScoreCount = 1000;
                    break;// 
            }
            Chart.title.text = string.Format("{0} Summaries", selectedType);
            SummaryTitle.text = string.Format("{0} Summaries", selectedType);
            Desc1.text = string.Format("This {0}, you have got ", selectedType);
            Desc2.text = string.Format(
                "{0} mission a completed \n" +
                "{1} mission released \n" +
                "{2} min Phone  use \n" +
                "{3} excellent score",
                completeMissionCount, releasedMissionCount, phoneUseTime, excellentScoreCount);
        });
    }
}
