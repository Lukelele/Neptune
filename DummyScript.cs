using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public float time = 0;
    public int generation = 0;
    public bool changeMade = false;
    [SerializeField] public List<float> xArray = new List<float>() {0,1,2,3,4,5,6,7,8,9};
    [SerializeField] public List<float> yArray = new List<float>() {0,1,4,9,16,25,36,49,64,81};

    public void Update() {
        time += Time.deltaTime;
        if (time > 5) {
            time = 0;
            generation++;
            // xArray = xArray.ConvertAll(x => x += 1);
            // yArray = yArray.ConvertAll(x => x += 10);
            xArray.Add(0);
            yArray.Add(1);
            changeMade = true;
        }
    }

    public DummyScript(int generation, bool changeMade, float time){
        this.generation = generation;
        this.changeMade = changeMade; 
        this.time = time;
    }
}