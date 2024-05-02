using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// The NetVisualisation visualizes the neural network of an entity.
/// </summary>
public class NetVisualisation : MonoBehaviour
{
    public static Net net;
    public static bool isDirty = true;

    public GameObject spherePrefab;
    public GameObject linePrefab;

    public float scale = 1f; // Scale factor for the neural network visualization
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> connections = new List<GameObject>();

    public Material glowMaterial;

    void Start()
    {
    }
    
    private void Update()
    {
        VisualizeNetwork(net);

        // Rotate the network around its y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// VisualizeNetwork method visualizes the neural network of the entity.
    /// </summary>
    /// <param name="net"></param>
    private void VisualizeNetwork(Net net)
    {
        if (!isDirty) return;
        
        ClearVisualization();
        if (net == null)
        {
            return;
        }
        
        Debug.Log("222222222222222");

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
        isDirty = false;
    }

    /// <summary>
    /// CreateNodes method creates nodes for the neural network visualization.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="position"></param>
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

    /// <summary>
    /// VisualizeConnections method visualizes the connections between the nodes of the neural network.
    /// </summary>
    /// <param name="net"></param>
    /// <param name="inputSize"></param>
    /// <param name="hiddenSizes"></param>
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

    /// <summary>
    /// DrawConnection method draws a connection between two nodes of the neural network.
    /// </summary>
    /// <param name="layerIndex1"></param>
    /// <param name="nodeIndex1"></param>
    /// <param name="layerIndex2"></param>
    /// <param name="nodeIndex2"></param>
    /// <param name="weight"></param>
    /// <param name="inputSize"></param>
    /// <param name="hiddenSizes"></param>
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
            lineRenderer.material = glowMaterial;
            lineRenderer.material.SetColor("_EmissionColor", glowMaterial.color * Mathf.LinearToGammaSpace(Mathf.Abs(weight) * 100)); // Set the color based on the weight

            connections.Add(connection);  
        }
        else
        {
            Debug.LogError("Invalid node indices for drawing connection.");
        }
    }

    /// <summary>
    /// ClearVisualization method clears the existing nodes and connections from the visualization.
    /// </summary>
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