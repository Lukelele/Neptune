using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour
{
    [Header("Creature Settings")]
    [SerializeField] protected float moveSpeed = 3;

    [SerializeField] protected float energyConsume =  1;
    [SerializeField] protected float energyConsumeTime = 1;
    private float _energyConsumeTimer = 0;
    [SerializeField] public float energy = 10;
    [SerializeField] protected float energyForReproduction = 20;
    public int generation = 1;
    public float age;
    protected bool isAlive = true;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ManageEnergy();
    }
    
    protected void ManageEnergy()
    {
        if (!isAlive) return;
        _energyConsumeTimer -= Time.deltaTime;
        if (_energyConsumeTimer <= 0)
        {
            energy -= energyConsume;
            _energyConsumeTimer = energyConsumeTime;
        }
        
        float yPosition = transform.position.y;
        if (energy <= 0)
        {
            transform.Rotate(-90, 0, 0);
            Destroy(gameObject, 3);
            isAlive = false;
            
            string path = "Assets/LifeStats.txt";
            string content = generation + "," + age + "\n";
            if (!System.IO.File.Exists(path)) System.IO.File.WriteAllText(path, "");
            System.IO.File.AppendAllText(path, content);
        }

        if (energy >= energyForReproduction)
        {
            Reproduce();
        }
    }


    protected virtual void Reproduce() { }
    
    protected void Assess_IQ(float[] reactions, float[] expects)
    {
    }
}


public class Net
{
    public Layer[] layers;

    public Net(int[] networkStructure) 
    {
        int[] structure = networkStructure;

        layers = new Layer[networkStructure.Length];
        layers[0] = new Layer(networkStructure[0], 0);
        for (int i = 1; i < networkStructure.Length; i++) {
            layers[i] = new Layer(networkStructure[i], networkStructure[i - 1]);
        }
    }
    
    public Net(Layer[] newLayers) {
        layers = newLayers;
    }

    public void FeedForward(float[] inputs) {
        if (inputs.Length == layers[0].neurons.Length) {
            layers[0].neurons = inputs;
        }
        else {
            Debug.Log("Input sizes do not match in the input layer");
        }

        for (int i = 1; i < layers.Length; i++) {
            for (int j = 0; j < layers[i].size; j++) {
                float sum = 0;
                for (int k = 0; k < layers[i-1].size; k++) {
                    sum += layers[i-1].neurons[k] * layers[i].weights[j,k];
                }
                sum += layers[i-1].bias * layers[i].biasWeight;
                //layers[i].neurons[j] = Sigmoid(sum);
                layers[i].neurons[j] = sum;
            }
        }
    }

    public void MutateNet(float mutationProbability, float mutationMagnitude) {
        for (int i = 1; i < layers.Length; i++) {
            layers[i].MutateLayer(mutationProbability, mutationMagnitude);
        }
    }
    
    public Layer[] CopyLayers() {
        Layer[] newLayers = new Layer[layers.Length];
        for (int i = 0; i < layers.Length; i++) {
            newLayers[i] = new Layer(layers[i].size, layers[i].weights.GetLength(1))
            {
                neurons = layers[i].neurons,
                bias = layers[i].bias,
                weights = layers[i].weights,
                biasWeight = layers[i].biasWeight
            };
        }
        return newLayers;
    }

    public float[] GetOutput() {
        return layers[layers.Length - 1].neurons;
    }

    private float ReLU(float x) {
        return x > 0 ? x : 0;
    }

    private float Sigmoid(float x) {
        return 1 / (1 + Mathf.Exp(-x));
    }


    public void Save(string outputFile) {
        string content = "";
        // saving the network structure
        for (int i = 0; i < layers.Length; i++) {
            content += layers[i].size + ",";
        }

        // saving the network weights
        content = content.Substring(0, content.Length - 1);
        content += "\n";
        for (int i = 1; i < layers.Length; i++) {
            for (int j = 0; j < layers[i].size; j++) {
                for (int k = 0; k < layers[i].weights.GetLength(1); k++) {
                    content += layers[i].weights[j, k] + ",";
                }
                content = content.Substring(0, content.Length - 1);
                content += "\n";
            }
            for (int j = 0; j < layers[i].size; j++) {
                content += layers[i].bias + ",";
            }
            content = content.Substring(0, content.Length - 1);
            content += "\n";
            for (int j = 0; j < layers[i].size; j++) {
                content += layers[i].biasWeight + ",";
            }
            content = content.Substring(0, content.Length - 1);
            content += "\n";
        }
        System.IO.File.WriteAllText(outputFile, content);
    
    }

    public void Load(string filepath) {
        string[] content = System.IO.File.ReadAllLines(filepath);
        string[] structure = content[0].Split(',');
        int[] networkStructure = new int[structure.Length];
        for (int i = 0; i < structure.Length; i++) {
            networkStructure[i] = int.Parse(structure[i]);
        }
        layers = new Layer[networkStructure.Length];
        layers[0] = new Layer(networkStructure[0], 0);
        for (int i = 1; i < networkStructure.Length; i++) {
            layers[i] = new Layer(networkStructure[i], networkStructure[i - 1]);
        }
        int lineIndex = 1;
        for (int i = 1; i < layers.Length; i++) {
            for (int j = 0; j < layers[i].size; j++) {
                string[] weights = content[lineIndex].Split(',');
                for (int k = 0; k < layers[i].weights.GetLength(1); k++) {
                    layers[i].weights[j, k] = float.Parse(weights[k]);
                }
                lineIndex++;
            }
            string[] biases = content[lineIndex].Split(',');
            for (int j = 0; j < layers[i].size; j++) {
                layers[i].bias = float.Parse(biases[j]);
            }
            lineIndex++;
            string[] biasWeights = content[lineIndex].Split(',');
            for (int j = 0; j < layers[i].size; j++) {
                layers[i].biasWeight = float.Parse(biasWeights[j]);
            }
            lineIndex++;
        }
    
    }
}


public class Layer
{
    public int size;
    public float[] neurons;
    public float bias;
    public float[,] weights;
    public float biasWeight;

    public Layer(int layerSize, int inputSize, float biasValue = 1) {
        size = layerSize;
        neurons = new float[size];
        bias = biasValue;
        weights = new float[size, inputSize];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < inputSize; j++) {
                weights[i, j] = Random.Range(-1f, 1f);
            }
        }
        // biasWeight = Random.Range(-1f, 1f);
        biasWeight = 0;
    }
    
    public void MutateLayer(float mutationProbability, float mutationMagnitude) {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < weights.GetLength(1); j++) {
                if (Random.Range(0f, 1f) < mutationProbability) {
                    weights[i,j] += Random.Range(-mutationMagnitude, mutationMagnitude);
                }
            }
        }
        if (Random.Range(0f, 1f) < mutationProbability) {
            bias += Random.Range(-mutationMagnitude, mutationMagnitude);
        }
    }
}
