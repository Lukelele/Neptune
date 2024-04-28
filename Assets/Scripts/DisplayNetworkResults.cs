using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class DisplayNetworkResults : MonoBehaviour
{
    private int arraySize = 0;
    private int minX = 0;
    private int maxX = 100;

    public string XName;
    public string YName;

    float time = 0;
    float updateInterval = 1;

    [SerializeField] public List<float> xArray = new List<float>();
    [SerializeField] public List<float> yArray = new List<float>();

    private LineChart chart;


    private void Start() {
        // https://www.youtube.com/watch?v=2pCkInvkwZ0

        chart = this.GetComponentInChildren<LineChart>();
        arraySize = xArray.Count;
        makeChart();
        plotLine();
    }


    private void Update() {
        // if (resultsValues.changeMade == true) {
        //     displayGraphUpdate();
        //     resultsValues.changeMade = false;
        // }

        time += Time.deltaTime;
        if (time > updateInterval) {
            time = 0;
            gatherData();

            displayGraphUpdate();
        }
    }


    private void displayGraphUpdate() {
        if (chart == null)  {
            Debug.LogError("Missing Chart");
            return;
        } 

        arraySize = xArray.Count;
        if (arraySize >= 1) {
            chart.series[0].AddData(xArray[arraySize-1],  yArray[arraySize-1]);
        }
    }



    public void makeChart() {
        if (transform.childCount > 0) {
            if (chart == null)  {
                chart = gameObject.transform.GetChild(0).gameObject.AddComponent<LineChart>();
                chart.Init();
            } else {
                Debug.Log("Canvas already has scatter chart child");
            }

            // chart.SetSize(600, 600);

            // Set title
            var title = chart.EnsureChartComponent<Title>();
            title.text = "Population Over Time";

            // Set whether prompt boxes and legends are displayed
            var tooltip = chart.EnsureChartComponent<Tooltip>();
            tooltip.show = true;

            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = false;

            // Set axes
            var xAxis = chart.EnsureChartComponent<XAxis>();
            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;
            xAxis.type = Axis.AxisType.Value;

            var yAxis = chart.EnsureChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;

            // disable line symbol
            chart.series[0].symbol.show = false;

            // Clear default data
            chart.RemoveData();
        }
    }

    private void plotScatter() {
        chart.AddSerie<Line>("scatter");
        // chart.series[0].symbol.size = 8f;
        chart.series[0].animation.enable = true;
        chart.series[0].symbol.show = false;

        arraySize = xArray.Count;

        for (int i = 0; i < arraySize; i++) {
            chart.series[0].AddData(xArray[i],  yArray[i]);
        }

    }

    private void plotLine() {
        chart.AddSerie<Line>("line");
        // chart.series[0].symbol.size = 0f;
        chart.series[0].animation.enable = true;
        chart.series[0].symbol.show = false;

        arraySize = xArray.Count;

        for (int i = 0; i < arraySize; i++) {
            chart.series[0].AddData(xArray[i],  yArray[i]);
        }
    }

    private void AddData(float yData) {
        xArray.Add(xArray.Count * updateInterval);
        yArray.Add(yData);
    }

    private void gatherData() {
        if (YName == "Food") {
            int foodCount = getFoodCount();
            AddData(foodCount);
        }
        else if (YName == "Shark") {
            int predatorCount = getPredatorCount();
            AddData(predatorCount);
        }
        else if (YName == "Flock") {
            int flockCount = getFlockCount();
            AddData(flockCount);
        }
        else if (YName == "Fish") {
            int preyCount = getPreyCount();
            AddData(preyCount);
        }
    }

    private void clearData() {
        xArray = new List<float>();
        yArray = new List<float>();

        arraySize = 0;

        chart.RemoveData();
    }


    public int getFoodCount() {
        return GameObject.FindGameObjectsWithTag("Food").Length;
    }

    public int getPredatorCount() {
        return GameObject.FindGameObjectsWithTag("Shark").Length;
    }

    public int getFlockCount() {
        return GameObject.FindGameObjectsWithTag("Flock").Length;
    }

    public int getPreyCount() {
        return GameObject.FindGameObjectsWithTag("Fish").Length;
    }
    
    public void SetXAxisName(int nameId) {
        chart.series[0].data.Clear();
        xArray = new List<float>();
        yArray = new List<float>();
        XName = NameIdToName(nameId);
    }
    
    public void SetYAxisName(int nameId) {
        chart.series[0].data.Clear();
        xArray = new List<float>();
        yArray = new List<float>();
        YName = NameIdToName(nameId);
    }
    
    public string NameIdToName(int nameId) {
        switch (nameId) {
            case 0:
                return "Food";
            case 1:
                return "Shark";
            case 2:
                return "Flock";
            case 3:
                return "Fish";
            default:
                return "";
        }
    }

    // private int getFlockUnitAverageAge() {
    //     Flock flocks[] = GameObject.FindGameObjectsWithTag("Flock").GetComponent<Flock>();
    //     int totalAge = 0;

    //     for (int i = 0; i < flocks.Length; i++) {
    //         totalAge += flocks[i].getAverageAge();
    //     }
    // }

    // public float getFlockDiversity() {
    //     GameObject[] flocks = GameObject.FindGameObjectsWithTag("Flock");
    //     float diversity = 0;
    //     for (int layer = 1; layer < flocks[0].GetComponent<Flock>().brain.layers.Length; layer++) {
    //         for (int i = 0; i < flocks[0].GetComponent<Flock>().brain.layers[layer].size; i++) {
    //             for (int j = 0; j < flocks[0].GetComponent<Flock>().brain.layers[layer+1].size; j++) {
    //                 for (int f = 0; f < flocks.Length; f++) {
    //                     for (int g = 0; g < flocks.Length; g++ ) {
    //                         diversity += Mathf.Abs(flocks[f].GetComponent<Flock>().brain.layers[layer].weights[i, j] - flocks[g].GetComponent<Flock>().brain.layers[layer].weights[i, j]);
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     return diversity;
    // }

}


// Stat Tracker



