using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The HuntingSystem class is a class that controls the hunting system of the creature, inherits from Net.
/// </summary>
public class HuntingSystem: Net
{
    private Quaternion _startRotation = Quaternion.identity;
    private Quaternion _huntingRotation = Quaternion.identity;
    private float _turningSpeed = 1f;
    private float _t;
    public float iq;
    public Color iqColor;

    public HuntingSystem(int[] networkStructure) : base(networkStructure) { }

    public HuntingSystem(Layer[] newLayers) : base(newLayers) { }

    public void AssessIQ()
    {
        float[] inputs = { Random.Range(-1f, 1f), Random.Range(-1f, 1f) };
        base.FeedForward(inputs);
        float[] outputs = GetOutput();
        // calculate r2 score
        float rss = Mathf.Pow(outputs[0] - inputs[0], 2) + Mathf.Pow(outputs[1] - inputs[1], 2);
        float yMean = (inputs[0] + inputs[1]) / 2;
        float tss = Mathf.Pow(inputs[0] - yMean, 2) + Mathf.Pow(inputs[1] - yMean, 2);
        float r2 = 1 - rss / tss;
        iq = r2;
        iqColor = Color.Lerp(Color.red, Color.green, r2);
    }
    

    /// <summary>
    /// FeedForward method feeds forward the inputs to the neural network.
    /// </summary>
    public void FeedForward(Transform transform, VisionCone vision)
    {
        // Get the target direction and target
        Vector3 targetDir = vision.TargetDir;
        Transform target = vision.Target;
        Quaternion foodLookRotation = transform.rotation;
        if (targetDir != Vector3.zero) foodLookRotation = Quaternion.LookRotation(targetDir);
        Vector3 foodRotationDifference = foodLookRotation.eulerAngles - transform.eulerAngles;
        
        // Normalize the rotation difference
        foodRotationDifference.x = ((foodRotationDifference.x + 180) % 360 - 180) / 180;
        foodRotationDifference.y = ((foodRotationDifference.y + 180) % 360 - 180) / 180;
        
        float[] inputs = { foodRotationDifference.x, foodRotationDifference.y };
        
        // Feed forward the inputs
        base.FeedForward(inputs);
        float[] outputs = GetOutput();
        Vector3 angleToRotate = new Vector3(outputs[0], outputs[1], 0);
        angleToRotate *= 180;
        
        // Rotate the creature
        _startRotation = transform.rotation;
        _huntingRotation = Quaternion.Euler(angleToRotate.x + transform.eulerAngles.x, angleToRotate.y + transform.eulerAngles.y, 0);
        _huntingRotation.eulerAngles = new Vector3(_huntingRotation.eulerAngles.x, _huntingRotation.eulerAngles.y, 0);
        _t = 0;
    }
    
    /// <summary>
    /// Rotate method rotates the creature towards the target.
    /// </summary>
    public void Rotate(Transform transform, float timeBetweenFeedForward=1f)
    {
        Debug.DrawRay(transform.position, _huntingRotation * Vector3.forward, Color.red);
        if (_t > 0.98f) _t = 0.99f;
        transform.rotation = Quaternion.Lerp(_startRotation, _huntingRotation, _t);
        _t += Time.deltaTime * Time.timeScale * _turningSpeed / timeBetweenFeedForward;
    }
    
    /// <summary>
    /// Mutate method mutates the neural network.
    /// </summary>
    public void Mutate()
    {
        MutateNet(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));
        AssessIQ();
    }
}
