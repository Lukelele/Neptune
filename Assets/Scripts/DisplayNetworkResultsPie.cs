using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class DisplayNetworkResultsPie : MonoBehaviour
{
    private int arraySize = 0;
    private int minX = 0;
    private int maxX = 100;

    public string XName;
    public string YName;

    float time = 0;
    public float updateInterval = 1;

    [SerializeField] public List<float> xArray = new List<float>();
    [SerializeField] public List<float> yArray = new List<float>();


    private void Start() {
        // https://www.youtube.com/watch?v=2pCkInvkwZ0
        // returns array of children with DummyScript attatched

        // xArray.Add(1);
        // xArray.Add(2);
        // xArray.Add(3);

        // // add data to xArray and yArray
        // yArray.Add(0);
        // yArray.Add(0);
        // yArray.Add(0);

        arraySize = xArray.Count;
        makeChart();
        // plotScatter();
        plotPie();
    }


    private void Update() {
        // if (resultsValues.changeMade == true) {
        //     displayGeneration();
        //     displayGraphUpdate();
        //     resultsValues.changeMade = false;
        // }

        time += Time.deltaTime;
        if (time > updateInterval) {
            gatherData();

            arraySize = xArray.Count;

            var chart = this.GetComponentInChildren<PieChart>();

            chart.series[0].ClearData();
            for (int i = 0; i < arraySize; i++) {
                chart.series[0].AddData(yArray[i]);
            }
            time = 0;
        }
    }


    private void displayGeneration() {
        var chart = gameObject.transform.GetChild(0).gameObject.GetComponent<PieChart>();
        var title = chart.EnsureChartComponent<Title>();
        title.text = "Generation ";  
    }



    private void displayGraphUpdate() {
        var chart = this.GetComponentInChildren<PieChart>();
        
        if (chart == null)  {
            Debug.LogError("Missing Chart");
        } 

        arraySize = xArray.Count;
        // chart.series[0].AddData(xArray[arraySize-1],  yArray[arraySize-1]);
    }



    public void makeChart() {
        var chart = this.GetComponentInChildren<PieChart>();
        if (transform.childCount > 0) {
            if (chart == null)  {
                chart = gameObject.transform.GetChild(0).gameObject.AddComponent<PieChart>();
                chart.Init();
            } else {
                Debug.Log("Canvas already has scatter chart child");
            }

            // chart.SetSize(600, 600);

            // Set title
            var title = chart.EnsureChartComponent<Title>();
            title.text = "POPULATION";

            // Set whether prompt boxes and legends are displayed
            var tooltip = chart.EnsureChartComponent<Tooltip>();
            tooltip.show = true;

            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = false;

            // Set axes
            // var xAxis = chart.EnsureChartComponent<XAxis>();
            // xAxis.splitNumber = 10;
            // xAxis.boundaryGap = true;
            // xAxis.type =  Axis.AxisType.Value;

            // var yAxis = chart.EnsureChartComponent<YAxis>();
            // yAxis.type = Axis.AxisType.Value;

            // Clear default data
            chart.RemoveData();
        }
    }

    private void plotScatter() {
        var chart = this.GetComponentInChildren<ScatterChart>();

        chart.AddSerie<Pie>("scatter");
        // chart.series[0].symbol.size = 8f;
        chart.series[0].animation.enable = false;

        arraySize = xArray.Count;

        for (int i = 0; i < arraySize; i++) {
            chart.series[0].AddData(xArray[i],  yArray[i]);
        }

    }

    private void plotPie() {
        var chart = this.GetComponentInChildren<PieChart>();

        chart.AddSerie<Pie>("Pie");
        // chart.series[0].symbol.size = 0f;
        chart.series[0].animation.enable = true;

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
        int foodCount = getFoodCount();
        int flockCount = getFlockCount();
        int predatorCount = getPredatorCount();

        yArray[0] = foodCount;
        yArray[1] = flockCount;
        yArray[2] = predatorCount;
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

    public float getFoodBiomass() {
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
        float biomass = 0;
        for (int i = 0; i < food.Length; i++) {
            biomass += food[i].GetComponent<Seaweed>().energy;
        }
        return biomass;
    }

    public float getPredatorBiomass() {
        GameObject[] predators = GameObject.FindGameObjectsWithTag("Predator");
        float biomass = 0;
        for (int i = 0; i < predators.Length; i++) {
            biomass += predators[i].GetComponent<Shark>().energy;
        }
        return biomass;
    }

    public float getPreyBiomass() {
        GameObject[] prey = GameObject.FindGameObjectsWithTag("Prey");
        float biomass = 0;
        for (int i = 0; i < prey.Length; i++) {
            biomass += prey[i].GetComponent<FlockUnit>().energy;
        }
        return biomass;
    }
    
    // public void SetXAxisName(int nameId) {
    //     XName = NameIdToName(nameId);
    // }
    
    // public void SetYAxisName(int nameId) {
    //     YName = NameIdToName(nameId);
    // }
    
    // public string NameIdToName(int nameId) {
    //     switch (nameId) {
    //         case 0:
    //             return "Food";
    //         case 1:
    //             return "Shark";
    //         case 2:
    //             return "Flock";
    //         case 3:
    //             return "Fish";
    //         default:
    //             return "";
    //     }
    // }

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



