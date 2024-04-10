using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class LineChartPlotter : MonoBehaviour
{
    public List<float> xArray;
    public List<float> yArray;
    public List<float> oldXArray;
    public List<float> oldYArray;
    private float time = 0;
    private int oldArraySize;
    private int arraySize;
    private int startPos = 0;
    private int minX = 0;
    private int maxX = 100;
    public List<string> xAxisLabels;

    private bool changeMade;

    private void Start()
    {   
        // for (int i = minX; i<=maxX; i += (maxX-minX)/10) {
        //     xAxisLabels.Add(i.ToString());
        // }

        // xArray = DummyScript.xArray;
        // yArray = DummyScript.yArray;


        // oldXArray = new List<float>(xArray);
        // oldYArray = new List<float>(yArray);

        // arraySize = yArray.Count;

        // changeMade = false;

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
                chart.AddData(0, xArray[i],  yArray[i]);
            } 
        }
    }



    public void ChangeMade() {
        changeMade = true;
    }


    public void Update() {
        time += Time.deltaTime;
        if (time > 5 || changeMade == true) {
            
            changeMade = false;
            time = 0;

            oldArraySize = oldYArray.Count;
            arraySize = yArray.Count;

            oldXArray = new List<float>(xArray);
            oldYArray = new List<float>(yArray);
            
            var chart = gameObject.transform.GetChild(0).gameObject.GetComponent<ScatterChart>();
            // // Clear default data and add Line type and Serie for receiving data
            // chart.RemoveData();
            // chart.AddSerie<Line>("line");  



            // // Add data:
            // for (int i = oldArraySize; i < arraySize; i++)
            // for (int i = 0;i<10;i++) {
            //     chart.AddXAxisData(xAxisLabels[i]);
            // }
            // {
            //     chart.AddData(0, xArray[i],  yArray[i]);
            // }

            
            // Replace data:

            chart.RemoveData();
            chart.AddSerie<Scatter>("scatter");
            chart.series[0].symbol.size = 8f;

            startPos += arraySize - oldArraySize;

            for (int i = 0;i<10;i++) {
                chart.AddXAxisData(xAxisLabels[i]);
            }

            for (int i = startPos; i < arraySize; i++) {
                chart.AddData(0, xArray[i],  yArray[i]);
            }
        }
    }
}  
    
