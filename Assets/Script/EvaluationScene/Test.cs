using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class Test : MonoBehaviour
{
    RadarChart chart;
    List<float> serie = new List<float> { 10, 20, 30, 40, 50 };
    void Start()
    {
        chart = gameObject.GetComponent<RadarChart>();
        chart.series.AddSerie("serie0", SerieType.Radar);
        chart.series.AddData("serie0", serie);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
