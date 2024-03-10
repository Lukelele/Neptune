using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    public Net net;

    public float food;
    

    // // Start is called before the first frame update
    // void Start()
    // {
    //     net = new Net(new int[] {3, 32, 3});
    //     food = 10;
    // }


    // // Update is called once per frame
    // void Update()
    // {
    //     float[] inputs = new float[] {0- transform.position.x, 0- transform.position.y, 0 - transform.position.z};
    //     Debug.Log("x: " + inputs[0] + " y: " + inputs[1] + " z: " + inputs[2]);
    //     net.FeedForward(inputs);
    //     float[] outputs = net.GetOutput();
    //     transform.position += new Vector3(outputs[0]-0.5f, outputs[1]-0.5f, outputs[2]-0.5f) * Time.deltaTime;

    //     manageFood(1f);
    // }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Food"))
    //     {
    //         food += 10;
    //         Agent newAgent = replicate();
    //         newAgent.net.Mutate(0.1f, 0.5f);
    //     }
    // }



    private void manageFood(float consumptionRate) {
        food -= Time.deltaTime * consumptionRate;
        checkAlive();
    }

    private void checkAlive() {
        if (food <= 0) {
            Destroy(gameObject);
        }
    }
    
    private Agent replicate() {
        GameObject newAgent = Instantiate(gameObject, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        newAgent.GetComponent<Agent>().net = net;

        return newAgent.GetComponent<Agent>();
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
            biasWeight = Random.Range(-1f, 1f);
        }
    }

    public class Net
    {
        public Layer[] layers;

        public Net(int[] networkStructure) {
            int[] structure = networkStructure;

            layers = new Layer[networkStructure.Length];
            layers[0] = new Layer(networkStructure[0], 0);
            for (int i = 1; i < networkStructure.Length; i++) {
                layers[i] = new Layer(networkStructure[i], networkStructure[i - 1]);
            }
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
                    layers[i].neurons[j] = Sigmoid(sum);
                }
            }
        }

        public void Mutate(float mutationProbability, float mutationMagnitude) {
            for (int i = 1; i < layers.Length; i++) {
                for (int j = 0; j < layers[i].size; j++) {
                    if (Random.Range(0f, 1f) < mutationProbability) {
                        for (int k = 0; k < layers[i-1].size; k++) {
                            layers[i].weights[j,k] += Random.Range(-mutationMagnitude, mutationMagnitude);
                        }
                    }
                }
            }
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
    }
