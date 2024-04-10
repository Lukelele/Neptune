using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getFoodCount() {
        return GameObject.FindGameObjectsWithTag("Food").Length;
    }

    public int getPredatorCount() {
        return GameObject.FindGameObjectsWithTag("Predator").Length;
    }

    public int getFlockCount() {
        return GameObject.FindGameObjectsWithTag("Flock").Length;
    }

    public int getPreyCount() {
        return GameObject.FindGameObjectsWithTag("Prey").Length;
    }

    public float getFlockDiversity() {
        GameObject[] flocks = GameObject.FindGameObjectsWithTag("Flock");
        float diversity = 0;
        for (int layer = 1; layer < flocks[0].GetComponent<Flock>().brain.layers.Length; layer++) {
            for (int i = 0; i < flocks[0].GetComponent<Flock>().brain.layers[layer].size; i++) {
                for (int j = 0; j < flocks[0].GetComponent<Flock>().brain.layers[layer+1].size; j++) {
                    for (int f = 0; f < flocks.Length; f++) {
                        for (int g = 0; g < flocks.Length; g++ ) {
                            diversity += Mathf.Abs(flocks[f].GetComponent<Flock>().brain.layers[layer].weights[i, j] - flocks[g].GetComponent<Flock>().brain.layers[layer].weights[i, j]);
                        }
                    }
                }
            }
        }

        return diversity;
    }

    public float getAverageAge() {
        GameObject[] flocks = GameObject.FindGameObjectsWithTag("Flock");
        float averageAge = 0;
        for (int i = 0; i < flocks.Length; i++) {
            averageAge += flocks[i].GetComponent<Flock>().age;
        }
        return averageAge / flocks.Length;
    }

}
