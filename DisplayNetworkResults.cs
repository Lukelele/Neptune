using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class DisplayNetworkResults : MonoBehaviour
{
    // [SerializeField]
    private DummyScript resultsValues;
    public GameObject otherObjectSpawner;
    public List<string> xAxisLabels;
    private int arraySize;
    private void Start() {
        resultsValues = otherObjectSpawner.GetComponentInChildren<DummyScript>();
        Debug.Log(resultsValues);
        // https://www.youtube.com/watch?v=2pCkInvkwZ0
        displayGeneration();
        
    }

    private void Update() {
        if (resultsValues.changeMade == true) {
            Debug.Log(resultsValues.changeMade);
            resultsValues.changeMade = false;
        }        
    }

    private void displayGeneration() {
        Debug.Log(resultsValues.generation);
    }

    private void plotScatter() {

        if (transform.childCount > 0) {
            // var chart = gameObject.transform.GetChild(0).gameObject.GetComponent<LineChart>();
            var chart = gameObject.transform.GetChild(0).gameObject.GetComponent<ScatterChart>();

            if (chart == null)  {
                chart = gameObject.transform.GetChild(0).gameObject.AddComponent<ScatterChart>();
                chart.Init();
            } else {
                Debug.Log("Canvas already has graph child");
            }

            chart.SetSize(800, 600);

            // Set title
            var title = chart.EnsureChartComponent<Title>();
            title.text = "Simple Scatter";     

            // Set whether prompt boxes and legends are displayed
            var tooltip = chart.EnsureChartComponent<Tooltip>();
            tooltip.show = true;

            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = false;

            // Set axes
            var xAxis = chart.EnsureChartComponent<XAxis>();
            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;
            xAxis.type =  Axis.AxisType.Category;

            var yAxis = chart.EnsureChartComponent<YAxis>();
            yAxis.type =  Axis.AxisType.Value;

            // Clear default data and add Scatter type and Serie for receiving data
            chart.RemoveData();
            chart.AddSerie<Scatter>("scatter");  
            
            chart.series[0].symbol.size = 8f;
            // foreach (var serie in chart.series)
            // {
            //     serie.symbol.size = 8f;
            // }

            // Add initial data:
            for (int i = 0;i<10;i++) {
                chart.AddXAxisData(xAxisLabels[i]);
            }
            for (int i = 0; i < arraySize; i++) {
                chart.AddData(0, resultsValues.xArray[i],  resultsValues.yArray[i]);
            } 
        }
    }

}


