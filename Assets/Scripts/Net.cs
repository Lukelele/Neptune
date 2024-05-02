using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// The Net class represents a neural network.
/// </summary>
public class Net
{
    public Layer[] layers;

    /// <summary>
    /// Net constructor creates a neural network with the given structure.
    /// </summary>
    /// <param name="networkStructure"></param>
    public Net(int[] networkStructure) 
    {
        int[] structure = networkStructure;

        layers = new Layer[networkStructure.Length];
        layers[0] = new Layer(networkStructure[0], 0);
        for (int i = 1; i < networkStructure.Length; i++) {
            layers[i] = new Layer(networkStructure[i], networkStructure[i - 1]);
        }
    }
    
    /// <summary>
    /// Net constructor creates a neural network with the given layers.
    /// </summary>
    /// <param name="newLayers"></param>
    public Net(Layer[] newLayers) {
        layers = newLayers;
    }

    /// <summary>
    /// FeedForward method feeds forward the inputs to the neural network.
    /// </summary>
    /// <param name="inputs"></param>
    public void FeedForward(float[] inputs) {
        // Check if the input size matches the input layer size
        if (inputs.Length == layers[0].neurons.Length) {
            layers[0].neurons = inputs;
        }
        else {
            Debug.Log("Input sizes do not match in the input layer");
        }

        // Feed forward the inputs
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

    /// <summary>
    /// MutateNet method mutates the network by changing the value of the weights randomly.
    /// </summary>
    /// <param name="mutationProbability"></param>
    /// <param name="mutationMagnitude"></param>
    public void MutateNet(float mutationProbability, float mutationMagnitude) {
        for (int i = 1; i < layers.Length; i++) {
            layers[i].MutateLayer(mutationProbability, mutationMagnitude);
        }
    }
    
    /// <summary>
    /// CopyLayers method copies the layers of the network.
    /// </summary>
    /// <returns>the copied layers</returns>
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

    /// <summary>
    /// GetOutput method returns the output of the network.
    /// </summary>
    /// <returns>the output of the network</returns>
    public float[] GetOutput() {
        return layers[layers.Length - 1].neurons;
    }

    /// <summary>
    /// ReLU method returns the ReLU value of the input.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>the ReLU value of the input</returns>
    private float ReLU(float x) {
        return x > 0 ? x : 0;
    }

    /// <summary>
    /// Sigmoid method returns the sigmoid value of the input.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>the sigmoid value of the input</returns>
    private float Sigmoid(float x) {
        return 1 / (1 + Mathf.Exp(-x));
    }

    /// <summary>
    /// VisualiseNet method visualises the network.
    /// </summary>
    public void VisualiseNet() {
        NetVisualisation.net = this;
        NetVisualisation.isDirty = true;
        Debug.Log("Visualising net: " + NetVisualisation.net);
    }
}


/// <summary>
/// The Layer class represents a layer in a neural network.
/// </summary>
public class Layer
{
    public int size;
    public float[] neurons;
    public float bias;
    public float[,] weights;
    public float biasWeight;

    /// <summary>
    /// Layer constructor creates a layer with the given size and input size.
    /// </summary>
    /// <param name="layerSize"></param>
    /// <param name="inputSize"></param>
    /// <param name="biasValue"></param>
    public Layer(int layerSize, int inputSize, float biasValue = 1) {
        size = layerSize;
        neurons = new float[size];
        bias = biasValue;
        weights = new float[size, inputSize];

        // create random weights
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < inputSize; j++) {
                weights[i, j] = Random.Range(-1f, 1f);
            }
        }
        // biasWeight = Random.Range(-1f, 1f);
        biasWeight = 0;
    }
    
    /// <summary>
    /// MutateLayer method mutates the layer by changing the value of the weights randomly.
    /// </summary>
    /// <param name="mutationProbability"></param>
    /// <param name="mutationMagnitude"></param>
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