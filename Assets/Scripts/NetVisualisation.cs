using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NetVisualisation : MonoBehaviour
{
    [SerializeField] private GameObject a;
    public static Net net;

    public GameObject spherePrefab;
    public GameObject linePrefab;

    public float scale = 1f; // Scale factor for the neural network visualization
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> connections = new List<GameObject>();

    private bool isVisualized = false;

    void Start()
    {
        net = a.GetComponent<Shark>().HuntingSystem;
    }
    
    private void Update()
    {
        if (a != null && !isVisualized)
        {
            // Visualize the neural network of the selected entity
            VisualizeNetwork(net);

            isVisualized = true;
        }
        else if (a == null && isVisualized)
        {
            // Clear the visualization
            ClearVisualization();
            isVisualized = false;
            Debug.Log("Cleared visualization.");
        }

        // Rotate the network around its y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void VisualizeNetwork(Net net)
    {
        if (net != null)
        {
            // Accessing the neural network layers from the Net
            Layer[] layers = net.layers;
            int inputSize = layers[0].size;
            int[] hiddenSizes = new int[layers.Length - 2]; // Excluding input and output layers
            for (int i = 1; i < layers.Length - 1; i++)
            {
                hiddenSizes[i - 1] = layers[i].size;
            }
            int outputSize = layers[layers.Length - 1].size;

            // Position of the visualization
            Vector3 visualizationPosition = gameObject.transform.position - new Vector3((hiddenSizes.Length + 1) * 5 * scale / 2, 0, 0);

            // Visualize input layer
            CreateNodes(inputSize, visualizationPosition - new Vector3(0, layers[0].size * 2 * scale / 2, 0));

            // Visualize hidden layers
            int offset = inputSize;
            for (int i = 0; i < hiddenSizes.Length; i++)
            {
                CreateNodes(hiddenSizes[i], visualizationPosition + new Vector3((i + 1) * 5 * scale, 0, 0) - new Vector3(0, hiddenSizes[i] * 2 * scale / 2, 0));
                offset += hiddenSizes[i];
            }

            // Visualize output layer
            CreateNodes(outputSize, visualizationPosition + new Vector3((hiddenSizes.Length + 1) * 5 * scale, 0, 0) - new Vector3(0, layers[layers.Length - 1].size * 2 * scale / 2, 0));

            // Visualize connections
            VisualizeConnections(net, inputSize, hiddenSizes);
        }
        else
        {
            Debug.LogError("Net not found.");
        }
    }

    private void CreateNodes(int size, Vector3 position)
    {
        for (int i = 0; i < size; i++)
        {
            // create spheres make sure the pivot is in the center
            GameObject sphere = Instantiate(spherePrefab, position + new Vector3(0, i * 2 * scale, 0), Quaternion.identity);
            sphere.transform.localScale = new Vector3(scale * 0.8f, scale * 0.8f, scale * 0.8f); // Scale the sphere
            sphere.transform.SetParent(gameObject.transform); // Set the parent of the sphere
            nodes.Add(sphere);
        }
    }

    private void VisualizeConnections(Net net, int inputSize, int[] hiddenSizes)
    {
        int numLayers = hiddenSizes.Length + 2; // Input layer + hidden layers + output layer

        for (int i = 0; i < numLayers - 1; i++)
        {
            int currentLayerSize = (i == 0) ? inputSize : ((i == numLayers - 1) ? net.layers[i].size : hiddenSizes[i - 1]);
            int nextLayerSize = (i == numLayers - 2) ? net.layers[i + 1].size : hiddenSizes[i];

            for (int j = 0; j < currentLayerSize; j++)
            {
                for (int k = 0; k < nextLayerSize; k++)
                {
                    float weight = net.layers[i + 1].weights[k, j]; // Accessing weights from Net
                    DrawConnection(i, j, i + 1, k, weight, inputSize, hiddenSizes);
                }
            }
        }
    }

    private void DrawConnection(int layerIndex1, int nodeIndex1, int layerIndex2, int nodeIndex2, float weight, int inputSize, int[] hiddenSizes)
    {
        int startIndex, endIndex;

        // Calculate start index
        if (layerIndex1 == 0)
        {
            startIndex = nodeIndex1;
        }
        else
        {
            int hiddenOffset = inputSize;
            for (int i = 0; i < layerIndex1 - 1; i++)
            {
                hiddenOffset += hiddenSizes[i];
            }
            startIndex = hiddenOffset + nodeIndex1;
        }

        // Calculate end index
        if (layerIndex2 == hiddenSizes.Length + 1)
        {
            endIndex = inputSize + hiddenSizes.Sum() + nodeIndex2;
        }
        else
        {
            int hiddenOffset = inputSize;
            for (int i = 0; i < layerIndex2 - 1; i++)
            {
                hiddenOffset += hiddenSizes[i];
            }
            endIndex = hiddenOffset + nodeIndex2;
        }

        if (startIndex >= 0 && startIndex < nodes.Count && endIndex >= 0 && endIndex < nodes.Count)
        {
            Vector3 startPos = nodes[startIndex].transform.position;
            Vector3 endPos = nodes[endIndex].transform.position;

            GameObject connection = Instantiate(linePrefab, startPos, Quaternion.identity);
            connection.transform.SetParent(gameObject.transform); // Set the parent of the connection
            LineRenderer lineRenderer = connection.GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new Vector3[] { new Vector3(0, 0, 0), (endPos - startPos) * 10});
            lineRenderer.transform.SetParent(gameObject.transform); // Set the parent of the line renderer

            // Scale the line thickness based on the weight
            float thickness = Mathf.Abs(weight) * 0.1f * scale; // Example scaling factor
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
            lineRenderer.material.SetColor("_EmissionColor", lineRenderer.material.color * thickness); // Set the color based on the weight

            connections.Add(connection);
        }
        else
        {
            Debug.LogError("Invalid node indices for drawing connection.");
        }
    }

    private void ClearVisualization()
    {
        // Clear existing nodes and connections from the visualization
        foreach (var node in nodes)
        {
            Destroy(node);
        }
        nodes.Clear();

        foreach (var connection in connections)
        {
            Destroy(connection);
        }
        connections.Clear();
    }
}